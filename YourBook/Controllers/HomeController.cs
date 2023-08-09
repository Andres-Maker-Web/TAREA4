using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using YourBook.Data;
using YourBook.Data.Repository;
using YourBook.Models;

namespace YourBook.Controllers
{
    public class HomeController : Controller
    {
        private readonly YourBookDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, YourBookDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Books != null ?
                          View(await _context.Books.ToListAsync()) :
                          Problem("Entity set 'YourBookDbContext.Books'  is null.");
        }

        //[ActionName("IndexWithSearch")]
        //public async Task<IActionResult> Index(string searchString)
        //{
        //    var bks = from s in _context.Books
        //                   select s;
        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        bks = bks.Where(s => s.Title.Contains(searchString)
        //                               || s.Title.Contains(searchString));
        //    }

        //    return _context.Books != null ?
        //                  View(await bks.ToListAsync()) :
        //                  Problem("Entity set 'YourBookDbContext.Books'  is null.");
        //}


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}