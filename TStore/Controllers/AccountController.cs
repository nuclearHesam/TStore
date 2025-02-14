using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TStore.Data;

namespace TStore.Controllers
{
    public class AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : Controller
    {
        public IActionResult Register()
        {
            ViewData["isRegisterPage"] = true;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(string username, string email, string password, string firstName, string lastName)
        {
            var user = new ApplicationUser { UserName = username, Email = email, FirstName = firstName, LastName = lastName };
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description); 
            }
            return View();
        }

        public IActionResult Login()=> View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, bool rememberMe)
        {
            var result = await signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");
            
            ModelState.AddModelError(string.Empty, "The username or password is incorrect."); 
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
