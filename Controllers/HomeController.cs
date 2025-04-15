using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Worktastic.Data;
using Worktastic.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        //einzelnen Job raussuchen aus DB für das Modal
        [HttpGet]
        public IActionResult GetJobPosting(int id) 
        {
            if (id == 0) return BadRequest();

            var JobPostingFromDB = _context.JobPostings.SingleOrDefault(x => x.Id == id);
            if(JobPostingFromDB == null) return NotFound();

            return Ok(JobPostingFromDB);
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
