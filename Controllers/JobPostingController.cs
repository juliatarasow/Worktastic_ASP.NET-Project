using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
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
            var jobPostingsFromDb = _context.JobPostings.Where(x => x.OwnerUsername == User.Identity.Name).ToList();
            return View(jobPostingsFromDb);
        }

        //bearbeiten
        public IActionResult CreatedEditJobPosting(int id)
        {
            var jobPosting = _context.JobPostings.Find(id); // Annahme: _context ist dein Datenkontext
            if (jobPosting == null)
            {
                return NotFound();
            }

            return View(jobPosting); // Zeige nur das spezifische JobPosting
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

            // Überprüfen, ob die Jobanzeige bereits existiert
            var existingJob = _context.JobPostings.Find(jobPostingModel.Id);
            if (existingJob != null)
            {
                // Bestehenden Eintrag überschreiben
                existingJob.JobTitle = jobPostingModel.JobTitle;
                existingJob.JobDescription = jobPostingModel.JobDescription;
                existingJob.Salary = jobPostingModel.Salary;
                existingJob.JobLocation = jobPostingModel.JobLocation;
                existingJob.CompanyName = jobPostingModel.CompanyName;

                // Falls eine neue Datei hochgeladen wurde, ersetze das Bild
                if (file != null)
                {
                    existingJob.CompanyImage = jobPostingModel.CompanyImage;
                }

                _context.JobPostings.Update(existingJob);
            }
            else
            {
                // Falls nein, erstelle eine neue Anzeige
                _context.JobPostings.Add(jobPostingModel);
            }

            // Speichern der Änderungen in der Datenbank
            _context.SaveChanges();

            // Zurück zur Übersicht
            return RedirectToAction("Index", "JobPosting");
        }


        /*  public IActionResult CreateEditJob(JobPostingModel jobPostingModel, IFormFile file)
          {
              //beim Speichern übergeben
              jobPostingModel.OwnerUsername = User.Identity.Name;

              if (file != null)
              {
                  using (var ms = new MemoryStream())
                  {
                      file.CopyTo(ms);
                      var bytes = ms.ToArray();
                      jobPostingModel.CompanyImage = bytes;
                  }
              }
              _context.JobPostings.Add(jobPostingModel);
              _context.SaveChanges();

              return RedirectToAction("Index");
          }*/

        public IActionResult DeleteJobPosting(int id)
        {
            var job = _context.JobPostings.Find(id);
            if (job == null) return NotFound();

            _context.JobPostings.Remove(job);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
