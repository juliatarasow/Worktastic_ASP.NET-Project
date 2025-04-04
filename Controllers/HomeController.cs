using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Worktastic.Data;
using Worktastic.Models;

namespace Worktastic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context; //_context -> private Variable
        }

        public IActionResult Index()
        {
            // Hole die JobPostings-Daten
            var jobPostingsFromDb = _context.JobPostings.ToList();

            return View(jobPostingsFromDb);
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
    }
}
