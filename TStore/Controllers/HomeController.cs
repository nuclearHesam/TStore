using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TStore.Data;

namespace TStore.Controllers
{
    public class HomeController(TStoreContext context) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await context.Categories.AsNoTracking().Where(c=>c.ShowinSlider == true).ToListAsync());
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/Categories")]
        public async Task<IActionResult> Categories()
        {
            return View(await context.Categories.AsNoTracking().ToListAsync());
        }
    }
}
