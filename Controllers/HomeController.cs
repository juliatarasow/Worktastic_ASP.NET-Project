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
            return View();
        }

        public IActionResult GetJobPostingPartial(string query)
        {
            List<JobPostingModel> jobPostingsFromDb = new List<JobPostingModel>();

            if (string.IsNullOrWhiteSpace(query))
            {
                // Hole die JobPostings-Daten
                jobPostingsFromDb = _context.JobPostings.ToList();
            }
            else
            {
                string searchQuery = query.ToLower().Replace(" ", "");
                DateTime.TryParse(query, out DateTime parsedDate);
                int maxDistance = 2;

                // Hole alle JobPostings
                jobPostingsFromDb = _context.JobPostings.ToList();

                // Filtere die JobPostings nach den Suchkriterien
                jobPostingsFromDb = jobPostingsFromDb
                    .Where(x =>
                    {
                        string jobTitle = x.JobTitle.ToLower().Replace(" ", "");
                        string jobLocation = x.JobLocation.ToLower().Replace(" ", "");
                        string companyName = x.CompanyName.ToLower().Replace(" ", "");

                        return
                            jobTitle.Contains(searchQuery) ||
                            jobLocation.Contains(searchQuery) ||
                            companyName.Contains(searchQuery) ||
                            (parsedDate != default(DateTime) && x.StartDate.Date == parsedDate.Date) ||
                            LevenshteinDistance(jobTitle, searchQuery) <= maxDistance ||
                            LevenshteinDistance(jobLocation, searchQuery) <= maxDistance ||
                            LevenshteinDistance(companyName, searchQuery) <= maxDistance;
                    })
                    .ToList(); // Wende ToList() nach dem Where an
            }
            return PartialView("_jobPostingListPartial", jobPostingsFromDb);
        }


        private static int HandleEmptyStrings(string s, string t)
        {
            if(string.IsNullOrEmpty(s))
            {
                //return string.IsNullOrEmpty(t) ? 0 : t.Length;
                if (string.IsNullOrEmpty(t))
                {
                    return 0;
                }
                else
                {
                    return t.Length;
                }
            }

            if(string.IsNullOrEmpty(t))
            {
                return s.Length;
            }

            return -1;
        }

        //s = source; t = target
        public static int LevenshteinDistance(string s, string t)
        {
            //falls eines der Strings leer ist
            int shortcut = HandleEmptyStrings(s, t);
            if (shortcut != -1)
            {
                return shortcut;
            }

            

            int[,] matrix = new int[s.Length + 1, t.Length + 1];

            //Initialisierung der ersten Zeile und Spalte
            for (int i = 0; i <= s.Length; i++)
            {
                matrix[i, 0] = i;
            }

            for (int j = 0; j <= t.Length; j++)
            {
                matrix[0, j] = j;
            }

            for(int i = 1; i <= s.Length; i++)
            {
                for(int j = 1; j <= t.Length; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(
                        Math.Min(
                            matrix[i - 1, j] + 1,
                            matrix[i, j - 1] + 1
                            ),
                        matrix[i - 1, j - 1] + cost
                        );
                }
            }
          
            return matrix[s.Length, t.Length];
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
