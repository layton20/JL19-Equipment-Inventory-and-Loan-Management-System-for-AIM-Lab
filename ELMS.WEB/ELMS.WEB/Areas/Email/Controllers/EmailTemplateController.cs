using AutoMapper;
using ELMS.WEB.Areas.Email.Models.EmailTemplate;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Request;
using ELMS.WEB.Models.Email.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Email.Controllers
{
    [Authorize]
    [Area("Email")]
    public class EmailTemplateController : Controller
    {
        private readonly IMapper __Mapper;
        private readonly IEmailTemplateManager __EmailTemplateManager;
        private readonly String ENTITY_NAME = "Email template";

        public EmailTemplateController(IMapper mapper, IEmailTemplateManager emailTemplateManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __EmailTemplateManager = emailTemplateManager ?? throw new ArgumentNullException(nameof(emailTemplateManager));
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(string successMessage = "", string errorMessage = "")
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
                EmailTemplates = __Mapper.Map<IList<EmailTemplateViewModel>>((await __EmailTemplateManager.GetAsync()).EmailTemplates)
            };

            return View("Index", _Model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateViewAsync()
        {
            CreateEmailTemplateViewModel _Model = new CreateEmailTemplateViewModel();
            return View("Create", _Model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateEmailTemplateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
                return PartialView("Create", model);
            }

            model.OwnerUID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            EmailTemplateResponse _Response = await __EmailTemplateManager.CreateAsync(__Mapper.Map<CreateEmailTemplateRequest>(model));

            if (!_Response.Success)
            {
                return RedirectToAction("Index", new { error = _Response.Message });
            }
            
            return RedirectToAction("Index", new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}." });
        }

        [HttpGet]
        public async Task<IActionResult> EditViewAsync(Guid uid)
        {
            EmailTemplateResponse _Response = await __EmailTemplateManager.GetByUIDAsync(uid);

            if (_Response == null)
            {
                return RedirectToAction("Index", new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            return View("Edit", __Mapper.Map<EmailTemplateViewModel>(_Response));
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EmailTemplateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
            }

            BaseResponse _Response = await __EmailTemplateManager.UpdateAsync(__Mapper.Map<UpdateEmailTemplateRequest>(model));

            if (!_Response.Success)
            {
                return RedirectToAction("Index", new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            return RedirectToAction("Index", new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}." });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteModalAsync(Guid uid)
        {
            EmailTemplateResponse _Response = await __EmailTemplateManager.GetByUIDAsync(uid);

            if (!_Response.Success)
            {
                return await IndexAsync();
            }

            return PartialView("_DeleteModal", __Mapper.Map<EmailTemplateViewModel>(_Response));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(Guid uid)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission.";
                return RedirectToAction("Index", "EmailTemplate", new { Area = "Email", errorMessage = "Invalid form submission." });
            }

            BaseResponse _Response = await __EmailTemplateManager.DeleteAsync(uid);

            if (!_Response.Success)
            {
                return Json(new { error = _Response.Message });
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}." });
        }

        [HttpGet]
        public async Task<IActionResult> DetailsModalAsync(Guid uid)
        {
            EmailTemplateResponse _Response = await __EmailTemplateManager.GetByUIDAsync(uid);
            return PartialView("_DetailsModal", __Mapper.Map<EmailTemplateViewModel>(_Response));
        }
    }
}