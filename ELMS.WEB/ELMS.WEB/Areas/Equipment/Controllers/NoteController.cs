using ELMS.WEB.Adapters.Equipment;
using ELMS.WEB.Areas.Equipment.Models.Note;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Models.Base.Response;
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
        private readonly String ENTITY_NAME = "Note";

        public NoteController(INoteManager noteManager)
        {
            __NoteManager = noteManager ?? throw new ArgumentNullException(nameof(noteManager));
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

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}" });
        }
    }
}