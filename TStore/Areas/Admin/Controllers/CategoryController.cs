using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TStore.Data;

namespace TStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController(TStoreContext context) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string Name, string Brands, IFormFile FormImage, bool? ShowinSlider)
        {
            bool isChecked = ShowinSlider ?? false;

            // پردازش اطلاعات

            return View("Index");
        }
    }
}