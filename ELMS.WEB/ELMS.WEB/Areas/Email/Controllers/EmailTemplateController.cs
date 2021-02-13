using ELMS.WEB.Adapters.Email;
using ELMS.WEB.Areas.Email.Models.EmailTemplate;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Email.Controllers
{
    [Authorize]
    [Area("Email")]
    public class EmailTemplateController : Controller
    {
        private readonly IEmailTemplateManager __EmailTemplateManager;
        private readonly String ENTITY_NAME = "Email template";

        public EmailTemplateController(IEmailTemplateManager emailTemplateManager)
        {
            __EmailTemplateManager = emailTemplateManager ?? throw new ArgumentNullException(nameof(emailTemplateManager));
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            IndexViewModel _Model = new IndexViewModel
            {
                EmailTemplates = (await __EmailTemplateManager.GetAsync()).EmailTemplates.ToViewModel()
            };

            return View("Index", _Model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateModalAsync()
        {
            CreateEmailTemplateViewModel _Model = new CreateEmailTemplateViewModel();
            return PartialView("_CreateModal", _Model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateEmailTemplateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
                return PartialView("_CreateModal", model);
            }

            model.OwnerUID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            EmailTemplateResponse _Response = await __EmailTemplateManager.CreateAsync(model.ToRequest());

            if (!_Response.Success)
            {
                ModelState.AddModelError("Error", _Response.Message);
                return await CreateModalAsync();
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}." });
        }

        [HttpGet]
        public async Task<IActionResult> EditModalAsync(Guid uid)
        {
            EmailTemplateResponse _Response = await __EmailTemplateManager.GetByUIDAsync(uid);

            if (_Response == null)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}" });
            }

            return PartialView("_EditModal", _Response.ToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EmailTemplateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
            }

            BaseResponse _Response = await __EmailTemplateManager.UpdateAsync(model.ToRequest());

            if (!_Response.Success)
            {
                ViewData["ErrorMessage"] = _Response.Message;
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} update {ENTITY_NAME}" });
            }

            ViewData["SuccessMessage"] = _Response.Message;
            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} update {ENTITY_NAME}" });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteModalAsync(Guid uid)
        {
            EmailTemplateResponse _Response = await __EmailTemplateManager.GetByUIDAsync(uid);

            if (!_Response.Success)
            {
                return await IndexAsync();
            }

            return PartialView("_DeleteModal", _Response.ToViewModel());
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
                return RedirectToAction("Index", "EmailTemplate", new { Area = "Email", errorMessage = _Response.Message });
            }

            return RedirectToAction("Index", "EmailTemplate", new { Area = "Email", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} deleted {ENTITY_NAME}" });
        }

        [HttpGet]
        public async Task<IActionResult> DetailsModalAsync(Guid uid)
        {
            EmailTemplateResponse _Response = await __EmailTemplateManager.GetByUIDAsync(uid);
            return PartialView("_DetailsModal", _Response.ToViewModel());
        }
    }
}