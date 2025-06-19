using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIMEA.Models;

namespace SIMEA.Controllers
{
    [Authorize(Roles = "Root, Admin")]
    public class UserRolesController : Controller
    {
        private readonly UserManager<IdentityExtendUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserRolesController> _logger;

        public UserRolesController(
            UserManager<IdentityExtendUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<UserRolesController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                var userViewModels = new List<UserViewModel>();

                foreach (var user in users)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var userViewModel = new UserViewModel
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        AccountStatus = user.AccountStatus,
                        Roles = roles.Select(r => new RoleViewModel { Name = r }).ToList()
                    };
                    userViewModels.Add(userViewModel);
                }

                return View(userViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching users");
                return RedirectToAction("ErrorPage", "Home", new { area = "" });
            }
        }
        
        public IActionResult Create()
        {
            try
            {
                var roles = _roleManager.Roles
                    .Select(r => new RoleViewModel { Id = r.Id, Name = r.Name })
                    .Where(r => User.IsInRole("Root") || r.Name != "Root")
                    .ToList();

                var model = new UserViewModel
                {
                    Roles = roles
                };

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while preparing the Create view");
                return RedirectToAction("ErrorPage", "Home", new { area = "" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new IdentityExtendUser
                    {
                        FullName = model.FullName,
                        UserName = model.UserName,
                        Email = model.Email,
                        AccountStatus = model.AccountStatus,
                        EmailConfirmed = model.EmailConfirmed
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        foreach (var roleName in model.SelectedRoles)
                        {
                            var role = await _roleManager.FindByNameAsync(roleName);
                            if (role != null)
                            {
                                await _userManager.AddToRoleAsync(user, role.Name);
                            }
                        }

                        _logger.LogInformation("Created a new user with ID {UserId}", user.Id);
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating a new user");
                    ModelState.AddModelError(string.Empty, "An error occurred while creating the user. Please try again.");
                }
            }

            var roles = _roleManager.Roles
                .Select(r => new RoleViewModel { Id = r.Id, Name = r.Name })
                .Where(r => User.IsInRole("Root") || r.Name != "Root")
                .ToList();
            model.Roles = roles;

            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = _roleManager.Roles
                .Select(r => new RoleViewModel { Id = r.Id, Name = r.Name })
                .ToList();

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Email = user.Email,
                AccountStatus = user.AccountStatus,
                EmailConfirmed = user.EmailConfirmed,
                Roles = roles,
                SelectedRoles = userRoles.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);

                if (user == null)
                {
                    return NotFound();
                }

                user.FullName = model.FullName;
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.AccountStatus = model.AccountStatus;
                user.EmailConfirmed = model.EmailConfirmed;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var rolesToRemove = userRoles.Except(model.SelectedRoles).ToList();
                    var rolesToAdd = model.SelectedRoles.Except(userRoles).ToList();

                    result = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to remove user roles.");
                        return View(model);
                    }

                    result = await _userManager.AddToRolesAsync(user, rolesToAdd);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to add user roles.");
                        return View(model);
                    }

                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            var roles = _roleManager.Roles
                .Select(r => new RoleViewModel { Id = r.Id, Name = r.Name })
                .ToList();
            model.Roles = roles;

            return View(model);
        }
    }
}
