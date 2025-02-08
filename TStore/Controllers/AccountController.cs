using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TStore.Data;

namespace TStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password, string firstName, string lastName)
        {
            var user = new ApplicationUser { UserName = username, Email = email, FirstName = firstName, LastName = lastName };
            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            return BadRequest(result.Errors);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool rememberMe)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            return Unauthorized("نام کاربری یا رمز عبور اشتباه است");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
