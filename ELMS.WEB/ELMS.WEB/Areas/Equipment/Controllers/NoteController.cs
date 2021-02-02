using ELMS.WEB.Adapters.Equipment;
using ELMS.WEB.Areas.Equipment.Models.Note;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Equipment.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Equipment.Controllers
{
    [Authorize]
    [Area("Equipment")]
    public class NoteController : Controller
    {
        private readonly INoteManager __NoteManager;
        private readonly IEquipmentManager __EquipmentManager;
        private readonly String ENTITY_NAME = "Note";

        public NoteController(INoteManager noteManager, IEquipmentManager equipmentManager)
        {
            __NoteManager = noteManager ?? throw new ArgumentNullException(nameof(noteManager));
            __EquipmentManager = equipmentManager ?? throw new ArgumentNullException(nameof(noteManager));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> CreateModalAsync(Guid equipmentUID)
        {
            if (equipmentUID == Guid.Empty)
            {
                return Json(0);
            }

            CreateNoteViewModel _Model = new CreateNoteViewModel
            {
                EquipmentUID = equipmentUID
            };

            return PartialView("_CreateNote", _Model);
        }

        [HttpGet]
        public async Task<IActionResult> EditModalAsync(Guid NoteUID)
        {
            NoteResponse _Response = await __NoteManager.GetByUIDAsync(NoteUID);

            if (_Response == null)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}" });
            }

            return PartialView("_EditNote", _Response.ToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateNoteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
                return PartialView("_CreateNote", model);
            }

            BaseResponse _Response = new BaseResponse();
            model.OwnerUID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _Response = await __NoteManager.CreateAsync(model.ToRequest());

            if (!_Response.Success)
            {
                ModelState.AddModelError("Error", _Response.Message);
                return await CreateModalAsync(model.EquipmentUID);
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}." });
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(NoteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
            }

            BaseResponse _Response = await __NoteManager.UpdateAsync(model.ToRequest());

            if (!_Response.Success)
            {
                ViewData["ErrorMessage"] = _Response.Message;
            }
            else
            {
                ViewData["SuccessMessage"] = _Response.Message;
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} updated {ENTITY_NAME}." });
        }

        [HttpGet]
        public async Task<IActionResult> DeleteModalAsync(Guid noteUID)
        {
            NoteResponse _Response = await __NoteManager.GetByUIDAsync(noteUID);

            if (!_Response.Success)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            return PartialView("_DeleteNote", _Response.ToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(DeleteNoteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission.";
                return PartialView("_DeleteNote", model);
            }

            BaseResponse _Response = await __NoteManager.DeleteAsync(model.UID);

            if (!_Response.Success)
            {
                return RedirectToAction("DetailsView", "Equipment", new { Area = "Equipment", model.EquipmentUID, errorMessage = _Response.Message });
            }

            return RedirectToAction("DetailsView", "Equipment", new { Area = "Equipment", model.EquipmentUID, successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} deleted {ENTITY_NAME}." });
        }
    }
}