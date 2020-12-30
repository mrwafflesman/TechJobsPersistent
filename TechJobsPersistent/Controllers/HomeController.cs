using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext context;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();

            return View(jobs);
        }

        [HttpGet("/Add")]
        public IActionResult AddJob()
        {
            List<Employer> employers = context.Employers.ToList();
            List<Skill> skills = context.Skills.ToList();
            AddJobViewModel addJobViewModel = new AddJobViewModel(employers, skills);
            return View(addJobViewModel);
        }

        public IActionResult ProcessAddJobForm(AddJobViewModel addJobViewModel, string[] selectedSkills)
        {
            if (ModelState.IsValid)
            {
                int employerId = int.Parse(addJobViewModel.Employers.ElementAt(0).Value);
                Employer employer = (Employer)context.Employers.Where(e => e.Id == employerId);

                Job newJob = new Job
                {
                    Name = addJobViewModel.Name,
                    Employer = employer,
                    EmployerId = employerId
                };

                foreach (string skill in selectedSkills)
                {
                    JobSkill newJobSkill = new JobSkill();

                    newJobSkill.JobId = newJob.Id;
                    newJobSkill.Job = newJob;

                    newJobSkill.SkillId = int.Parse(skill);
                    newJobSkill.Skill = (Skill)context.Skills.Where(s => s.Id == int.Parse(skill));

                    context.JobSkills.Add(newJobSkill);
                }

                context.Jobs.Add(newJob);
                context.SaveChanges();

                return Redirect("/Index");
            }
            return View(addJobViewModel);
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }
}
