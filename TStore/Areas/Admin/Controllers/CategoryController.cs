using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TStore.Data;

namespace TStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController(IWebHostEnvironment webHostEnvironment, TStoreContext context) : Controller
    {
        public IActionResult Index()
        {
            return View(context.Categories.AsNoTracking());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Name, string Brands, IFormFile FormImage, bool? ShowinSlider)
        {
            bool isChecked = ShowinSlider ?? false;

            string imageData = "";
            if (FormImage != null && FormImage.Length != 0)
            {
                string UploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(UploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(FormImage.FileName);
                string filePath = Path.Combine(UploadsFolder, uniqueFileName);

                using FileStream fileStream = new(filePath, FileMode.Create);
                await FormImage.CopyToAsync(fileStream);

                imageData = "/images/" + uniqueFileName;
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

                string path = webHostEnvironment.WebRootPath + category.Image;
                System.IO.File.Delete(path);

                _ = await context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "Category deleted successfully!" });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var category = await context.Categories.FindAsync(id);
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string CategoryId, string Name, string Brands, IFormFile? FormImage, bool ShowinSlider)
        {
            var category = await context.Categories.FindAsync(CategoryId);
            if (category == null)
            {
                return NotFound();
            }

            category.Name = Name;
            category.Brands = Brands;
            category.ShowinSlider = ShowinSlider;

            string imageData = "";
            if (FormImage != null && FormImage.Length != 0)
            {
                string UploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(UploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(FormImage.FileName);
                string filePath = Path.Combine(UploadsFolder, uniqueFileName);

                using FileStream fileStream = new(filePath, FileMode.Create);
                await FormImage.CopyToAsync(fileStream);

                imageData = "/images/" + uniqueFileName;

                category.Image = imageData;
            }

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}