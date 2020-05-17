using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wray_Tracker.Models;

namespace Wray_Tracker.ViewModels
{
    public class UserProfileVM
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string RoleName { get; set; }

        public string Email { get; set; }

        public ICollection<Project> MyProjects { get; set; } // Projects assigned to user

        public ICollection<Ticket> MyTickets { get; set; } // Tickets assigned to user

        public string FullName
        {
            get
            {
                return $"{LastName}, {FirstName}";
            }
        }

    }
}