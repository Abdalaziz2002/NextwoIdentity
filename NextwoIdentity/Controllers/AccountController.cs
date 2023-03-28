using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NextwoIdentity.Models;
using NextwoIdentity.Models.ViewModels;
using System.Diagnostics;

namespace NextwoIdentity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        #region Configuration
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private RoleManager<IdentityRole> roleManager;


        public AccountController(UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager,
            RoleManager<IdentityRole> _roleManager)
        {
            userManager= _userManager;
            signInManager= _signInManager;
            roleManager=_roleManager;
        }
        #endregion


        #region Users
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityUser user = new IdentityUser
                {
                    UserName = model.Email,
                    Email= model.Email,
                    PhoneNumber=model.Phone
                };
                var result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);
            }

            return View(model);
        }

        [AllowAnonymous]

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync
                    (model.Email!, model.Password!, false, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid user name or password");
                return View(model); 
            }

            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }
        #endregion

        #region Roles
        [Authorize(Roles ="Admin")]
        public IActionResult CreateRole()
        {
           
            return View();  
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(CreteRoleViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name=model.RoleName
                };
                var result=await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("RolesList");
                }
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);
            }
            return View(model); 
        }
        [Authorize(Roles = "Admin")]

        public IActionResult RolesList()
        {
            return View(roleManager.Roles);
        }
        [HttpPost]
        //public async Task<IActionResult> ModifyUsersInRole(string id, List<UserRoleViewModel> models)
        //{
        //    if (id == null)
        //    {
        //        return RedirectToAction(nameof(RolesList));
        //    }
        //    var role = await roleManager.FindByIdAsync(id);
        //    if (role == null)
        //    {
        //        return RedirectToAction(nameof(ErrorPage));
        //    }
        //    IdentityResult result = new IdentityResult();
        //    for (int i = 0; i < models.Count; i++)
        //    {

        //        var user = await userManager.FindByIdAsync(models[i].UserId!);
        //        if (models[i].IsSelected && (!await userManager.IsInRoleAsync(user!, role.Name!)))
        //        {
        //            result = await userManager.AddToRoleAsync(user, role.Name!);
        //        }
        //        else if (!models[i].IsSelected && (await userManager.IsInRoleAsync(user!, role.Name!)))
        //        {
        //            result = await userManager.RemoveFromRoleAsync(user, role.Name!);
        //        }

        //    }
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction(nameof(RolesList));
        //    }
        //    return View(models);

        //}


        /*public IActionResult AccessDeny()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("AccessDeny");
            }
            return View();
        }*/
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> EditRole(string id)
        {
            if (id == null)
            {
                return RedirectToAction("RolesList");
            }
            var role=await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return RedirectToAction("RolesList");
            }
            EditViewModel model = new EditViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditViewModel model)
        {
            if(ModelState.IsValid) {
                var role = await roleManager.FindByIdAsync(model.RoleId!);
                if (role == null)
                {
                    return RedirectToAction(nameof(ErrorPage));
                }
                role.Name = model.RoleName;
                var result=await roleManager.UpdateAsync(role);
                if(result.Succeeded)
                {
                    return RedirectToAction(nameof(RolesList));
                }
                foreach(var err in result.Errors)
                {
                    ModelState.AddModelError(err.Code, err.Description);
                }
                return View(model);
            }
            return View(model);
        }
        public IActionResult ErrorPage()
        {
            return View();
        }
        #endregion
    }

}
