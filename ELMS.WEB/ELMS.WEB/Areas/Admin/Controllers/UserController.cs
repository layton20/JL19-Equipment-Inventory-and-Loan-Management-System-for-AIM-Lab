using AutoMapper;
using ELMS.WEB.Areas.Admin.Data;
using ELMS.WEB.Areas.Admin.Models.User;
using ELMS.WEB.Entities.Admin;
using ELMS.WEB.Helpers;
using ELMS.WEB.Managers.Equipment.Interfaces;
using ELMS.WEB.Managers.Loan.Interface;
using ELMS.WEB.Models.Loan.Response;
using ELMS.WEB.Repositories.Identity.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NsModelPermission = ELMS.WEB.Areas.Admin.Models.Permission;
using NsModelUser = ELMS.WEB.Areas.Admin.Models.User;

namespace ELMS.WEB.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IMapper __Mapper;
        private readonly UserManager<IdentityUser> __UserManager;
        private readonly ILoanManager __LoanManager;
        private readonly IUserRepository __UserRepository;
        private readonly ILoanEquipmentManager __LoanEquipmentManager;
        private readonly IEquipmentManager __EquipmentManager;
        private readonly string ENTITY_NAME = "User";

        public UserController(IMapper mapper, UserManager<IdentityUser> userManager, ILoanManager loanManager, IUserRepository userRepository, ILoanEquipmentManager loanEquipmentManager, IEquipmentManager equipmentManager)
        {
            __Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            __UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            __LoanManager = loanManager ?? throw new ArgumentNullException(nameof(loanManager));
            __UserRepository = userRepository;
            __LoanEquipmentManager = loanEquipmentManager;
            __EquipmentManager = equipmentManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string successMessage = "", string errorMessage = "")
        {
            if (!String.IsNullOrWhiteSpace(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }

            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            NsModelUser.IndexViewModel _Model = new NsModelUser.IndexViewModel
            {
                Users = await __UserManager.Users.ToListAsync()
            };

            return View(_Model);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsModalAsync(Guid uid)
        {
            IdentityUser _User = await __UserManager.FindByIdAsync(uid.ToString());

            if (_User == null)
            {
                return Json(new { success = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}" });
            }

            DetailsViewModel _Model = new DetailsViewModel
            {
                User = _User,
                Loans = __Mapper.Map<IList<Loan.Models.LoanViewModel>>(await __LoanManager.GetByUserAsync(_User.Email)),
                Roles = await __UserManager.GetRolesAsync(_User)
            };

            return PartialView("_DetailsModal", _Model);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsViewAsync(Guid uid, string successMessage, string errorMessage)
        {
            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            IdentityUser _User = await __UserManager.FindByIdAsync(uid.ToString());

            if (_User == null)
            {
                return RedirectToAction("Index", "User", new { Area = "Admin", errorMessage = $"User does not exist" });
            }

            DetailsViewModel _Model = new DetailsViewModel
            {
                User = _User,
                Roles = await __UserManager.GetRolesAsync(_User),
            };

            foreach (LoanResponse loan in await __LoanManager.GetByUserAsync(_User.Email))
            {
                Loan.Models.LoanViewModel _LoanViewModel = __Mapper.Map<Loan.Models.LoanViewModel>(loan);

                IList<Guid> _EquipmentUIDs = (await __LoanEquipmentManager.GetAsync(loan.UID)).Select(x => x.EquipmentUID).ToList();
                if (_EquipmentUIDs != null && _EquipmentUIDs.Count > 0)
                {
                    _LoanViewModel.EquipmentList = __Mapper.Map<IList<Equipment.Models.EquipmentViewModel>>((await __EquipmentManager.GetAsync(_EquipmentUIDs)).Equipments);
                }

                _Model.Loans.Add(_LoanViewModel);
            }

            IList<Claim> _ExistingClains = await __UserManager.GetClaimsAsync(_User);

            _Model.UserClaims = new NsModelPermission.UserClaimsViewModel
            {
                UserID = uid.ToString()
            };

            foreach (Claim claim in ClaimsStore.AllClaims)
            {
                UserClaim _UserClaim = new UserClaim
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                };

                if (_ExistingClains.Any(c => c.Type == claim.Type && c.Value == "true"))
                {
                    _UserClaim.IsSelected = true;
                }

                _Model.UserClaims.Claims.Add(_UserClaim);
            }

            return View("Details", _Model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserPermissionsAsync(DetailsViewModel model)
        {
            IdentityUser _User = await __UserManager.FindByIdAsync(model.UserClaims.UserID);

            if (_User == null)
            {
                return RedirectToAction("DetailsView", "User", new { Area = "Admin", uid = model.UserClaims.UserID, errorMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} find User." });
            }

            IList<Claim> _Claims = await __UserManager.GetClaimsAsync(_User);
            IdentityResult _Result = await __UserManager.RemoveClaimsAsync(_User, _Claims);

            if (!_Result.Succeeded)
            {
                return RedirectToAction("DetailsView", "User", new { Area = "Admin", uid = model.UserClaims.UserID, errorMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} update User Permissions." });
            }

            _Result = await __UserManager.AddClaimsAsync(_User,
                model.UserClaims.Claims.Select(c => new Claim(c.ClaimType, c.IsSelected ? "true" : "false")));

            if (!_Result.Succeeded)
            {
                return RedirectToAction("DetailsView", "User", new { Area = "Admin", uid = model.UserClaims.UserID, errorMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} update User Permissions." });
            }

            return RedirectToAction("DetailsView", "User", new { Area = "Admin", uid = model.UserClaims.UserID, successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} updated User Permissions." });
        }
        
        [HttpGet]
        public async Task<IActionResult> DeleteModalAsync(Guid uid)
        {
            if (uid == null)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            IdentityUser _User = await __UserRepository.GetByUIDAsync(uid);

            if (_User == null)
            {
                return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} find {ENTITY_NAME}." });
            }

            return View("_DeleteModal", _User);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(IdentityUser model)
        {
            if (model != null && !String.IsNullOrWhiteSpace(model.Id))
            {
                if (Guid.TryParse(model.Id, out Guid uid))
                {
                    if (await __UserRepository.DeleteAsync(uid))
                    {
                        return Json(new { success = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} deleted {ENTITY_NAME}." });
                    }
                }
            }

            return Json(new { error = $"{GlobalConstants.ERROR_ACTION_PREFIX} delete {ENTITY_NAME}." });
        }
    }
}