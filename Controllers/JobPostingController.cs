using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using Worktastic.Data;
using Worktastic.Models;

namespace Worktastic.Controllers
{
    public class JobPostingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobPostingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var jobPostingsFromDb = _context.JobPostings.Where(x=> x.OwnerUsername);
            return View();
        }
        public IActionResult CreatedEditJobPosting(int id)
        {
            return View();
        }
        public IActionResult CreateEditJob(JobPostingModel jobPostingModel, IFormFile file)
        {
            //beim Speichern
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
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
