using Microsoft.AspNetCore.Mvc;
using Movies_Alexandra_marian.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Movies_Alexandra_marian.Data;
using Movies_Alexandra_marian.Models.MovieViewModels;

namespace Movies_Alexandra_marian.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieContext _context;

        public HomeController(MovieContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<ActionResult> Statistics()
        {
            IQueryable<HistoryGroup> data =
            from order in _context.Histories
            group order by order.OrderDate into dateGroup
            select new HistoryGroup()
            {
                OrderDate = dateGroup.Key,
                MovieCount = dateGroup.Count()
            };
            return View(await data.AsNoTracking().ToListAsync());
        }
    }
}