using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechJobsPersistent.Models;

namespace TechJobsPersistent.ViewModels
{
    public class AddJobViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        public int Id { get; set; }
        [Required(ErrorMessage ="At least one employer must be attached to this job.")]
        public List<SelectListItem> Employers { get; set; }
        public List<Skill> Skills { get; set; }

        public AddJobViewModel() { }

        public AddJobViewModel(List<Employer> possibleEmployers, List<Skill> possibleSkills) 
        {
            Employers = new List<SelectListItem>();
            Skills = possibleSkills;

            foreach(Employer employer in possibleEmployers)
            {
                Employers.Add(new SelectListItem
                {
                    Value = employer.Id.ToString(),
                    Text = employer.Name
                });
            }

        }

    }
}
