using AutoMapper;
using ELMS.WEB.Areas.Email.Models.EmailSchedule;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        public EmailScheduleController(IMapper mapper, IEmailScheduleManager emailScheduleManager, IEmailScheduleParameterManager emailScheduleParameterManager, IEmailTemplateManager emailTemplateManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __EmailScheduleManager = emailScheduleManager ?? throw new ArgumentNullException(nameof(emailScheduleManager));
            __EmailScheduleParameterManager = emailScheduleParameterManager ?? throw new ArgumentNullException(nameof(emailScheduleParameterManager));
            __EmailTemplateManager = emailTemplateManager ?? throw new ArgumentNullException(nameof(emailTemplateManager));
        }

        [HttpGet]
        public async Task<IActionResult> Index(string successMessage = "", string errorMessage = "")
        {
            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            IndexViewModel _Model = new IndexViewModel
            {
                EmailSchedules = __Mapper.Map<IList<EmailScheduleViewModel>>(await __EmailScheduleManager.GetAsync())
            };

            return View(_Model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateModalAsync()
        {
            IList<Models.EmailTemplate.EmailTemplateViewModel> _Templates = (__Mapper.Map<IList<Models.EmailTemplate.EmailTemplateViewModel>>((await __EmailTemplateManager.GetAsync())?.EmailTemplates)) ?? new List<Models.EmailTemplate.EmailTemplateViewModel>();

            if (_Templates.Count <= 0)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {ENTITY_NAME} because no Email templates exist." });
            }

            CreateCustomEmailScheduleViewModel _Model = new CreateCustomEmailScheduleViewModel
            {
                EmailTemplates = _Templates
            };

            return PartialView("_CreateModal", _Model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateCustomEmailScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
                model.EmailTemplates = __Mapper.Map<IList<Models.EmailTemplate.EmailTemplateViewModel>>((await __EmailTemplateManager.GetAsync())?.EmailTemplates);
                return PartialView("_CreateModal", model);
            }

            EmailScheduleResponse _Response = await __EmailScheduleManager.CreateAsync(new CreateEmailScheduleRequest
            {
                EmailTemplateUID = model.EmailTemplateUID,
                EmailType = Enums.Email.EmailType.Custom,
                RecipientEmailAddress = model.RecipientEmailAddress,
                SendTimestamp = model.SendTimestamp
            });

            if (!_Response.Success)
            {
                return Json(new { message = _Response.Message });
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}." });
        }

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

        [HttpGet]
        public async Task<IActionResult> ForceSendAsync(Guid uid)
        {
            return null;
        }
    }
}