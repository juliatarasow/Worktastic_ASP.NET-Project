using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Worktastic.Data;

namespace Worktastic.Controllers
{
    [Route("api/jobposting")]
    [ApiController]

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
    }
}
