using Microsoft.AspNetCore.Mvc;
using Worktastic.Data;
using Worktastic.Models;

namespace Worktastic.Controllers
{
    
    public class JobPostingController : Controller
    {
        //Instanz des Datenbank-Kontexts
        private readonly ApplicationDbContext _context;

        public JobPostingController(ApplicationDbContext context) //Datenbank-Kontext von ASP.NET Core Dependency Injection - Überbabe
        {            
            _context = context; //_context -> private Variable
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

        //JobPostings bearbeiten
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

            // Falls eine Datei hochgeladen wurde, speichere sie als Byte-Array
            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    jobPostingModel.CompanyImage = ms.ToArray();
                }
            }
                       
            if (jobPostingModel.Id == 0)
            {
                _context.JobPostings.Add(jobPostingModel);
            }
            else 
            {
                var existingJob = _context.JobPostings.Find(jobPostingModel.Id);

                if (existingJob == null) return NotFound();
                
                if (file != null) //if (jobPostingModel.CompanyImage != null)
                {
                    existingJob.CompanyImage = jobPostingModel.CompanyImage;
                }

                existingJob.JobTitle = jobPostingModel.JobTitle;
                existingJob.JobDescription = jobPostingModel.JobDescription;
                existingJob.JobLocation = jobPostingModel.JobLocation;
                existingJob.StartDate = jobPostingModel.StartDate;
                existingJob.Salary = jobPostingModel.Salary;
                existingJob.CompanyName = jobPostingModel.CompanyName;
                existingJob.ContactName = jobPostingModel.ContactName;
                existingJob.ContactMail = jobPostingModel.ContactMail;
                //existingJob.CompanyWebsite = jobPostingModel.CompanyWebsite;           
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

            return RedirectToAction("Index");
        }

        public IActionResult JobPostingDetails(int id)
        {
            var jobPostingFromDb = _context.JobPostings.Find(id);
            if (jobPostingFromDb == null)
            {
                return NotFound();
            }
            return View(jobPostingFromDb);
        }
    }
}
