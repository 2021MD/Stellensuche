using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Workstatic.Models;
using Workstatic.Data;

namespace Workstatic.Controllers
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
            return View();
        }

        public IActionResult CreateEditJobPosting(int id)
        {
            return View();
        }

        public IActionResult CreateEditJob(JobPosting jobPosting, IFormFile file)
        {
            if(file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var bytes = ms.ToArray();
                    jobPosting.CompanyImage = bytes;
                }
            }

            if (jobPosting.Id == 0)
            {
                _context.JobPostings.Add(jobPosting);
            } else 
            {
                var jobFromDb = _context.JobPostings.SingleOrDefault(x => x.Id == jobPosting.Id);

                if(jobFromDb == null)
                {
                    return NotFound();
                }

                jobFromDb.CompanyImage = jobPosting.CompanyImage;
                jobFromDb.CompanyName = jobPosting.CompanyName;
                jobFromDb.ContactMail = jobPosting.ContactMail;
                jobFromDb.ContactPhone = jobPosting.ContactPhone;
                jobFromDb.ContactWebsite = jobPosting.ContactWebsite;
                jobFromDb.Description = jobPosting.Description;
                jobFromDb.JobLocation = jobPosting.JobLocation;
                jobFromDb.JobTitle = jobPosting.JobTitle;
                jobFromDb.Salary = jobPosting.Salary;
                jobFromDb.StartDate = jobPosting.StartDate;
            }

            _context.JobPostings.Add(jobPosting);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
