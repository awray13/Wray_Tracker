using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wray_Tracker.Models;

namespace Wray_Tracker.Helper
{
    public class HistoryHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public void ManageHistoryRecordCreation(Ticket oldTicket, Ticket newTicket)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();
            var time = (DateTime)newTicket.Updated;

            // Now I can compare the new Ticket, "ticket" to the old Ticket, "oldTicket"
            // for changes that need to be recorded in the History table
            if (oldTicket.TicketPriorityId != newTicket.TicketPriorityId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "TicketPriorityId",
                    OldValue = oldTicket.TicketPriority.Name,
                    NewValue = newTicket.TicketPriority.Name,
                    ChangedOn = time,
                    TicketId = newTicket.Id,
                    UserId = userId
                };

                db.TicketHistories.Add(newHistoryRecord);
            }

            if (oldTicket.TicketStatusId != newTicket.TicketStatusId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "TicketStatusId",
                    OldValue = oldTicket.TicketStatus.Name,
                    NewValue = newTicket.TicketStatus.Name,
                    ChangedOn = time,
                    TicketId = newTicket.Id,
                    UserId = userId
                };

                db.TicketHistories.Add(newHistoryRecord);
            }

            if (oldTicket.TicketTypeId != newTicket.TicketTypeId)
            {
                var newHistoryRecord = new TicketHistory
                {
                    Property = "TicketTypeId",
                    OldValue = oldTicket.TicketType.Name,
                    NewValue = newTicket.TicketType.Name,
                    ChangedOn = time,
                    TicketId = newTicket.Id,
                    UserId = userId
                };

                db.TicketHistories.Add(newHistoryRecord);
            }

            if (oldTicket.Title != newTicket.Title)
            {
                var newHistoryRecord = new TicketHistory
                {
                    ChangedOn = time,
                    UserId = userId,
                    Property = "Title",
                    OldValue = oldTicket.Title,
                    NewValue = newTicket.Title,
                    TicketId = newTicket.Id
                };

                db.TicketHistories.Add(newHistoryRecord);
                
            }

            if (oldTicket.DeveloperId != newTicket.DeveloperId)
            {
                var newHistoryRecord = new TicketHistory 
                {
                    ChangedOn = time,
                    UserId = userId,
                    Property = "DeveloperId",
                    OldValue = oldTicket.DeveloperId == null ? "Unassigned" : oldTicket.Developer.FullName,
                    NewValue = newTicket.DeveloperId == null ? "Unassigned" : newTicket.Developer.FullName,
                    TicketId = newTicket.Id
                };

                db.TicketHistories.Add(newHistoryRecord);
                
            }

            db.SaveChanges();
        }

    }
}