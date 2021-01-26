using ELMS.WEB.Adapters.Equipment;
using ELMS.WEB.Adapters.Loan;
using ELMS.WEB.Areas.Loan.Models;
using ELMS.WEB.Enums.Equipment;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Base.Response;
using ELMS.WEB.Models.Loan.Response;
using ELMS.WEB.Repositories.Identity.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public LoanController(ILoanManager loanManager, IEquipmentManager equipmentManager, IUserRepository userRepository)
        {
            __LoanManager = loanManager ?? throw new ArgumentNullException(nameof(loanManager));
            __EquipmentManager = equipmentManager ?? throw new ArgumentNullException(nameof(equipmentManager));
            __UserRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public IActionResult Index(string errorMessage = "", string successMessage = "")
        {
            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            if (!String.IsNullOrWhiteSpace(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }

            return View();
        }

        [HttpGet]
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

            return RedirectToAction("Index", "Loan", new { Area = "Loan" });
        }
    }
}