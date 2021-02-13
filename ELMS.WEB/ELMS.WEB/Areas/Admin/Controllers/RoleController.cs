using ELMS.WEB.Areas.Admin.Models.Role;
using ELMS.WEB.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ELMS.WEB.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> __RoleManager;
        private readonly string MODEL_NAME = "Role";

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            __RoleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        }

        public async Task<IActionResult> Index()
        {
            IndexViewModel _Model = new IndexViewModel
            {
                Roles = await __RoleManager.Roles.ToListAsync()
            };

            return View(_Model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateModalAsync()
        {
            CreateRoleViewModel _Model = new CreateRoleViewModel();
            return PartialView("_CreateModal", _Model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
                return PartialView("_CreateModal", model);
            }

            if (await __RoleManager.RoleExistsAsync(model.Name))
            {
                return RedirectToAction("Index", "Role", new { Area = "Admin", errorMessage = $"A role with that name already exists." });
            }

            await __RoleManager.CreateAsync(new IdentityRole(model.Name));

            return RedirectToAction("Index", "Role", new { Area = "Admin", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} added new {MODEL_NAME}." });
        }

        [HttpGet]
        public async Task<IActionResult> EditModalAsync(string uid)
        {
            IdentityRole _Role = await __RoleManager.FindByIdAsync(uid);

            if (_Role == null)
            {
                return RedirectToAction("Index", "Role", new { Area = "Admin", errorMessage = $"The role does not exist." });
            }

            UpdateRoleViewModal _Model = new UpdateRoleViewModal
            {
                Name = _Role.Name,
                UID = _Role.Id
            };

            return PartialView("_EditModal", _Model);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(UpdateRoleViewModal model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["ErrorMessage"] = "Invalid form submission";
                return PartialView("_EditModal", model);
            }

            IdentityRole _Role = await __RoleManager.FindByIdAsync(model.UID);

            if (_Role == null)
            {
                return RedirectToAction("Index", "Role", new { Area = "Admin", errorMessage = $"The role does not exist." });
            }

            _Role.Name = model.Name;
            _Role.NormalizedName = model.Name.ToUpper();
            await __RoleManager.UpdateAsync(_Role);

            return RedirectToAction("Index", "Role", new { Area = "Admin", successMessage = $"{GlobalConstants.SUCCESS_ACTION_PREFIX} edited {MODEL_NAME}." });
        }
    }
}