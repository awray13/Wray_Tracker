using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wray_Tracker.Models;

namespace Wray_Tracker.Helper
{
    
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            // Manage a Dev assignment notification
            GenerateAssignmentNotifications(oldTicket, newTicket);

            //
            GenrateTicketChangeNotifications(oldTicket, newTicket);

        }

        private void GenerateAssignmentNotifications(Ticket oldTicket, Ticket newTicket)
        {
            bool assigned = oldTicket.DeveloperId == null && newTicket.DeveloperId != null;
            bool unassigned = oldTicket.DeveloperId != null && newTicket.DeveloperId == null;
            bool reassigned = !assigned && !unassigned && oldTicket.DeveloperId != newTicket.DeveloperId;

            if (assigned)
            {
                var created = DateTime.Now;
                db.TicketNotifications.Add(new TicketNotification
                {
                    Created = created,
                    TicketId = newTicket.Id,
                    SenderId = HttpContext.Current.User.Identity.GetUserId(),
                    RecipientId = newTicket.DeveloperId,
                    NotificationBody = $"You have been assigned to Ticket Id: {newTicket.Id} on {created.ToString("MMM dd, yyyy")}."
                
                });
            }
            db.SaveChanges();
        }

        private void GenrateTicketChangeNotifications(Ticket oldTicket, Ticket newTicket)
        {

        }

    }
}