using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wray_Tracker.Models;

namespace Wray_Tracker.Helper
{
    public class TicketHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper roleHelper = new UserRoleHelper();
        private UserProjectHelper projectHelper = new UserProjectHelper();

        public ICollection<ApplicationUser> AssignableDevelopers(int projectId)
        {
            var developers = roleHelper.UsersInRole("Developer");
            var onProject = projectHelper.UsersOnProject(projectId);
            return developers.Intersect(onProject, new CustomComparer.TicketComparer()).ToList();
        }

        public List<Ticket> ListMyTickets(string userId)
        {
            var myTickets = new List<Ticket>();
            
            //var userId = HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);
            var myRole = roleHelper.ListUserRoles(userId).FirstOrDefault();
            switch (myRole)
            {
                case "Admin":
                case "DemoAdmin":
                    myTickets.AddRange(db.Tickets);
                    break;
                case "Manager":
                case "DemoPM":
                    myTickets.AddRange(db.Projects.Where(p => p.ManagerId == userId).SelectMany(p => p.Tickets));
                    break;
                case "Developer":
                case "DemoDeveloper":
                    myTickets.AddRange(db.Tickets.Where(t => t.IsArchived == false).Where(t => t.DeveloperId == userId));
                    break;
                case "Submitter":
                case "DemoSubmitter":
                    myTickets.AddRange(db.Tickets.Where(t => t.IsArchived == false).Where(t => t.SubmitterId == userId));
                    break;
            }
            return myTickets;
        }
        //public List<Ticket> ListMyTicketsByPriority(string priority)
        //{
        //    var myTickets = new List<Ticket>();
        //}
    }
}