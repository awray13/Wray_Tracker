using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wray_Tracker.ViewModels
{
    public class UserRoleData
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string RoleName { get; set; }

        public string FullName 
        {
            get
            {
                return $"{LastName}, {FirstName}"; 
            }
        }


    }
}