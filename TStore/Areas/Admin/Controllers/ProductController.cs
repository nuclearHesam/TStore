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
        public async Task<IActionResult> Create(string Name, decimal Price, string Brand, IFormFile FormImage, List<IFormFile>? Images, string? Description, string CategoryId)
        {
            Product product = new()
            {
                ProductId = Guid.NewGuid().ToString(),
                Name = Name,
                Price = Price,
                Brand = Brand,
                Image = await CreateImage(FormImage),
                Description = Description,
                CategoryId = CategoryId
            };

            foreach (var Image in Images)
            {
                product.ImageList.Add(await CreateImage(Image));
            }

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

                System.IO.File.Delete(webHostEnvironment.WebRootPath + product.Image);

                foreach (var image in product.ImageList)
                {
                    System.IO.File.Delete(webHostEnvironment.WebRootPath + image);
                }

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
        public async Task<IActionResult> Edit(string ProductId, string Name, int Price, string Brand, IFormFile? FormImage, List<IFormFile>? Images, string Description, string CategoryId)
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

            if (FormImage != null && FormImage.Length != 0)
            {
                product.Image = await CreateImage(FormImage);
            }

            product.ImageList = [];
            if (Images != null && Images.Count != 0)
            {
                foreach (var Image in Images)
                {
                    product.ImageList.Add(await CreateImage(Image));
                }
            }

            await context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(string id)
        {
            return View(await context.Products.FirstOrDefaultAsync(p => p.ProductId == id));
        }

        public async Task<string> CreateImage(IFormFile formFile)
        {
            if (formFile != null && formFile.Length != 0)
            {
                string UploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                Directory.CreateDirectory(UploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetExtension(formFile.FileName);
                string filePath = Path.Combine(UploadsFolder, uniqueFileName);

                using FileStream fileStream = new(filePath, FileMode.Create);
                await formFile.CopyToAsync(fileStream);

                return "/images/" + uniqueFileName;
            }
            return "";
        }
    }
}
