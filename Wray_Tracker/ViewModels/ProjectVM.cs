using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wray_Tracker.Models;

namespace Wray_Tracker.ViewModels
{
    public class ProjectVM
    {
        public string Id { get; set; }
        public int ProjectCount { get; set; }

        public ICollection<Project> AllProjects { get; set; }

        public ICollection<Project> ProjectIds { get; set; }
        public ICollection<ApplicationUser> AllPMs { get; set; }
        public ICollection<ApplicationUser> AllDevs { get; set; }
        public ICollection<ApplicationUser> AllSubs { get; set; }

        public ProjectVM()
        {
            ProjectIds = new List<Project>();
            AllProjects = new List<Project>();
            AllPMs = new List<ApplicationUser>();
            AllDevs = new List<ApplicationUser>();
            AllSubs = new List<ApplicationUser>();
        }
        


    }
}