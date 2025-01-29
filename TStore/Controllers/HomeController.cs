using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TStore.Context;

namespace TStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly TStoreContext _context;

        public HomeController(TStoreContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.AsNoTracking().ToListAsync());
        }
        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Categories()
        {
            return View(await _context.Categories.AsNoTracking().ToListAsync());
        }
    }
}
