using AutoMapper;
using ELMS.WEB.Areas.Loan.Models;
using ELMS.WEB.Areas.Loan.Models.LoanExtension;
using ELMS.WEB.Enums.Equipment;
using ELMS.WEB.Enums.General;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Admin.Interfaces;
using ELMS.WEB.Managers.Email.Interface;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Email.Response;
using ELMS.WEB.Models.Loan.Request;
using ELMS.WEB.Models.Loan.Response;
using ELMS.WEB.Repositories.Identity.Interface;
using ELMS.WEB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Loan.Controllers
{
    [Authorize]
    [Area("Loan")]
    public class LoanController : Controller
    {
        private readonly IMapper __Mapper;
        private readonly ILoanManager __LoanManager;
        private readonly IEquipmentManager __EquipmentManager;
        private readonly IUserRepository __UserRepository;
        private readonly ILoanEquipmentManager __LoanEquipmentManager;
        private readonly IEmailScheduleManager __EmailScheduleManager;
        private readonly IApplicationEmailSender __EmailSender;
        private readonly IBlacklistManager __BlacklistManager;
        private readonly UserManager<IdentityUser> __UserManager;
        private readonly ILoanExtensionManager __LoanExtensionManager;
        private readonly string ENTITY_NAME = "Loan";

        public LoanController(IMapper mapper, ILoanManager loanManager, IEquipmentManager equipmentManager, IUserRepository userRepository,
            ILoanEquipmentManager loanEquipmentManager, IEmailScheduleManager emailScheduleManager,
            IApplicationEmailSender emailSender, IBlacklistManager blacklistManager,
            UserManager<IdentityUser> userManager, ILoanExtensionManager loanExtensionManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __LoanManager = loanManager ?? throw new ArgumentNullException(nameof(loanManager));
            __EquipmentManager = equipmentManager ?? throw new ArgumentNullException(nameof(equipmentManager));
            __UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            __LoanEquipmentManager = loanEquipmentManager ?? throw new ArgumentNullException(nameof(loanEquipmentManager));
            __EmailScheduleManager = emailScheduleManager ?? throw new ArgumentNullException(nameof(emailScheduleManager));
            __EmailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            __BlacklistManager = blacklistManager ?? throw new ArgumentNullException(nameof(blacklistManager));
            __UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            __LoanExtensionManager = loanExtensionManager ?? throw new ArgumentNullException(nameof(loanExtensionManager));
        }

        [Authorize(Policy = "ViewLoanPolicy")]
        public async Task<IActionResult> Index(string errorMessage = "", string successMessage = "")
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
                Filter = new LoanFilterViewModel(),
                Loans = __Mapper.Map<IList<LoanViewModel>>(await __LoanManager.GetAsync())
            };

            foreach (LoanViewModel loan in _Model.Loans)
            {
                loan.Extensions = __Mapper.Map<IList<LoanExtensionViewModel>>(await __LoanExtensionManager.GetAsync(loan.UID));
                loan.ExpiryTimestamp = await __LoanManager.GetExpiryDate(loan.UID);
            }

            return View("Index", _Model);
        }

        [Authorize(Policy = "ViewLoanPolicy")]
        [HttpPost]
        public async Task<IActionResult> FilterIndexAsync(LoanFilterViewModel filter)
        {
            IList<LoanViewModel> _FilteredLoans = await FilterAsync(await __LoanManager.GetAsync(), filter);

            IndexViewModel _Model = new IndexViewModel
            {
                Filter = filter,
                Loans = _FilteredLoans
            };

            return View("Index", _Model);
        }

        private async Task<IList<LoanViewModel>> FilterAsync(IList<LoanResponse> loans, LoanFilterViewModel filter)
        {
            if (loans.Count <= 0)
            {
                return new List<LoanViewModel>();
            }

            if (!string.IsNullOrWhiteSpace(filter.LoaneeEmail))
            {
                loans = loans.Where(x => x.LoaneeEmail.ToUpper().Contains(filter.LoaneeEmail.ToUpper())).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filter.LoanerEmail))
            {
                loans = loans.Where(x => x.LoanerEmail.ToUpper().Contains(filter.LoanerEmail.ToUpper())).ToList();
            }

            if (filter.FromTimestamp != null)
            {
                loans = loans.Where(x => x.FromTimestamp >= filter.FromTimestamp).ToList();
            }

            if (filter.ExpiryTimestamp != null)
            {
                loans = loans.Where(x => x.FromTimestamp <= filter.ExpiryTimestamp).ToList();
            }

            if (filter.CreatedFromTimestamp != null)
            {
                loans = loans.Where(x => x.CreatedTimestamp >= filter.CreatedFromTimestamp).ToList();
            }

            if (filter.CreatedToTimestamp != null)
            {
                loans = loans.Where(x => x.CreatedTimestamp <= filter.CreatedToTimestamp).ToList();
            }

            if (filter.AcceptedTermsAndConditions != BooleanFilter.All)
            {
                loans = (filter.AcceptedTermsAndConditions == BooleanFilter.True) ?
                    loans.Where(x => x.AcceptedTermsAndConditions).ToList() :
                    loans.Where(x => !x.AcceptedTermsAndConditions).ToList();
            }

            if (filter.FromTimestamp != null)
            {
                loans = loans.Where(x => x.FromTimestamp >= filter.FromTimestamp).ToList();
            }

            if (filter.ExpiryTimestamp != null)
            {
                loans = loans.Where(x => x.ExpiryTimestamp <= filter.ExpiryTimestamp).ToList();
            }

            if (filter.Statuses != null && filter.Statuses?.Count > 0)
            {
                loans = loans.Where(x => filter.Statuses.Contains(x.Status)).ToList();
            }

            return __Mapper.Map<IList<LoanViewModel>>(loans);
        }

        [HttpGet]
        [Authorize(Policy = "CreateLoanPolicy")]
        public async Task<IActionResult> CreateViewAsync()
        {
            IList<LoanResponse> _Loans = (await __LoanManager.GetAsync()).Where(loan => loan.Status != Enums.Loan.Status.Complete && loan.Status != Enums.Loan.Status.ManualComplete).ToList();

            List<Guid> _ExcludeEquipment = new List<Guid>();
            foreach (LoanResponse loan in _Loans)
            {
                IList<LoanEquipmentResponse> _Response = await __LoanEquipmentManager.GetAsync(loan.UID);
                _ExcludeEquipment.AddRange(_Response.Select(x => x.EquipmentUID));
            }
            _ExcludeEquipment = _ExcludeEquipment.Distinct().ToList();

            CreateLoanViewModel _Model = new CreateLoanViewModel
            {
                EquipmentSelectList = __Mapper.Map<IList<Equipment.Models.EquipmentViewModel>>((await __EquipmentManager.GetAsync()).Equipments.Where(x => x.Status == Status.Available && !_ExcludeEquipment.Contains(x.UID))),
                UserSelectList = await __UserRepository.GetAsync(),
                Blacklists = __Mapper.Map<IList<Admin.Models.Blacklist.BlacklistViewModel>>(await __BlacklistManager.GetAsync())
            };

            return View("CreateLoan", _Model);
        }

        [HttpPost]
        [Authorize(Policy = "CreateLoanPolicy")]
        public async Task<IActionResult> ConfirmViewAsync(CreateLoanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                IList<LoanResponse> _Loans = (await __LoanManager.GetAsync()).Where(loan => loan.Status != Enums.Loan.Status.Complete && loan.Status != Enums.Loan.Status.ManualComplete).ToList();
                List<Guid> _ExcludeEquipment = new List<Guid>();
                foreach (LoanResponse loan in _Loans)
                {
                    IList<LoanEquipmentResponse> _Response = await __LoanEquipmentManager.GetAsync(loan.UID);
                    _ExcludeEquipment.AddRange(_Response.Select(x => x.EquipmentUID));
                }
                _ExcludeEquipment = _ExcludeEquipment.Distinct().ToList();

                model.EquipmentSelectList = __Mapper.Map<IList<Equipment.Models.EquipmentViewModel>>((await __EquipmentManager.GetAsync()).Equipments.Where(x => x.Status == Status.Available && !_ExcludeEquipment.Contains(x.UID)));
                model.UserSelectList = await __UserRepository.GetAsync();
                model.Blacklists = __Mapper.Map<IList<Admin.Models.Blacklist.BlacklistViewModel>>(await __BlacklistManager.GetAsync());

                return View("CreateLoan", model);
            }

            ConfirmationLoanViewModel _Model = new ConfirmationLoanViewModel
            {
                ExpiryTimestamp = model.ExpiryTimestamp,
                FromTimestamp = model.FromTimestamp,
                LoaneeEmail = model.LoaneeEmailAddress,
                SelectedEquipmentList = __Mapper.Map<IList<Equipment.Models.EquipmentViewModel>>((await __EquipmentManager.GetAsync(model.SelectedEquipment)).Equipments),
                LoanerEmail = __UserManager.GetUserAsync(HttpContext.User).Result.Email,
                BlacklistState = await __BlacklistManager.GetState(model.LoaneeEmailAddress)
            };

            return View("ConfirmationLoan", _Model);
        }

        [HttpPost]
        [Authorize(Policy = "CreateLoanPolicy")]
        public async Task<IActionResult> CreateAsync(ConfirmationLoanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                IList<LoanResponse> _Loans = (await __LoanManager.GetAsync()).Where(loan => loan.Status != Enums.Loan.Status.Complete && loan.Status != Enums.Loan.Status.ManualComplete).ToList();
                List<Guid> _ExcludeEquipment = new List<Guid>();
                foreach (LoanResponse loan in _Loans)
                {
                    IList<LoanEquipmentResponse> _LoanEquipment = await __LoanEquipmentManager.GetAsync(loan.UID);
                    _ExcludeEquipment.AddRange(_LoanEquipment.Select(x => x.EquipmentUID));
                }
                _ExcludeEquipment = _ExcludeEquipment.Distinct().ToList();

                CreateLoanViewModel _CreateLoanViewModel = new CreateLoanViewModel
                {
                    EquipmentSelectList = __Mapper.Map<IList<Equipment.Models.EquipmentViewModel>>((await __EquipmentManager.GetAsync()).Equipments.Where(x => x.Status == Status.Available && !_ExcludeEquipment.Contains(x.UID))),
                    UserSelectList = await __UserRepository.GetAsync(),
                    Blacklists = __Mapper.Map<IList<Admin.Models.Blacklist.BlacklistViewModel>>(await __BlacklistManager.GetAsync())
                };

                ModelState.AddModelError("Error", "Unable to create Loan");
                return View("CreateLoan", _CreateLoanViewModel);
            }

            model.LoanerEmail = __UserManager.GetUserAsync(HttpContext.User).Result.Email;
            LoanResponse _Response = await __LoanManager.CreateAsync(__Mapper.Map<CreateLoanRequest>(model));

            if (!_Response.Success)
            {
                ModelState.AddModelError("Error", _Response.Message);
                return View("CreateLoan", model);
            }

            await __EmailScheduleManager.CreateLoanConfirmScheduleAsync(_Response, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}", true);

            return RedirectToAction("Index", "Loan", new { Area = "Loan", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {ENTITY_NAME}." });
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

            LoanViewModel _LoanViewModel = __Mapper.Map<LoanViewModel>(_Response);
            IList<Guid> _EquipmentUIDs = (await __LoanEquipmentManager.GetAsync(_Response.UID)).Select(x => x.EquipmentUID).ToList();
            if (_EquipmentUIDs != null && _EquipmentUIDs.Count > 0)
            {
                _LoanViewModel.EquipmentList = __Mapper.Map<IList<Equipment.Models.EquipmentViewModel>>((await __EquipmentManager.GetAsync(_EquipmentUIDs)).Equipments);
            }

            if (_Response.AcceptedTermsAndConditions)
            {
                AlreadyAcceptedTermsAndConditionsViewModel _AlreadyAcceptedModel = new AlreadyAcceptedTermsAndConditionsViewModel
                {
                    UID = loanUID,
                    Accepted = _Response.AcceptedTermsAndConditions,
                    Loan = _LoanViewModel
                };

                return View("AlreadyAcceptedTermsAndConditions", _AlreadyAcceptedModel);
            }

            AcceptTermsAndConditionsViewModel _Model = new AcceptTermsAndConditionsViewModel
            {
                UID = loanUID,
                Accepted = _Response.AcceptedTermsAndConditions,
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
            await __EmailScheduleManager.CreateLoanConfirmedScheduleAsync(_Loan, $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}", true);

            return View("AcceptedTermsAndConditions");
        }

        [Authorize(Policy = "ViewLoanPolicy")]
        public async Task<IActionResult> DetailsViewAsync(Guid uid, string successMessage = "", string errorMessage = "")
        {
            if (!string.IsNullOrEmpty(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }
            else if (!string.IsNullOrEmpty(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            return View("Details", new DetailsViewModel {
                Loan = __Mapper.Map<LoanViewModel>(await __LoanManager.GetByUIDAsync(uid)),
                Extensions = __Mapper.Map<IList<LoanExtensionViewModel>>(await __LoanExtensionManager.GetAsync(uid))
            });
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

            LoanViewModel _Model = __Mapper.Map<LoanViewModel>(_Response);

            IList<Guid> _EquipmentUIDs = (await __LoanEquipmentManager.GetAsync(_Response.UID)).Select(x => x.EquipmentUID).ToList();
            if (_EquipmentUIDs != null && _EquipmentUIDs.Count > 0)
            {
                _Model.EquipmentList = __Mapper.Map<IList<Equipment.Models.EquipmentViewModel>>((await __EquipmentManager.GetAsync(_EquipmentUIDs)).Equipments);
            }

            return View("LoanPreview", _Model);
        }

        [Authorize(Policy = "EditLoanPolicy")]
        [HttpPost]
        public async Task<IActionResult> EditAsync(UpdateLoanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("DetailsView", new { Area = "Loan", uid = model.UID, errorMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} update {ENTITY_NAME} details." });
            }

            LoanResponse _Loan = await __LoanManager.GetByUIDAsync(model.UID);

            if (_Loan == null)
            {
                return RedirectToAction("DetailsView", new { Area = "Loan", uid = model.UID, errorMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME} details." });
            }

            UpdateLoanRequest _Request = new UpdateLoanRequest
            {
                UID = model.UID,
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
                return RedirectToAction("DetailsView", new { Area = "Loan", uid = model.UID, errorMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} update {ENTITY_NAME} details." });
            }

            return RedirectToAction("DetailsView", new { Area = "Loan", uid = model.UID, successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} updated {ENTITY_NAME} details." });
        }

        [HttpGet]
        public async Task<IActionResult> ForceCompleteModalAsync(Guid uid)
        {
            if (uid == Guid.Empty)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            LoanResponse _Loan = await __LoanManager.GetByUIDAsync(uid);

            if (_Loan == null)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            ForceCompleteLoanViewModel _Model = new ForceCompleteLoanViewModel
            {
                UID = uid,
                LoaneeEmail = _Loan.LoaneeEmail,
                StartTimestamp = _Loan.FromTimestamp,
                ExpiryTimestamp = _Loan.ExpiryTimestamp
            };

            return PartialView("_ForceCompleteModal", _Model);
        }

        [HttpPost]
        public async Task<IActionResult> ForceCompleteAsync(ForceCompleteLoanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            LoanResponse _Loan = await __LoanManager.GetByUIDAsync(model.UID);
            if (_Loan == null)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }
            await __LoanManager.ChangeStatusAsync(_Loan.UID, Enums.Loan.Status.ManualComplete);

            EmailScheduleResponse _Schedule = (await __EmailScheduleManager.GetAsync()).FirstOrDefault(s => s.Sent == false && s.RecipientEmailAddress == _Loan.LoaneeEmail && s.SendTimestamp == _Loan.ExpiryTimestamp);
            if (_Schedule != null)
            {
                await __EmailScheduleManager.UpdateSentAsync(_Schedule.UID, true);
            }

            return RedirectToAction("Index", "Loan", new { Area = "Loan", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} forced completed {ENTITY_NAME}" });
        }
    }
}