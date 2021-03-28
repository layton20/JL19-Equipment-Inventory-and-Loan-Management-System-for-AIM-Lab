using AutoMapper;
using ELMS.WEB.Areas.Equipment.Models;
using ELMS.WEB.Areas.Equipment.Models.Note;
using ELMS.WEB.Areas.General.Models.Media;
using ELMS.WEB.Enums.Equipment;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.General.Interface;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Equipment.Request;
using ELMS.WEB.Models.Equipment.Response;
using ELMS.WEB.Models.General.Request;
using ELMS.WEB.Models.General.Response;
using ELMS.WEB.Models.Loan.Response;
using ELMS.WEB.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IBlobManager __BlobManager;
        private readonly IEquipmentBlobManager __EquipmentBlobManager;
        private readonly IBlobService __BlobService;
        private readonly ILoanManager __LoanManager;
        private readonly ILoanEquipmentManager __LoanEquipmentManager;
        private readonly String ENTITY_NAME = "Equipment";

        public EquipmentController(IMapper mapper, IEquipmentManager equipmentManager,
            INoteManager noteManager, IEmailScheduleManager emailScheduleManager,
            IBlobManager blobManager, IEquipmentBlobManager equipmentBlobManager,
            IBlobService blobService, ILoanManager loanManager, ILoanEquipmentManager loanEquipmentManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __EquipmentManager = equipmentManager ?? throw new ArgumentNullException(nameof(equipmentManager));
            __NoteManager = noteManager ?? throw new ArgumentNullException(nameof(noteManager));
            __EmailScheduleManager = emailScheduleManager ?? throw new ArgumentNullException(nameof(emailScheduleManager));
            __BlobManager = blobManager ?? throw new ArgumentNullException(nameof(blobManager));
            __EquipmentBlobManager = equipmentBlobManager ?? throw new ArgumentNullException(nameof(equipmentBlobManager));
            __BlobService = blobService ?? throw new ArgumentNullException(nameof(blobService));
            __LoanManager = loanManager ?? throw new ArgumentNullException(nameof(loanManager));
            __LoanEquipmentManager = loanEquipmentManager ?? throw new ArgumentNullException(nameof(loanEquipmentManager));
        }

        [Authorize(Policy = "ViewEquipmentPolicy")]
        [HttpGet]
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

            IList<EquipmentViewModel> _EquipmentList = __Mapper.Map<IList<EquipmentViewModel>>(await __EquipmentManager.GetAsync());
            foreach (EquipmentViewModel equipment in _EquipmentList)
            {
                equipment.Blobs = (await __EquipmentBlobManager.GetAsync(equipment.UID)).Select(x => x.Blob).ToList();
            }

            return View("Index", new IndexViewModel
            {
                Equipment = _EquipmentList,
                AvailableEquipmentCount = _EquipmentList.Where(x => x.Status == Status.Available).Count(),
                OnLoanEquipmentCount = _EquipmentList.Where(x => x.Status == Status.PendingLoan || x.Status == Status.ActiveLoan).Count(),
                WarrantyExpiredEquipmentCount = _EquipmentList.Where(x => x.Status == Status.WrittenOff).Count()
            });
        }

        [Authorize(Policy = "ViewEquipmentPolicy")]
        [HttpPost]
        public async Task<IActionResult> FilterIndexAsync(FilterEquipmentViewModel filter)
        {
            if (filter == null)
            {
                return RedirectToAction("Index");
            }

            IList<EquipmentResponse> _Responses = await __EquipmentManager.GetAsync();

            IndexViewModel _Model = new IndexViewModel()
            {
                Filter = filter,
                AvailableEquipmentCount = _Responses.Where(x => x.Status == Status.Available).Count(),
                OnLoanEquipmentCount = _Responses.Where(x => x.Status == Status.PendingLoan || x.Status == Status.ActiveLoan).Count(),
                WarrantyExpiredEquipmentCount = _Responses.Where(x => x.Status == Status.WrittenOff).Count()
            };

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                _Responses = _Responses.Where(x => x.Name.ToUpper().Contains(filter.Name.ToUpper())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filter.Description))
            {
                _Responses = _Responses.Where(x => x.Name.ToUpper().Contains(filter.Description.ToUpper())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filter.SerialNumber))
            {
                _Responses = _Responses.Where(x => x.Name.ToUpper().Contains(filter.SerialNumber.ToUpper())).ToList();
            }

            if (filter.PurchasePriceFrom >= 0)
            {
                _Responses = _Responses.Where(x => x.PurchasePrice >= filter.PurchasePriceFrom).ToList();
            }

            if (filter.PurchasePriceFrom <= double.MaxValue)
            {
                _Responses = _Responses.Where(x => x.PurchasePrice <= filter.PurchasePriceTo).ToList();
            }

            if (filter.PurchaseDateFrom != DateTime.MinValue)
            {
                _Responses = _Responses.Where(x => x.PurchaseDate >= filter.PurchaseDateFrom).ToList();
            }

            if (filter.PurchaseDateTo != DateTime.MinValue)
            {
                _Responses = _Responses.Where(x => x.PurchaseDate <= filter.PurchaseDateTo).ToList();
            }

            if (filter.WarrantyExpirationDateFrom != DateTime.MinValue)
            {
                _Responses = _Responses.Where(x => x.WarrantyExpirationDate >= filter.WarrantyExpirationDateFrom).ToList();
            }

            if (filter.WarrantyExpirationDateTo != DateTime.MinValue)
            {
                _Responses = _Responses.Where(x => x.WarrantyExpirationDate <= filter.WarrantyExpirationDateTo).ToList();
            }

            if (filter.Statuses != null && filter.Statuses?.Count > 0)
            {
                _Responses = _Responses.Where(x => filter.Statuses.Contains(x.Status)).ToList();
            }

            foreach (EquipmentResponse equipment in _Responses)
            {
                equipment.Blobs = (await __EquipmentBlobManager.GetAsync(equipment.UID)).Select(x => x.Blob).ToList();
            }

            _Model.Equipment = __Mapper.Map<IList<EquipmentViewModel>>(_Responses);

            return View("Index", _Model);
        }

        [Authorize(Policy = "ViewEquipmentPolicy")]
        [HttpGet]
        public async Task<IActionResult> AvailableEquipmentViewAsync()
        {
            return await FilterIndexAsync(new FilterEquipmentViewModel
            {
                Statuses = new List<Status>() { Status.Available }
            });
        }

        [Authorize(Policy = "ViewEquipmentPolicy")]
        [HttpGet]
        public async Task<IActionResult> OnLoanEquipmentViewAsync()
        {
            return await FilterIndexAsync(new FilterEquipmentViewModel
            {
                Statuses = new List<Status>() { Status.ActiveLoan }
            });
        }

        [Authorize(Policy = "ViewEquipmentPolicy")]
        [HttpGet]
        public async Task<IActionResult> ExpiredEquipmentViewAsync()
        {
            return await FilterIndexAsync(new FilterEquipmentViewModel
            {
                Statuses = new List<Status>() { Status.WrittenOff }
            });
        }

        [Authorize(Policy = "CreateEquipmentPolicy")]
        public async Task<IActionResult> CreateModalAsync()
        {
            CreateEquipmentViewModel _Model = new CreateEquipmentViewModel();
            return PartialView("_CreateEquipment", _Model);
        }

        [Authorize(Policy = "CreateEquipmentPolicy")]
        [HttpGet]
        public async Task<IActionResult> CreateViewAsync()
        {
            return View("Create", new CreateEquipmentViewModel());
        }

        [Authorize(Policy = "CreateEquipmentPolicy")]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateEquipmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
                return View("Create", model);
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

                    if (model.MediaFiles != null && model.MediaFiles.Count > 0)
                    {
                        foreach (IFormFile file in model.MediaFiles)
                        {
                            await UploadEquipmentMediaAsync(file, _EquipmentResponse.UID);
                        }
                    }
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

                if (model.MediaFiles != null && model.MediaFiles.Count > 0)
                {
                    foreach (IFormFile file in model.MediaFiles)
                    {
                        foreach (EquipmentResponse createdEquipment in _EquipmentResponses)
                        {
                            await UploadEquipmentMediaAsync(file, createdEquipment.UID);
                        }
                    }
                }
            }

            return RedirectToAction("Index", "Equipment", new { Area = "Equipment", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}." });
        }

        private async Task UploadEquipmentMediaAsync(IFormFile file, Guid equipmentUID)
        {
            string blobName = $"{Guid.NewGuid()}_{file.FileName}";
            string uri = await __BlobService.UploadFormFile(file, blobName);

            if (!string.IsNullOrWhiteSpace(uri))
            {
                BlobResponse _BlobResponse = await __BlobManager.CreateAsync(new CreateBlobRequest
                {
                    Name = blobName,
                    Path = uri
                });

                await __EquipmentBlobManager.CreateAsync(new CreateEquipmentBlobRequest
                {
                    EquipmentUID = equipmentUID,
                    BlobUID = _BlobResponse.UID
                });
            }
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
        public async Task<IActionResult> DetailsViewAsync(Guid uid, string successMessage = "", string errorMessage = "")
        {
            if (uid == null || uid == Guid.Empty)
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

            IList<LoanEquipmentResponse> _LoanEquipments = await __LoanEquipmentManager.GetByEquipmentAsync(uid);

            DetailsViewModel _Model = new DetailsViewModel
            {
                Equipment = __Mapper.Map<EquipmentViewModel>(await __EquipmentManager.GetAsync(uid)),
                Notes = __Mapper.Map<IList<NoteViewModel>>(await __NoteManager.GetAsync(uid)),
                EquipmentMedia = __Mapper.Map<IList<EquipmentMediaViewModel>>(await __EquipmentBlobManager.GetAsync(uid)),
                UploadMedia = new CreateEquipmentMediaViewModel
                {
                    EquipmentUID = uid
                },
                Loans = __Mapper.Map<IList<Loan.Models.LoanViewModel>>((await __LoanManager.GetAsync()).Where(x => _LoanEquipments.Select(y => y.LoanUID).Contains(x.UID)))
            };

            return View("Details", _Model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DetailsModalAsync(Guid uid)
        {
            if (uid == null || uid == Guid.Empty)
            {
                ViewData["ErrorMessage"] = "Invalid equipment UID";
            }

            DetailsViewModel _Model = new DetailsViewModel
            {
                Equipment = __Mapper.Map<EquipmentViewModel>(await __EquipmentManager.GetAsync(uid)),
                Notes = __Mapper.Map<IList<NoteViewModel>>(await __NoteManager.GetAsync(uid))
            };

            return PartialView("_DetailsModal", _Model);
        }

        [Authorize(Policy = "DeleteEquipmentPolicy")]
        [HttpGet]
        public async Task<IActionResult> DeleteModalAsync(Guid uid)
        {
            EquipmentResponse _Response = await __EquipmentManager.GetAsync(uid);

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

        [Authorize(Policy = "ViewEquipmentPolicy")]
        [HttpGet]
        public async Task<IActionResult> ExpiredViewAsync()
        {
            IList<EquipmentResponse> _ExpiredEquipment = (await __EquipmentManager.GetAsync())?.Where(x => x.WarrantyExpirationDate <= DateTime.Now && x.Status != Status.WrittenOff).ToList();
            IList<EquipmentResponse> _WrittenOffEquipment = await __EquipmentManager.GetByStatusAsync(Status.WrittenOff);

            ExpiredViewModel _Model = new ExpiredViewModel
            {
                ExpiredEquipment = __Mapper.Map<IList<EquipmentViewModel>>(_ExpiredEquipment),
                WrittenOffEquipment = __Mapper.Map<IList<EquipmentViewModel>>(_WrittenOffEquipment)
            };

            return View("Expired", _Model);
        }
    }
}