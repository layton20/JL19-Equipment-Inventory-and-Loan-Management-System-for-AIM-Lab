using ELMS.WEB.Adapters.Email;
using ELMS.WEB.Areas.Email.Models.EmailSchedule;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Response;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IEmailScheduleManager __EmailScheduleManager;
        private readonly IEmailTemplateManager __EmailTemplateManager;
        private readonly String ENTITY_NAME = "Email schedule";

        public EmailScheduleController(IEmailScheduleManager emailScheduleManager, IEmailTemplateManager emailTemplateManager)
        {
            __EmailScheduleManager = emailScheduleManager ?? throw new ArgumentNullException(nameof(emailScheduleManager));
            __EmailTemplateManager = emailTemplateManager ?? throw new ArgumentNullException(nameof(emailTemplateManager));
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IList<EmailScheduleViewModel> _EmailSchedules = (await __EmailScheduleManager.GetAsync()).Responses.ToViewModel();

            foreach (EmailScheduleViewModel schedule in _EmailSchedules)
            {
                schedule.EmailTemplate = (await __EmailTemplateManager.GetByUIDAsync(schedule.EmailTemplateUID)).ToViewModel();
            }

            IndexViewModel _Model = new IndexViewModel
            {
                EmailSchedules = _EmailSchedules
            };

            return View(_Model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateModalAsync()
        {
            EmailTemplatesResponse _Responses = await __EmailTemplateManager.GetAsync();
            IList<EmailTemplateResponse> _EmailTemplates = (await __EmailTemplateManager.GetAsync()).EmailTemplates;
            CreateEmailScheduleViewModel _Model = new CreateEmailScheduleViewModel
            {
                EmailTemplates = _EmailTemplates.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.UID.ToString()
                }).ToList()
            };

            return PartialView("_CreateModal", _Model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateEmailScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
                IList<EmailTemplateResponse> _EmailTemplates = (await __EmailTemplateManager.GetAsync()).EmailTemplates;
                model.EmailTemplates = _EmailTemplates.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.UID.ToString()
                }).ToList();

                return PartialView("_CreateModal", model);
            }

            EmailScheduleResponse _Response = await __EmailScheduleManager.CreateAsync(model.ToRequest());

            if (!_Response.Success)
            {
                ModelState.AddModelError("Error", _Response.Message);
                ViewData["ErrorMessage"] = "Invalid form submission";
                return PartialView("_CreateModal", model);
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}" });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteModalAsync(Guid uid)
        {
            EmailScheduleResponse _Response = await __EmailScheduleManager.GetByUIDAsync(uid);

            if (!_Response.Success)
            {
                ModelState.AddModelError("Error", _Response.Message);
                ViewData["ErrorMessage"] = "Invalid form submission";
            }

            EmailScheduleViewModel _Model = _Response.ToViewModel();
            _Model.EmailTemplate = (await __EmailTemplateManager.GetByUIDAsync(_Response.EmailTemplateUID)).ToViewModel();

            return PartialView("_DeleteModal", _Model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(EmailScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission.";
                return PartialView("_DeleteModal", model);
            }

            BaseResponse _Response = await __EmailScheduleManager.DeleteAsync(model.UID);

            if (!_Response.Success)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} created {ENTITY_NAME}" });
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}" });
        }
    }
}