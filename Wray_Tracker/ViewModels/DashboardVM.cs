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


    }
}