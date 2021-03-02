using AutoMapper;
using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Areas.Equipment.Models.Note;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Equipment.Controllers
{
    [Authorize]
    [Area("Equipment")]
    public class EquipmentController : Controller
    {
        private readonly IMapper __Mapper;
        private readonly IEquipmentManager __EquipmentManager;
        private readonly INoteManager __NoteManager;
        private readonly IEmailScheduleManager __EmailScheduleManager;
        private readonly String ENTITY_NAME = "Equipment";

        public EquipmentController(IMapper mapper, IEquipmentManager equipmentManager, INoteManager noteManager, IEmailScheduleManager emailScheduleManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __EquipmentManager = equipmentManager ?? throw new ArgumentNullException(nameof(equipmentManager));
            __NoteManager = noteManager ?? throw new ArgumentNullException(nameof(noteManager));
            __EmailScheduleManager = emailScheduleManager ?? throw new ArgumentNullException(nameof(emailScheduleManager));
        }

        [Authorize(Policy = "ViewEquipmentPolicy")]
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

            IndexViewModel _Model = new IndexViewModel
            {
                Equipment = __Mapper.Map<IList<EquipmentViewModel>>((await __EquipmentManager.GetAsync()).Equipments),
            };

            return View(_Model);
        }

        [Authorize(Policy = "ViewEquipmentPolicy")]
        public async Task<IActionResult> FilterIndexAsync(FilterEquipmentViewModel filter)
        {
            if (filter == null)
            {
                return RedirectToAction("Index");
            }

            EquipmentListResponse _Equipment = await __EquipmentManager.GetAsync();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                _Equipment.Equipments = _Equipment.Equipments.Where(x => x.Name.ToUpper().Contains(filter.Name.ToUpper())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filter.Description))
            {
                _Equipment.Equipments = _Equipment.Equipments.Where(x => x.Name.ToUpper().Contains(filter.Description.ToUpper())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filter.SerialNumber))
            {
                _Equipment.Equipments = _Equipment.Equipments.Where(x => x.Name.ToUpper().Contains(filter.SerialNumber.ToUpper())).ToList();
            }

            if (filter.PurchasePriceFrom >= 0)
            {
                _Equipment.Equipments = _Equipment.Equipments.Where(x => x.PurchasePrice >= filter.PurchasePriceFrom).ToList();
            }

            if (filter.PurchasePriceFrom <= double.MaxValue)
            {
                _Equipment.Equipments = _Equipment.Equipments.Where(x => x.PurchasePrice <= filter.PurchasePriceTo).ToList();
            }

            if (filter.PurchaseDateFrom != null)
            {
                _Equipment.Equipments = _Equipment.Equipments.Where(x => x.PurchaseDate >= filter.PurchaseDateFrom).ToList();
            }

            if (filter.PurchaseDateTo != null)
            {
                _Equipment.Equipments = _Equipment.Equipments.Where(x => x.PurchaseDate <= filter.PurchaseDateTo).ToList();
            }

            if (filter.WarrantyExpirationDateFrom != null)
            {
                _Equipment.Equipments = _Equipment.Equipments.Where(x => x.WarrantyExpirationDate >= filter.WarrantyExpirationDateFrom).ToList();
            }

            if (filter.WarrantyExpirationDateTo != null)
            {
                _Equipment.Equipments = _Equipment.Equipments.Where(x => x.WarrantyExpirationDate <= filter.WarrantyExpirationDateTo).ToList();
            }

            if (filter.Statuses != null && filter.Statuses?.Count > 0)
            {
                _Equipment.Equipments = _Equipment.Equipments.Where(x => filter.Statuses.Contains(x.Status)).ToList();
            }

            return View("Index", new IndexViewModel
            {
                Filter = filter,
                Equipment = __Mapper.Map<IList<EquipmentViewModel>>(_Equipment.Equipments)
            });
        }

        [Authorize(Policy = "CreateEquipmentPolicy")]
        public async Task<IActionResult> CreateModalAsync()
        {
            CreateEquipmentViewModel _Model = new CreateEquipmentViewModel();
            return PartialView("_CreateEquipment", _Model);
        }

        [Authorize(Policy = "CreateEquipmentPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateEquipmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
                return PartialView("_CreateEquipment", model);
            }

            BaseResponse _Response = new BaseResponse();
            model.OwnerUID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (model.PurchasePrice != 0 && model.ReplacementPrice == 0)
            {
                model.ReplacementPrice = model.PurchasePrice;
            }

            if (model.Quantity <= 1)
            {
                EquipmentResponse _EquipmentResponse = await __EquipmentManager.CreateAsync(__Mapper.Map<CreateEquipmentRequest>(model));

                if (_EquipmentResponse.Success)
                {
                    await __EmailScheduleManager.CreateEquipmentWarrantyScheduleAsync(_EquipmentResponse, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}");
                }
            }
            else
            {
                IList<EquipmentResponse> _EquipmentResponses = await __EquipmentManager.BulkCreateAsync(__Mapper.Map<CreateEquipmentRequest>(model));

                if (_EquipmentResponses == null || _EquipmentResponses?.Count <= 0)
                {
                    return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {ENTITY_NAME}." });
                }

                await __EmailScheduleManager.BulkCreateEquipmentWarrantyScheduleAsync(_EquipmentResponses, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}");
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}." });
        }

        [Authorize(Policy = "EditEquipmentPolicy")]
        [HttpGet]
        public async Task<IActionResult> EditViewAsync(Guid equipmentUID)
        {
            EquipmentResponse _Response = await __EquipmentManager.GetAsync(equipmentUID);

            if (!_Response.Success)
            {
                return RedirectToAction("Index", new { errorMessage = _Response.Message });
            }

            return View("Edit", __Mapper.Map<UpdateEquipmentViewModel>(_Response));
        }

        [Authorize(Policy = "EditEquipmentPolicy")]
        [HttpPost]
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

        [Authorize(Policy = "ViewEquipmentPolicy")]
        [HttpGet]
        public async Task<IActionResult> DetailsViewAsync(Guid equipmentUID, string successMessage = "", string errorMessage = "")
        {
            if (equipmentUID == null || equipmentUID == Guid.Empty)
            {
                ViewData["ErrorMessage"] = "Invalid equipment UID";
            }

            if (!String.IsNullOrWhiteSpace(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }

            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            DetailsViewModel _Model = new DetailsViewModel
            {
                Equipment = __Mapper.Map<EquipmentViewModel>(await __EquipmentManager.GetAsync(equipmentUID)),
                Notes = __Mapper.Map<IList<NoteViewModel>>(await __NoteManager.GetAsync(equipmentUID))
            };

            return View("Details", _Model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DetailsModalAsync(Guid equipmentUID)
        {
            if (equipmentUID == null || equipmentUID == Guid.Empty)
            {
                ViewData["ErrorMessage"] = "Invalid equipment UID";
            }

            DetailsViewModel _Model = new DetailsViewModel
            {
                Equipment = __Mapper.Map<EquipmentViewModel>(await __EquipmentManager.GetAsync(equipmentUID)),
                Notes = __Mapper.Map<IList<NoteViewModel>>(await __NoteManager.GetAsync(equipmentUID))
            };

            return PartialView("_DetailsModal", _Model);
        }

        [Authorize(Policy = "DeleteEquipmentPolicy")]
        [HttpGet]
        public async Task<IActionResult> DeleteModalAsync(Guid equipmentUID)
        {
            EquipmentResponse _Response = await __EquipmentManager.GetAsync(equipmentUID);

            if (!_Response.Success)
            {
                return RedirectToAction("DetailsView", "Equipment", new { Area = "Equipment", errorMessage = _Response.Message });
            }

            return PartialView("_DeleteEquipment", __Mapper.Map<EquipmentViewModel>(_Response));
        }

        [Authorize(Policy = "DeleteEquipmentPolicy")]
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(DeleteEquipmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission.";
                return PartialView("_DeleteEquipment", model);
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