using ELMS.WEB.Areas.Admin.Models.Permission;
using ELMS.WEB.Entities.Admin;
using ELMS.WEB.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class PermissionController : Controller
    {
        private readonly UserManager<IdentityUser> __UserManager;

        public PermissionController(UserManager<IdentityUser> userManager)
        {
            __UserManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        public IActionResult Index(string successMessage = "", string errorMessage = "")
        {
            if (!string.IsNullOrWhiteSpace(successMessage))
            {
                ViewData["SuccessMessage"] = successMessage;
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                ViewData["ErrorMessage"] = errorMessage;
            }

            IndexViewModel _Model = new IndexViewModel()
            {
                Claims = ClaimsStore.AllClaims
            };

            return View(_Model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateModalAsync()
        {
            CreateViewModel _Model = new CreateViewModel();
            return PartialView("_CreateModal", _Model);
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateAsync(CreateViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ViewData["ErrorMessage"] = "Invalid form submission";
        //        return PartialView("_CreateModal", model);
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> EditUserPermissionsAsync(UserClaimsViewModel model)
        {
            IdentityUser _User = await __UserManager.FindByIdAsync(model.UserID);

            if (_User == null)
            {
                return RedirectToAction("DetailsView", "User", new { Area = "Admin", uid = model.UserID, errorMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} find User." });
            }

            IList<Claim> _Claims = await __UserManager.GetClaimsAsync(_User);
            IdentityResult _Result = await __UserManager.RemoveClaimsAsync(_User, _Claims);

            if (!_Result.Succeeded)
            {
                return RedirectToAction("DetailsView", "User", new { Area = "Admin", uid = model.UserID, errorMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} update User Permissions." });
            }

            _Result = await __UserManager.AddClaimsAsync(_User,
                model.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimValue)));

            if (!_Result.Succeeded)
            {
                return RedirectToAction("DetailsView", "User", new { Area = "Admin", uid = model.UserID, errorMessage = $"{GlobalConstants.ERROR_ACTION_PREFIX} update User Permissions." });
            }

            return RedirectToAction("DetailsView", "User", new { Area = "Admin", uid = model.UserID, errorMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} updated User Permissions." });
        }
    }
}
