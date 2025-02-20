using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TStore.Areas.Admin.ViewModels;
using TStore.Data;

namespace TStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController(TStoreContext context) : Controller
    {
        public IActionResult Index()
        {
            return View(new DashboardViewModel
            {
                ProductsCount = context.Products.Count(),
                CategoreisCount = context.Categories.Count(),
                UserCount = context.Users.Count()
            });
        }
    }
}
