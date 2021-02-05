using ELMS.WEB.Areas.Admin.Models.Permission;
using ELMS.WEB.Entities.Admin;
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
    }
}
