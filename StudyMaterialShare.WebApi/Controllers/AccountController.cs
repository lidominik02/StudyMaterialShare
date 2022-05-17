using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudyMaterialShare.Data;
using StudyMaterialShare.Database.Models;

namespace StudyMaterialShare.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDTO user)
        {
            if(_signInManager.IsSignedIn(User))
                await _signInManager.SignOutAsync();

            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Ok();
            }

            return Unauthorized("A bejelentkezés sikertelen!");
        }
        [Route("Admin/Login")]
        [HttpPost]
        public async Task<ActionResult> AdminLogin([FromBody] LoginDTO user)
        {
            if (_signInManager.IsSignedIn(User))
                await _signInManager.SignOutAsync();

            var result = await _signInManager.PasswordSignInAsync(user.UserName, user.Password,
                isPersistent: false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                return Unauthorized("A bejelentkezés sikertelen!");
            }

            bool isAdmin = (await _signInManager.UserManager.GetUsersInRoleAsync("admin"))
                .Any(admin => admin.UserName == user.UserName);
            if (!isAdmin)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            return Ok();
        }

        [Route("Logout")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
        }
    }
}
