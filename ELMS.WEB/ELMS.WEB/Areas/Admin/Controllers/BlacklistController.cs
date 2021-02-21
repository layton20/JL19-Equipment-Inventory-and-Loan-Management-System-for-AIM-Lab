using AutoMapper;
using ELMS.WEB.Areas.Admin.Models.Blacklist;
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
    public class BlacklistController : Controller
    {
        private readonly IMapper __Mapper;
        private readonly IBlacklistManager __BlacklistManager;
        private const string ENTITY_NAME = "Blacklist";

        public BlacklistController(IMapper mapper, IBlacklistManager blacklistManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __BlacklistManager = blacklistManager ?? throw new ArgumentException(nameof(blacklistManager));
        }

        public async Task<IActionResult> IndexAsync(string successMessage = "", string errorMessage = "")
        {
            if (!String.IsNullOrWhiteSpace(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }
            else if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            IndexViewModel _Model = new IndexViewModel
            {
                Blacklists = __Mapper.Map<IList<BlacklistViewModel>>(await __BlacklistManager.GetAsync())
            };

            return View(_Model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateModalAsync()
        {
            return PartialView("_CreateModal", new CreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CreateModal", model);
            }

            BlacklistResponse _Response = await __BlacklistManager.CreateAsync(__Mapper.Map<CreateBlacklistRequest>(model));

            if (!_Response.Success)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {ENTITY_NAME}" });
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}." });
        }

        [HttpGet]
        public async Task<IActionResult> EditModalAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}" });
            }

            BlacklistResponse _Response = await __BlacklistManager.GetByUIDAsync(uid);

            if (_Response == null)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}" });
            }

            UpdateViewModel _Model = new UpdateViewModel
            {
                Email = _Response.Email,
                Reason = _Response.Reason,
                Type = _Response.Type,
                UID = _Response.UID,
                Active = _Response.Active
            };

            return PartialView("_EditModal", _Model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(UpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_EditModal", model);
            }

            BlacklistResponse _Blacklist = await __BlacklistManager.GetByUIDAsync(model.UID);

            if (_Blacklist == null)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            BaseResponse _Response = await __BlacklistManager.UpdateAsync(__Mapper.Map<UpdateBlacklistRequest>(model));

            if (_Response == null)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} update {ENTITY_NAME}." });
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} update {ENTITY_NAME}." });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteModalAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}" });
            }

            BlacklistResponse _Blacklist = await __BlacklistManager.GetByUIDAsync(uid);

            if (_Blacklist == null)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {ENTITY_NAME}" });
            }

            return PartialView("_DeleteModal", __Mapper.Map<DeleteViewModel>(_Blacklist));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(DeleteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_DeleteModal", model);
            }

            BaseResponse _Response = await __BlacklistManager.DeleteAsync(model.UID);

            if (!_Response.Success)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {ENTITY_NAME}." });
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} deleted {ENTITY_NAME}." });
        }
    }
}