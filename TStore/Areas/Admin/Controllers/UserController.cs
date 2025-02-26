using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TStore.Data;

namespace TStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController(UserManager<ApplicationUser> userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            var userViewModels = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                UserName = u.UserName,
                Image = u.Image
            });

            return View(userViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string FirstName,string LastName,string Email,string UserName,string Password)
        {
            var user = new ApplicationUser {FirstName = FirstName,LastName= LastName, Email =Email,UserName = UserName };
            await userManager.CreateAsync(user, Password);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            
            if(user == null)
            {
                return NotFound();
            }
            
            await userManager.DeleteAsync(user);

            return RedirectToAction("Index");
        }
    }
}
