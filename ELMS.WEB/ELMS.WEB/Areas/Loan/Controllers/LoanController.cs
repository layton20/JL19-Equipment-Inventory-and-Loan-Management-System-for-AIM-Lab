using ELMS.WEB.Adapters.Equipment;
using ELMS.WEB.Adapters.Loan;
using ELMS.WEB.Areas.Email.Data;
using ELMS.WEB.Areas.Loan.Models;
using ELMS.WEB.Enums.Equipment;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Loan.Request;
using ELMS.WEB.Models.Loan.Response;
using ELMS.WEB.Repositories.Identity.Interface;
using ELMS.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Loan.Controllers
{
    [Authorize]
    [Area("Loan")]
    public class LoanController : Controller
    {
        private readonly ILoanManager __LoanManager;
        private readonly IEquipmentManager __EquipmentManager;
        private readonly IUserRepository __UserRepository;
        private readonly ILoanEquipmentManager __LoanEquipmentManager;
        private readonly IApplicationEmailSender __EmailSender;
        private readonly string MODEL_NAME = "Loan";

        public LoanController(ILoanManager loanManager, IEquipmentManager equipmentManager, IUserRepository userRepository, ILoanEquipmentManager loanEquipmentManager, IApplicationEmailSender emailSender)
        {
            __LoanManager = loanManager ?? throw new ArgumentNullException(nameof(loanManager));
            __EquipmentManager = equipmentManager ?? throw new ArgumentNullException(nameof(equipmentManager));
            __UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            __LoanEquipmentManager = loanEquipmentManager ?? throw new ArgumentNullException(nameof(loanEquipmentManager));
            __EmailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        [Authorize(Policy = "ViewLoanPolicy")]
        public async Task<IActionResult> Index(string errorMessage = "", string successMessage = "")
        {
            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            if (!String.IsNullOrWhiteSpace(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }

            IndexViewModel _Model = new IndexViewModel();

            foreach (LoanResponse loan in await __LoanManager.GetAsync())
            {
                LoanViewModel _LoanViewModel = loan.ToViewModel();

                IList<Guid> _EquipmentUIDs = (await __LoanEquipmentManager.GetAsync(loan.UID)).Select(x => x.EquipmentUID).ToList();
                if (_EquipmentUIDs != null && _EquipmentUIDs.Count > 0)
                {
                    _LoanViewModel.EquipmentList = (await __EquipmentManager.GetAsync(_EquipmentUIDs)).Equipments.ToViewModel();
                }

                if (loan.LoaneeUID != Guid.Empty)
                {
                    _LoanViewModel.Loanee = await __UserRepository.GetByUIDAsync(loan.LoaneeUID);
                }

                if (loan.LoanerUID != Guid.Empty)
                {
                    _LoanViewModel.Loaner = await __UserRepository.GetByUIDAsync(loan.LoanerUID);
                }

                _Model.Loans.Add(_LoanViewModel);
            }

            return View("Index", _Model);
        }

        [HttpGet]
        [Authorize(Policy = "CreateLoanPolicy")]
        public async Task<IActionResult> CreateViewAsync()
        {
            CreateLoanViewModel _Model = new CreateLoanViewModel
            {
                EquipmentSelectList = (await __EquipmentManager.GetAsync()).Equipments.Where(x => x.Status == Status.Available).ToList().ToViewModel(),
                UserSelectList = await __UserRepository.GetAsync()
            };

            return View("CreateLoan", _Model);
        }

        [HttpPost]
        [Authorize(Policy = "CreateLoanPolicy")]
        public async Task<IActionResult> CreateAsync(CreateLoanViewModel model)
        {
            model.LoanerUID = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (model.FromTimestamp >= model.ExpiryTimestamp)
            {
                ModelState.AddModelError("Error", "From Date must be less than the Expiry Date");
            }

            if (!ModelState.IsValid)
            {
                model.EquipmentSelectList = (await __EquipmentManager.GetAsync()).Equipments.Where(x => x.Status == Status.Available).ToList().ToViewModel();
                model.UserSelectList = await __UserRepository.GetAsync();

                ViewData["ErrorMessage"] = "Invalid form submission";
                return View("CreateLoan", model);
            }

            model.LoanerUID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            LoanResponse _Response = await __LoanManager.CreateAsync(model.ToRequest());

            if (!_Response.Success)
            {
                ModelState.AddModelError("Error", _Response.Message);
                return View("CreateLoan", model);
            }

            await __EmailSender.SendLoanConfirmEmail(_Response.LoaneeEmail, "AIM LAB - Activate Loan", new ConfirmEmailTemplate
            {
                Confirm_Loan_URL = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/Loan/Loan/AcceptTermsAndConditionsView?loanUID={_Response.UID}"
            });

            return RedirectToAction("Index", "Loan", new { Area = "Loan", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {MODEL_NAME}." });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> AcceptTermsAndConditionsViewAsync(Guid loanUID)
        {
            if (loanUID == null || loanUID == Guid.Empty)
            {
                return Json("Page not found");
            }

            LoanResponse _Response = await __LoanManager.GetByUIDAsync(loanUID);

            if (_Response == null)
            {
                return Json("Page not found");
            }

            LoanViewModel _LoanViewModel = _Response.ToViewModel();

            IList<Guid> _EquipmentUIDs = (await __LoanEquipmentManager.GetAsync(_Response.UID)).Select(x => x.EquipmentUID).ToList();
            if (_EquipmentUIDs != null && _EquipmentUIDs.Count > 0)
            {
                _LoanViewModel.EquipmentList = (await __EquipmentManager.GetAsync(_EquipmentUIDs)).Equipments.ToViewModel();
            }

            if (_Response.LoaneeUID != Guid.Empty)
            {
                _LoanViewModel.Loanee = await __UserRepository.GetByUIDAsync(_Response.LoaneeUID);
            }

            if (_Response.LoanerUID != Guid.Empty)
            {
                _LoanViewModel.Loaner = await __UserRepository.GetByUIDAsync(_Response.LoanerUID);
            }

            AcceptTermsAndConditionsViewModel _Model = new AcceptTermsAndConditionsViewModel
            {
                UID = loanUID,
                Accepted = false,
                Loan = _LoanViewModel
            };

            return View("AcceptTermsAndConditions", _Model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AcceptTermsAndConditionsAsync(AcceptTermsAndConditionsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
                return View("AcceptTermsAndConditions", model);
            }

            BaseResponse _Response = await __LoanManager.AcceptTermsAndConditions(model.UID);

            if (!_Response.Success)
            {
                ModelState.AddModelError("Error", _Response.Message);
                return await AcceptTermsAndConditionsViewAsync(model.UID);
            }

            LoanResponse _Loan = await __LoanManager.GetByUIDAsync(model.UID);
            await __EmailSender.SendLoanConfirmedEmail(_Loan.LoaneeEmail, "AIM - Loan Confirmed", new ConfirmedEmailTemplate
            {
                Loan_Details_URL = "",
                Start_Timestamp = _Loan.FromTimestamp.ToString()
            });

            return View("AcceptedTermsAndConditions");
        }

        [Authorize(Policy = "ViewLoanPolicy")]
        public async Task<IActionResult> DetailsViewAsync(Guid uid, string successMessage = "", string errorMessage = "")
        {
            if (!String.IsNullOrEmpty(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }

            if (!String.IsNullOrEmpty(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            LoanViewModel _Model = (await __LoanManager.GetByUIDAsync(uid)).ToViewModel();
            IList<Guid> _EquipmentUIDs = (await __LoanEquipmentManager.GetAsync(_Model.UID)).Select(x => x.EquipmentUID).ToList();
            if (_EquipmentUIDs != null && _EquipmentUIDs.Count > 0)
            {
                _Model.EquipmentList = (await __EquipmentManager.GetAsync(_EquipmentUIDs)).Equipments.ToViewModel();
            }

            return View("Details", _Model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LoanPreviewAsync(Guid uid)
        {
            if (uid == null || uid == Guid.Empty)
            {
                return Json("Page not found");
            }

            LoanResponse _Response = await __LoanManager.GetByUIDAsync(uid);

            if (_Response == null)
            {
                return Json("Page not found");
            }

            LoanViewModel _Model = _Response.ToViewModel();

            IList<Guid> _EquipmentUIDs = (await __LoanEquipmentManager.GetAsync(_Response.UID)).Select(x => x.EquipmentUID).ToList();
            if (_EquipmentUIDs != null && _EquipmentUIDs.Count > 0)
            {
                _Model.EquipmentList = (await __EquipmentManager.GetAsync(_EquipmentUIDs)).Equipments.ToViewModel();
            }

            if (_Response.LoaneeUID != Guid.Empty)
            {
                _Model.Loanee = await __UserRepository.GetByUIDAsync(_Response.LoaneeUID);
            }

            if (_Response.LoanerUID != Guid.Empty)
            {
                _Model.Loaner = await __UserRepository.GetByUIDAsync(_Response.LoanerUID);
            }

            return View("LoanPreview", _Model);
        }

        [Authorize(Policy = "EditLoanPolicy")]
        [HttpPost]
        public async Task<IActionResult> EditAsync(UpdateLoanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("DetailsView", new { Area = "Loan", uid = model.UID, errorMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} update {MODEL_NAME} details." });
            }

            LoanResponse _Loan = await __LoanManager.GetByUIDAsync(model.UID);

            if (_Loan == null)
            {
                return RedirectToAction("DetailsView", new { Area = "Loan", uid = model.UID, errorMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {MODEL_NAME} details." });
            }

            UpdateLoanRequest _Request = new UpdateLoanRequest {
                UID = model.UID,
                Name = model.Name,
                FromTimestamp = model.StartTimestamp,
                ExpiryTimestamp = model.ExpiryTimestamp,
                AcceptedTermsAndConditions = model.AcceptedTermsAndConditions
            };

            if (model.AcceptedTermsAndConditions)
            {
                if (_Loan.Status == Enums.Loan.Status.Pending)
                {
                    if (model.StartTimestamp <= DateTime.Now)
                    {
                        _Request.Status = Enums.Loan.Status.ActiveLoan;
                    }
                    else
                    {
                        _Request.Status = Enums.Loan.Status.InactiveLoan;
                    }
                }
            }

            BaseResponse _Response = await __LoanManager.UpdateAsync(_Request);

            if (!_Response.Success)
            {
                return RedirectToAction("DetailsView", new { Area = "Loan", uid = model.UID, errorMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} update {MODEL_NAME} details." });
            }

            return RedirectToAction("DetailsView", new { Area = "Loan", uid = model.UID, successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} updated {MODEL_NAME} details." });
        }
    }
}