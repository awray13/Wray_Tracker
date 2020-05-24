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
            GenrateTicketChangeNotifications(newTicket);

        }

        private void GenerateAssignmentNotifications(Ticket oldTicket, Ticket newTicket)
        {
            bool assigned = oldTicket.DeveloperId == null && newTicket.DeveloperId != null;
            bool unassigned = oldTicket.DeveloperId != null && newTicket.DeveloperId == null;
            bool reassigned = !assigned && !unassigned && oldTicket.DeveloperId != newTicket.DeveloperId;

            var created = DateTime.Now;

            if (assigned)
            {
                GenerateAssignmentNotifications(newTicket.Id, created, newTicket.DeveloperId);
                
            }
            else if (unassigned)
            {
                GenerateUnAssignmentNotifications(newTicket.Id, created, oldTicket.DeveloperId);
            }
            else if (reassigned)
            {
                GenerateAssignmentNotifications(newTicket.Id, created, newTicket.DeveloperId);
                GenerateUnAssignmentNotifications(newTicket.Id, created, oldTicket.DeveloperId);
            }
            
        }

        private void GenerateAssignmentNotifications(int id, DateTime created, string recipientId)
        {
            db.TicketNotifications.Add(new TicketNotification
            {
                Created = created,
                TicketId = id,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                RecipientId = recipientId,
                NotificationBody = $"You have been assigned to Ticket Id: {id} on {created.ToString("MMM dd, yyyy")}."

            });
            db.SaveChanges();
        }

        private void GenerateUnAssignmentNotifications(int id, DateTime created, string recipientId)
        {
            db.TicketNotifications.Add(new TicketNotification
            {
                Created = created,
                TicketId = id,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                RecipientId = recipientId,
                NotificationBody = $"You have been unassigned to Ticket Id: {id} on {created.ToString("MMM dd, yyyy")}."

            });
            db.SaveChanges();
        }

        private void GenrateTicketChangeNotifications(Ticket ticket)
        {
            var created = DateTime.Now;

            db.TicketNotifications.Add(new TicketNotification
            {
                Created = created,
                TicketId = ticket.Id,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                RecipientId = ticket.DeveloperId,
                NotificationBody = $"Changes have been made to Ticket Id: {ticket.Id}."
            });
            db.SaveChanges();
        }

    }
}