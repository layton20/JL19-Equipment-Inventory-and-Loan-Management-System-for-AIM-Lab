using ELMS.WEB.Adapters.Equipment;
using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Equipment.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Equipment.Controllers
{
    [Area("Equipment")]
    public class EquipmentController : Controller
    {
        private readonly IEquipmentManager __EquipmentManager;
        private readonly String ENTITY_NAME = "Equipment";

        public EquipmentController(IEquipmentManager equipmentManager)
        {
            __EquipmentManager = equipmentManager ?? throw new ArgumentNullException(nameof(equipmentManager));
        }

        public async Task<IActionResult> IndexAsync(String errorMessage, String successMessage)
        {
            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }
            else if (!String.IsNullOrWhiteSpace(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }

            IndexViewModel model = new IndexViewModel
            {
                Equipment = (await __EquipmentManager.GetAsync()).Equipments.ToViewModel()
            };

            return View(model);
        }

        public async Task<IActionResult> CreateViewAsync()
        {
            CreateEquipmentViewModel model = new CreateEquipmentViewModel();
            return View("Create");
        }

        public async Task<IActionResult> CreateModalAsync()
        {
            CreateEquipmentViewModel model = new CreateEquipmentViewModel();
            return PartialView("_CreateEquipment", model);
        }

        public async Task<IActionResult> CreateAsync(CreateEquipmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
                return View("Create", model);
            }

            EquipmentResponse _Response = await __EquipmentManager.CreateAsync(model.ToRequest());

            if (!_Response.Success)
            {
                ViewData["ErrorMessage"] = _Response.Message;
            }

            return RedirectToAction("Index", new { successMessage = _Response.Message });
        }

        public async Task<IActionResult> EditViewAsync(Guid equipmentUID)
        {
            EquipmentResponse _Response = await __EquipmentManager.GetAsync(equipmentUID);

            if (!_Response.Success)
            {
                return RedirectToAction("Index", new { errorMessage = _Response.Message });
            }

            return View("Edit", _Response.ToUpdateViewModel());
        }

        public async Task<IActionResult> EditAsync(DetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
            }

            BaseResponse _Response = await __EquipmentManager.UpdateAsync(model.Equipment);

            if (!_Response.Success)
            {
                ViewData["ErrorMessage"] = _Response.Message;
            }
            else
            {
                ViewData["SuccessMessage"] = _Response.Message;
            }

            return await DetailsViewAsync(model.Equipment.UID);
        }

        public async Task<IActionResult> DetailsViewAsync(Guid equipmentUID)
        {
            if (equipmentUID == null || equipmentUID == Guid.Empty)
            {
                ViewData["ErrorMessage"] = "Invalid equipment UID";
            }

            EquipmentResponse _Response = await __EquipmentManager.GetAsync(equipmentUID);

            DetailsViewModel _Model = new DetailsViewModel
            {
                Equipment = _Response.ToViewModel()
            };

            return View("Details", _Model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteModalAsync(Guid equipmentUID)
        {
             EquipmentResponse _Response = await __EquipmentManager.GetAsync(equipmentUID);

            if (!_Response.Success)
            {
                return RedirectToAction("DetailsView", "Equipment", new { Area = "Equipment", errorMessage = _Response.Message });
            }

            return PartialView("_DeleteEquipment", _Response.ToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(DeleteEquipmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission.";
                return PartialView("_DeleteStock", model);
            }

            BaseResponse _Response = await __EquipmentManager.DeleteAsync(model);

            if (!_Response.Success)
            {
                return RedirectToAction("Index", "Equipment", new { Area = "Equipment", errorMessage = _Response.Message });
            }

            return RedirectToAction("Index", "Equipment", new { Area = "Equipment", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} deleted {ENTITY_NAME}" });
        }
    }
}