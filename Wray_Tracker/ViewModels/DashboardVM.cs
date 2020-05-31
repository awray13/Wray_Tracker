using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wray_Tracker.Models;

namespace Wray_Tracker.ViewModels
{
    public class DashboardVM
    {
        public ICollection<Project> MyProjects { get; set; } // Projects assigned to user

        public ICollection<Ticket> MyTickets { get; set; } // Tickets assigned to user

        public ICollection<Ticket> AllTickets { get; set; }

        public ICollection<Project> AllProjects { get; set; }

        public ICollection<TicketHistory> AllTicketHistory { get; set; }

        public ICollection<CustomUserData> UserData { get; set; }

        public string Id { get; set; }

        public int TicketCount { get; set; }

        public int HighPriorityTicketCount { get; set; }

        public int NewTicketCount { get; set; }

        public int TotalComments { get; set; }

        public string RoleNames { get; set; }

        public ProjectVM ProjectVM { get; set; }
        

        public DashboardVM()
        {
            MyProjects = new List<Project>();
            MyTickets = new List<Ticket>();
            AllTickets = new List<Ticket>();
            AllTicketHistory = new List<TicketHistory>();
            AllProjects = new List<Project>();
            UserData = new List<CustomUserData>();
            ProjectVM = new ProjectVM();
            
            


        }


    }
}