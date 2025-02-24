using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TStore.Data;

namespace TStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController(IWebHostEnvironment webHostEnvironment, TStoreContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.Categories = await context.Categories.ToArrayAsync();
            return View(await context.Products.Include(p => p.Category).AsNoTracking().ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Name, decimal Price, string Brand, IFormFile FormImage, string? Description, string CategoryId)
        {
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

            Product product = new()
            {
                ProductId = Guid.NewGuid().ToString(),
                Name = Name,
                Price = Price,
                Brand = Brand,
                Image = imageData,
                Description = Description,
                CategoryId = CategoryId
            };

            await context.Products.AddAsync(product);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await context.Products.FindAsync(id);

            if (product != null)
            {
                context.Products.Remove(product);

                string path = webHostEnvironment.WebRootPath + product.Image;
                System.IO.File.Delete(path);

                _ = await context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "Product deleted successfully!" });
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Categories = await context.Categories.ToArrayAsync();
            return View(await context.Products.FirstOrDefaultAsync(p => p.ProductId == id));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string ProductId, string Name, int Price,string Brand, IFormFile? FormImage, string Description,string CategoryId)
        {
            var product = await context.Products.FindAsync(ProductId);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = Name;
            product.Brand = Brand;
            product.Price = Price;
            product.Description = Description;
            product.CategoryId = CategoryId;

            string imageData = "";
            if (FormImage != null && FormImage.Length != 0)
            {
                string path = webHostEnvironment.WebRootPath + product.Image;
                System.IO.File.Delete(path);

                string UploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(UploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(FormImage.FileName);
                string filePath = Path.Combine(UploadsFolder, uniqueFileName);

                using FileStream fileStream = new(filePath, FileMode.Create);
                await FormImage.CopyToAsync(fileStream);

                imageData = "/images/" + uniqueFileName;

                product.Image = imageData;
            }

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
