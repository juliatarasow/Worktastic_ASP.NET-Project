using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Worktastic.Data;
using Worktastic.Models;
using Worktastic.Filters;

namespace Worktastic.Controllers
{
    [Route("api/jobposting")]
    [ApiController]
    [ApiKeyAuthorization]

    public class ApiJobPostingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiJobPostingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var allJobPostings = _context.JobPostings.ToList();
            return Ok(allJobPostings);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id) 
        {
            var getJobPostingById = _context.JobPostings.SingleOrDefault(x => x.Id == id);
            if (getJobPostingById == null)
            {
                return NotFound();
            }
            return Ok(getJobPostingById);
        }

        [HttpPost("Create")]
        public IActionResult Create(JobPostingModel jobPostingModel)
        {
            if (jobPostingModel.Id != 0)
            {
                return BadRequest("Darf keine ID enthalten");
            }
            _context.JobPostings.Add(jobPostingModel);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var getJobPostingById = _context.JobPostings.SingleOrDefault(x => x.Id == id);

            if(getJobPostingById == null)
            {
                return NotFound();
            }

            _context.JobPostings.Remove(getJobPostingById);
            _context.SaveChanges();
            return Ok("Objekt wurde gelöscht!");
        }

        [HttpPut("Update")]
        public IActionResult Update(JobPostingModel jobPostingModel)
        {
            if(jobPostingModel.Id == 0)
            {
                return BadRequest("Objekt hat keine ID");
            }
            _context.JobPostings.Update(jobPostingModel);
            _context.SaveChanges();
            return Ok("Objekt gespeichert");
        }
    }
}
