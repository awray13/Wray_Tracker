using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wray_Tracker.ViewModels
{
    public class CustomUserData
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string RoleName { get; set; }

        public List<string> ProjectNames { get; set; }

        public List<string> Tickets { get; set; }

        public string FullName 
        {
            get
            {
                return $"{LastName}, {FirstName}"; 
            }
        }


    }
}