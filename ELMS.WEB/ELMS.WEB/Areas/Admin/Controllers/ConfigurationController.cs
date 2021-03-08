using AutoMapper;
using ELMS.WEB.Areas.Admin.Models.Configuration;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Admin.Interfaces;
using ELMS.WEB.Models.Admin.Request;
using ELMS.WEB.Models.Admin.Response;
using ELMS.WEB.Models.Base.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class ConfigurationController : Controller
    {
        private readonly IMapper __Mapper;
        private readonly IConfigurationManager __ConfigurationManager;
        private readonly string ENTITY_NAME = "Configuration";

        public ConfigurationController(IMapper mapper, IConfigurationManager configurationManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __ConfigurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
        }

        [HttpGet]
        [Authorize(Policy = "ViewConfigurationPolicy")]
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

            return View("Index", new IndexViewModel
            {
                Configurations = __Mapper.Map<IList<ConfigurationViewModel>>(await __ConfigurationManager.GetAsync())
            });
        }

        [HttpGet]
        [Authorize(Policy = "CreateConfigurationPolicy")]
        public async Task<IActionResult> CreateViewAsync()
        {
            return PartialView("Create", new CreateConfigurationViewModel());
        }

        [HttpPost]
        [Authorize(Policy = "CreateConfigurationPolicy")]
        public async Task<IActionResult> CreateAsync(CreateConfigurationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
                return PartialView("_CreateModal", model);
            }

            ConfigurationResponse _Response = await __ConfigurationManager.CreateAsync(__Mapper.Map<CreateConfigurationRequest>(model));

            if (!_Response.Success)
            {
                return RedirectToAction("Index", "Configuration", new { area = "Admin", successMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {ENTITY_NAME}." });
            }

            return RedirectToAction("Index", "Configuration", new { area = "Admin", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}." });
        }

        [HttpGet]
        [Authorize(Policy = "EditConfigurationPolicy")]
        public async Task<IActionResult> EditViewAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            ConfigurationResponse _Response = await __ConfigurationManager.GetByUIDAsync(uid);

            if (!_Response.Success)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            return View("Edit", __Mapper.Map<UpdateConfigurationViewModel>(_Response));
        }

        [HttpPost]
        [Authorize(Policy = "EditConfigurationPolicy")]
        public async Task<IActionResult> EditAsync(UpdateConfigurationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
                return PartialView("_CreateModal", model);
            }

            ConfigurationResponse _ConfigurationResponse = await __ConfigurationManager.GetByUIDAsync(model.UID);

            if (!_ConfigurationResponse.Success)
            {
                return RedirectToAction("Index", "Configuration", new { area = "Admin", successMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} update {ENTITY_NAME}." });
            }

            BaseResponse _UpdateResponse = await __ConfigurationManager.UpdateAsync(__Mapper.Map<UpdateConfigurationRequest>(model));

            if (!_UpdateResponse.Success)
            {
                return RedirectToAction("Index", "Configuration", new { area = "Admin", successMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} update {ENTITY_NAME}." });
            }

            return RedirectToAction("Index", "Configuration", new { area = "Admin", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} updated {ENTITY_NAME}." });
        }

        [HttpGet]
        [Authorize(Policy = "DeleteConfigurationPolicy")]
        public async Task<IActionResult> DeleteModalAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            ConfigurationResponse _Response = await __ConfigurationManager.GetByUIDAsync(uid);

            if (!_Response.Success)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            return PartialView("_DeleteModal", new DeleteConfigurationViewModel
            {
                UID = uid,
                Name = _Response.Name,
                Value = _Response.Value
            });
        }

        [HttpPost]
        [Authorize(Policy = "DeleteConfigurationPolicy")]
        public async Task<IActionResult> DeleteAsync(DeleteConfigurationViewModel model)
        {
            if (model == null || model.UID == Guid.Empty)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {ENTITY_NAME}." });
            }

            ConfigurationResponse _ConfigurationResponse = await __ConfigurationManager.GetByUIDAsync(model.UID);

            if (!_ConfigurationResponse.Success)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {ENTITY_NAME}." });
            }
            else if (model.VerificationName != _ConfigurationResponse.Name)
            {
                ModelState.AddModelError("Error", "Incorrectly entered configuration name.");
                return PartialView("_DeleteModal", model);
            }

            BaseResponse _DeleteResponse = await __ConfigurationManager.DeleteAsync(model.UID);

            if (!_DeleteResponse.Success)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {ENTITY_NAME}." });
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} deleted {ENTITY_NAME}." });
        }
    }
}