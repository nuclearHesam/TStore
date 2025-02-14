using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TStore.Data;

namespace TStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController(TStoreContext context) : Controller
    {
        public IActionResult Index()
        {
            return View(context.Categories.AsNoTracking());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Name, string Brands, IFormFile FormImage, bool? ShowinSlider)
        {
            bool isChecked = ShowinSlider ?? false;

            byte[] imageData = default!;
            if (FormImage != null)
            {
                using var memoryStream = new MemoryStream();
                await FormImage.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
            }

            var category = new Category
            {
                CategoryId = Guid.NewGuid().ToString(),
                Name = Name,
                Brands = Brands,
                ShowinSlider = isChecked,
                Image = imageData
            };

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var category = await context.Categories.FindAsync(id);

            if (category != null)
            {
                context.Categories.Remove(category);
                _ = await context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "Category deleted successfully!" });
        }
    }
}