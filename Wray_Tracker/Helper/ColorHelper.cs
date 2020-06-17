using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Wray_Tracker.Helper
{
    public class ColorHelper
    {
        public static string GetStatusColorClass(string name)
        {
            var colorClass = "";
            switch (name)
            {
                case "New":
                    colorClass = "text-maroon";
                    break;
                case "Assigned":
                    colorClass = "text-primary";
                    break;
                case "Reopened":
                    colorClass = "text-danger";
                    break;
                case "Resolved":
                    colorClass = "text-success";
                    break;
                case "Archived":
                    colorClass = "text-muted";
                    break;
                default:
                    colorClass = "text-info";
                    break;
            }
            return colorClass;
        }

        public string GetUserColorClass(string name)
        {
            var userColorClass = "";
            switch (name)
            {
                case "Admin":
                    userColorClass = "text-danger";
                    break;
                case "Manager":
                    userColorClass = "text-primary";
                    break;
                case "Developer":
                    userColorClass = "text-success";
                    break;
                case "Submitter":
                    userColorClass = "text-maroon";
                    break;
                default:
                    userColorClass = "text-info";
                    break;

            }

            return userColorClass;
        }
        
    }
}