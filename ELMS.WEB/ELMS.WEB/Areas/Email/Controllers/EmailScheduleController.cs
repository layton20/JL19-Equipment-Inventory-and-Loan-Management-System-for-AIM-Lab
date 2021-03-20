using AutoMapper;
using ELMS.WEB.Areas.Email.Models.EmailSchedule;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using ELMS.WEB.Repositories.Identity.Interface;
using ELMS.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Email.Controllers
{
    [Authorize]
    [Area("Email")]
    public class EmailScheduleController : Controller
    {
        private readonly String ENTITY_NAME = "Email schedule";
        private readonly IMapper __Mapper;
        private readonly IEmailScheduleManager __EmailScheduleManager;
        private readonly IEmailScheduleParameterManager __EmailScheduleParameterManager;
        private readonly IEmailTemplateManager __EmailTemplateManager;
        private readonly IApplicationEmailSender __EmailSender;
        private readonly UserManager<IdentityUser> __UserManager;
        private readonly IUserRepository __UserRepository;

        public EmailScheduleController(IMapper mapper, IEmailScheduleManager emailScheduleManager,
            IEmailScheduleParameterManager emailScheduleParameterManager, IEmailTemplateManager emailTemplateManager,
            IApplicationEmailSender applicationEmailSender, UserManager<IdentityUser> userManager, IUserRepository userRepository)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __EmailScheduleManager = emailScheduleManager ?? throw new ArgumentNullException(nameof(emailScheduleManager));
            __EmailScheduleParameterManager = emailScheduleParameterManager ?? throw new ArgumentNullException(nameof(emailScheduleParameterManager));
            __EmailTemplateManager = emailTemplateManager ?? throw new ArgumentNullException(nameof(emailTemplateManager));
            __EmailSender = applicationEmailSender ?? throw new ArgumentNullException(nameof(applicationEmailSender));
            __UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            __UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [Authorize(Policy = "ViewEmailSchedulePolicy")]
        [HttpGet]
        public async Task<IActionResult> Index(string successMessage = "", string errorMessage = "")
        {
            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }
            else if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            IndexViewModel _Model = new IndexViewModel
            {
                EmailSchedules = __Mapper.Map<IList<EmailScheduleViewModel>>(await __EmailScheduleManager.GetAsync()),
                Filter = new EmailScheduleFilterViewModel
                {
                    EmailTemplatesSelectList = (await __EmailTemplateManager.GetAsync()).EmailTemplates.Select(x => new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.UID.ToString()
                    }).ToList()
                }
            };

            return View(_Model);
        }

        [Authorize(Policy = "ViewEmailSchedulePolicy")]
        [HttpPost]
        public async Task<IActionResult> FilterAsync(IndexViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            IList<EmailScheduleResponse> _EmailScheduleResponses = await __EmailScheduleManager.GetAsync();

            if (!string.IsNullOrWhiteSpace(model.Filter.RecipientEmailAddress))
            {
                _EmailScheduleResponses = _EmailScheduleResponses.Where(x => x.RecipientEmailAddress.ToUpper().Contains(model.Filter.RecipientEmailAddress.ToUpper())).ToList();
            }

            if (model.Filter.EmailSent == Enums.General.BooleanFilter.True)
            {
                _EmailScheduleResponses = _EmailScheduleResponses.Where(x => x.Sent).ToList();
            }
            else if (model.Filter.EmailSent == Enums.General.BooleanFilter.False)
            {
                _EmailScheduleResponses = _EmailScheduleResponses.Where(x => !x.Sent).ToList();
            }

            if (model.Filter.ScheduledForFrom != DateTime.MinValue)
            {
                _EmailScheduleResponses = _EmailScheduleResponses.Where(x => x.SendTimestamp >= model.Filter.ScheduledForFrom).ToList();
            }

            if (model.Filter.ScheduledForTo != DateTime.MaxValue)
            {
                _EmailScheduleResponses = _EmailScheduleResponses.Where(x => x.SendTimestamp <= model.Filter.ScheduledForTo).ToList();
            }

            if (model.Filter.EmailTemplateUIs != null && model.Filter.EmailTemplateUIs.Count > 0)
            {
                _EmailScheduleResponses = _EmailScheduleResponses.Where(x => model.Filter.EmailTemplateUIs.Contains(x.EmailTemplateUID)).ToList();
            }

            if (model.Filter.EmailTypes != null && model.Filter.EmailTypes.Count > 0)
            {
                _EmailScheduleResponses = _EmailScheduleResponses.Where(x => model.Filter.EmailTypes.Contains(x.EmailType)).ToList();
            }

            return View("Index", new IndexViewModel
            {
                EmailSchedules = __Mapper.Map<IList<EmailScheduleViewModel>>(_EmailScheduleResponses),
                Filter = model.Filter
            });
        }

        [Authorize(Policy = "DeleteEmailSchedulePolicy")]
        [HttpGet]
        public async Task<IActionResult> DeleteModalAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return RedirectToAction("Index", "EmailSchedule", new { Area = "Email", errorMessage = "Unable to find Email Schedule." });
            }

            EmailScheduleResponse _Response = await __EmailScheduleManager.GetByUIDAsync(uid);

            if (!_Response.Success)
            {
                return RedirectToAction("Index", "EmailSchedule", new { Area = "Email", errorMessage = "Unable to find Email Schedule." });
            }

            DeleteEmailScheduleViewModel _Model = new DeleteEmailScheduleViewModel
            {
                UID = _Response.UID,
                RecipientEmail = _Response.RecipientEmailAddress,
                EmailType = _Response.EmailType,
                SendTimestamp = _Response.SendTimestamp,
                Sent = _Response.Sent
            };

            return PartialView("_DeleteModal", _Model);
        }

        [Authorize(Policy = "DeleteEmailSchedulePolicy")]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(DeleteEmailScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_DeleteModal", model);
            }

            BaseResponse _Response = await __EmailScheduleManager.DeleteAsync(model.UID);
            await __EmailScheduleParameterManager.DeleteAsync(model.UID);

            if (!_Response.Success)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {ENTITY_NAME}." });
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} deleted {ENTITY_NAME}." });
        }

        [Authorize(Policy = "SendEmailSchedulePolicy")]
        [HttpGet]
        public async Task<IActionResult> ForceSendModalAsync(Guid uid)
        {
            EmailScheduleResponse _ScheduleResponse = await __EmailScheduleManager.GetByUIDAsync(uid);

            if (!_ScheduleResponse.Success)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            return PartialView("_ForceSendModal", new ForceSendScheduleViewModel
            {
                UID = uid,
                RecipientEmailAddress = _ScheduleResponse.RecipientEmailAddress
            });
        }

        [Authorize(Policy = "SendEmailSchedulePolicy")]
        [HttpPost]
        public async Task<IActionResult> ForceSendAsync(ForceSendScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} send {ENTITY_NAME}." });
            }

            EmailScheduleResponse _ScheduleResponse = await __EmailScheduleManager.GetByUIDAsync(model.UID);

            if (!_ScheduleResponse.Success)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            if (!model.RetainSchedule)
            {
                await __EmailScheduleManager.SendScheduledEmail(_ScheduleResponse);
            }
            else
            {
                await __EmailScheduleManager.SendScheduledEmail(_ScheduleResponse, false);
            }

            if (model.SendCopyToSelf)
            {
                _ScheduleResponse.RecipientEmailAddress = __UserManager.GetUserAsync(HttpContext.User).Result.Email;
                await __EmailScheduleManager.SendScheduledEmail(_ScheduleResponse, false);
            }

            return RedirectToAction("Index", "EmailSchedule", new { Area = "Email", successMessage = $"Successfully attempted to send {ENTITY_NAME}s." });
        }

        [Authorize(Policy = "CreateEmailSchedulePolicy")]
        [HttpGet]
        public async Task<IActionResult> CreateViewAsync()
        {
            IList<EmailTemplateResponse> _Templates = (await __EmailTemplateManager.GetAsync())?.EmailTemplates ?? new List<EmailTemplateResponse>();

            CreateViewModel _Model = new CreateViewModel
            {
                EmailTemplates = __Mapper.Map<IList<Models.EmailTemplate.EmailTemplateViewModel>>(_Templates),
                Users = await __UserRepository.GetAsync()
            };

            return View("Create", _Model);
        }

        [Authorize(Policy = "CreateEmailSchedulePolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                IList<EmailTemplateResponse> _Templates = (await __EmailTemplateManager.GetAsync())?.EmailTemplates ?? new List<EmailTemplateResponse>();

                model.EmailTemplates = __Mapper.Map<IList<Models.EmailTemplate.EmailTemplateViewModel>>(_Templates);
                model.Users = await __UserRepository.GetAsync();

                return View("Create", model);
            }

            IList<CreateEmailScheduleRequest> _Requests = new List<CreateEmailScheduleRequest>();

            await __EmailScheduleManager.BulkCreateAsync(model.SelectedRecipientEmailAddresses.Select(x =>
            new CreateEmailScheduleRequest
            {
                EmailTemplateUID = model.SelectedEmailTemplate,
                EmailType = Enums.Email.EmailType.Custom,
                RecipientEmailAddress = x,
                SendTimestamp = model.SendTimestamp
            }
            ).ToList());

            return RedirectToAction("Index", new { successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} sent {ENTITY_NAME}s." });
        }
    }
}