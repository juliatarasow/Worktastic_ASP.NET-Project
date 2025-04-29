using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Worktastic.Data;
using Worktastic.Models;

namespace Worktastic.Controllers
{
    
    public class JobPostingController : Controller
    {
        //Instanz des Datenbank-Kontexts
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public JobPostingController(ApplicationDbContext context, IWebHostEnvironment webHostEnviroment) //Datenbank-Kontext von ASP.NET Core Dependency Injection - Überbabe
        {            
            _context = context; //_context -> private Variable
            _webHostEnviroment = webHostEnviroment;
        }

        public IActionResult Index()
        {
            //Admin sieht alles von jedem
            if(User.IsInRole("Admin"))
            {
                var allPosrings = _context.JobPostings.ToList();
                return View(allPosrings);
            }

            var jobPostingsFromDb = _context.JobPostings.Where(x => x.OwnerUsername == User.Identity.Name).ToList();
            return View(jobPostingsFromDb);
        }

        public IActionResult GetJobPosting(int id)
        {
            var posting = _context.JobPostings.FirstOrDefault(j => j.Id == id);
            if (posting == null)
                return NotFound();

            return PartialView("_JobPostingDetailsPartial", posting);
        }

        //Jobposting bearbeiten
        public IActionResult CreatedEditJobPosting(int id)
        {
            if (id == 0) return View();

            //var jobPosting = _context.JobPostings.SingleOrDefault(x => x.Id == id);
            var jobPosting = _context.JobPostings.Find(id); // Annahme: _context ist dein Datenkontext
            if (jobPosting.OwnerUsername != User.Identity.Name && !User.IsInRole("Admin"))
            {
                return Unauthorized();
            }

            if (jobPosting == null)
            {
                return NotFound();
            }

            // Zeige nur das spezifische JobPosting
            return View(jobPosting); 
        }

        public IActionResult CreateEditJob(JobPostingModel jobPostingModel, IFormFile file)
        {

            // Den aktuellen Benutzer als Besitzer der Anzeige speichern
            jobPostingModel.OwnerUsername = User.Identity.Name;
            byte[] uploadedLogo = null;

            // Falls eine Datei hochgeladen wurde, speichere sie als Byte-Array
            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    uploadedLogo = ms.ToArray();
                }
            }
            else
            {
                var placeholderPath = Path.Combine(_webHostEnviroment.WebRootPath, "images", "placeholder-logo.png");

                if (System.IO.File.Exists(placeholderPath))
                {
                    uploadedLogo = System.IO.File.ReadAllBytes(placeholderPath);
                }
            }
                       
            if (jobPostingModel.Id == 0)
            {
                jobPostingModel.CompanyImage = uploadedLogo;
                _context.JobPostings.Add(jobPostingModel);
            }
            else 
            {
                var existingJob = _context.JobPostings.Find(jobPostingModel.Id);

                if (existingJob == null) return NotFound();
                
                if (file != null) //if (jobPostingModel.CompanyImage != null)
                {
                    existingJob.CompanyImage = uploadedLogo;
                }

                existingJob.JobTitle = jobPostingModel.JobTitle;
                existingJob.JobDescription = jobPostingModel.JobDescription;
                existingJob.JobLocation = jobPostingModel.JobLocation;
                existingJob.StartDate = jobPostingModel.StartDate;
                existingJob.Salary = jobPostingModel.Salary;
                existingJob.CompanyName = jobPostingModel.CompanyName;
                existingJob.ContactName = jobPostingModel.ContactName;
                existingJob.ContactMail = jobPostingModel.ContactMail;
                existingJob.CompanyWebsite = jobPostingModel.CompanyWebsite;           
            }

            // Speichern der Änderungen in der Datenbank
            _context.SaveChanges();

            // Zurück zur Übersicht
            return RedirectToAction("Index", "JobPosting");
        }

        [HttpPost]
        public IActionResult DeleteJobPosting(int id)
        {
            var job = _context.JobPostings.Find(id);
            if (job == null) return NotFound();

            _context.JobPostings.Remove(job);
            _context.SaveChanges();

            return Ok();
        }
    }
}
