using AutoMapper;
using ELMS.WEB.Areas.Loan.Models.LoanExtension;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Loan.Request;
using ELMS.WEB.Models.Loan.Response;
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
    public class LoanExtensionController : Controller
    {
        private readonly IMapper __Mapper;
        private readonly ILoanExtensionManager __LoanExtensionManager;
        private readonly ILoanManager __LoanManager;
        private readonly UserManager<IdentityUser> __UserManager;
        private const string MODEL_NAME = "Loan extension";

        public LoanExtensionController(IMapper mapper, ILoanExtensionManager loanExtensionManager, ILoanManager loanManager, UserManager<IdentityUser> userManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __LoanExtensionManager = loanExtensionManager ?? throw new ArgumentNullException(nameof(loanExtensionManager));
            __LoanManager = loanManager ?? throw new ArgumentNullException(nameof(loanManager));
            __UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        [Authorize(Policy = "CreateLoanExtensionPolicy")]
        public async Task<IActionResult> CreateModalAsync(Guid loanUID)
        {
            if (loanUID == Guid.Empty)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} find Loan." });
            }

            LoanResponse _Loan = await __LoanManager.GetByUIDAsync(loanUID);

            if (_Loan == null)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} find Loan." });
            }

            CreateLoanExtensionViewModel _Model = new CreateLoanExtensionViewModel
            {
                LoanUID = loanUID,
                PreviousExpiryDate = _Loan.ExpiryTimestamp,
                ExtenderEmail = __UserManager.GetUserAsync(HttpContext.User).Result.Email
            };

            IList<LoanExtensionResponse> _Extensions = (await __LoanExtensionManager.GetAsync(loanUID))?.OrderByDescending(x => x.NewExpiryDate)?.ToList() ?? null;

            if (_Extensions?.Count > 0)
            {
                _Model.PreviousExpiryDate = _Extensions[0].NewExpiryDate;
            }

            _Model.NewExpiryDate = _Model.PreviousExpiryDate < DateTime.Now ? DateTime.Today.AddDays(7) : _Model.PreviousExpiryDate.AddDays(7);

            return PartialView("_CreateModal", _Model);
        }

        [HttpPost]
        [Authorize(Policy = "CreateLoanExtensionPolicy")]
        public async Task<IActionResult> CreateAsync(CreateLoanExtensionViewModel model)
        {
            if (model == null)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} find Loan." });
            }
            else if (model?.NewExpiryDate <= model.PreviousExpiryDate)
            {
                ModelState.AddModelError("Error", "New Expiry Date cannot be less than or equal to the Previous Expiry Date.");
            }

            if (!ModelState.IsValid)
            {
                return PartialView("_CreateModal", model);
            }

            LoanResponse _Loan = await __LoanManager.GetByUIDAsync(model.LoanUID);

            if (_Loan.Status == Enums.Loan.Status.EarlyComplete || _Loan.Status == Enums.Loan.Status.Complete)
            {
                return Json(new { message = $"{GlobalConstants.ERROR_ACTION_PREFIX} extend Loan because the loan is completed. Please create a new loan instead." });
            }

            LoanExtensionResponse _Response = await __LoanExtensionManager.CreateAsync(__Mapper.Map<CreateLoanExtensionRequest>(model));

            if (_Response?.Success == false)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} create {MODEL_NAME}." });
            }

            return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} created {MODEL_NAME}." });
        }
    }
}