using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyMaterialShare.Database.Models;
using StudyMaterialShare.Web.Models;

namespace StudyMaterialShare.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterViewModel vm)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = vm.UserName,
                    Email = vm.Email,
                    DisplayName = vm.DisplayName,
                };
                var userCreateResult = await _userManager.CreateAsync(user,vm.Password);

                if(userCreateResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Sikertelen regisztráció!");
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromForm] LoginViewModel vm)
        {
            if(!ModelState.IsValid) return View(vm);

            var user = await _userManager.FindByNameAsync(vm.UserName);
            if (user == null)
            {
                ModelState.AddModelError("", "Hibás felhasználónév vagy jelszó");
                return View(vm);
            }
            var signInResult = await _signInManager
                .PasswordSignInAsync(user, vm.Password, false, false);

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Hibás felhasználónév vagy jelszó");
                return View(vm);
            }

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
