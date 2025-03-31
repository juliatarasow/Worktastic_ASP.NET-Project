namespace Worktastic.Models
{
    public class JobPostingModel
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string JobLocation { get; set; }
        public DateTime StartDate { get; set; }
        public int Salary { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactMail { get; set; }
        public string CompanyWebsite { get; set; }
        public byte[] CompanyImage { get; set; }
        public string OwnerUsername { get; set; }
    }
}