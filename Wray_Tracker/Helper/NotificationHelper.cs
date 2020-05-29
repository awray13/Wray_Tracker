using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Wray_Tracker.Models;
using Wray_Tracker.ViewModels;

namespace Wray_Tracker.Helper
{
    
    public class NotificationHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        

        public async Task ManageNotifications(Ticket oldTicket, Ticket newTicket)
        {
            // Manage a Dev assignment notification
            await GenerateAssignmentNotifications(oldTicket, newTicket);

            // Manage ticket change notifications
            GenerateTicketChangeNotification(newTicket);
        }

        // Email to Dev about assignment/unassignment
        private async Task SendEmailAsync(TicketNotification notification)
        {
            try
            {
                var emailFrom = db.Users.Find(notification.SenderId).Email;

                emailFrom += $"<{WebConfigurationManager.AppSettings["emailfrom"]}>";

                var emailTo = db.Users.Find(notification.RecipientId).Email;

                var email = new MailMessage(emailFrom, emailTo)
                {
                    Subject = "Changes Made to Ticket Assigned to You",
                    Body = notification.NotificationBody,
                    IsBodyHtml = true
                };

                var emailSvc = new EmailService();
                await emailSvc.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }

        }

        private async Task GenerateAssignmentNotifications(Ticket oldTicket, Ticket newTicket)
        {
            bool assigned = oldTicket.DeveloperId == null && newTicket.DeveloperId != null;
            bool unassigned = oldTicket.DeveloperId != null && newTicket.DeveloperId == null;
            bool reassigned = !assigned && !unassigned && oldTicket.DeveloperId != newTicket.DeveloperId;

            var created = DateTime.Now;

            if (assigned)
            {
                await GenerateAssignmentNotification(newTicket.Id, created, newTicket.DeveloperId);
                
            }
            else if (unassigned)
            {
                await GenerateUnAssignmentNotification(newTicket.Id, created, oldTicket.DeveloperId);
            }
            else if (reassigned)
            {
                await GenerateUnAssignmentNotification(newTicket.Id, created, oldTicket.DeveloperId);
                await GenerateAssignmentNotification(newTicket.Id, created, newTicket.DeveloperId);
            }
            
        }

        private async Task GenerateAssignmentNotification(int id, DateTime created, string recipientId)
        {
            var newNotification = new TicketNotification
            {
                Created = created,
                TicketId = id,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                RecipientId = recipientId,
                NotificationBody = $"You have been assigned to Ticket Id: {id} on {created.ToString("MMM dd, yyyy")}."
            };

            db.TicketNotifications.Add(newNotification);

            db.SaveChanges();
            await SendEmailAsync(newNotification);
        }

        private async Task GenerateUnAssignmentNotification(int id, DateTime created, string recipientId)
        {
            var newNotification = new TicketNotification
            {
                Created = created,
                TicketId = id,
                SenderId = HttpContext.Current.User.Identity.GetUserId(),
                RecipientId = recipientId,
                NotificationBody = $"You have been unassigned from Ticket Id: {id} on {created.ToString("MMM dd, yyyy")}."

            };

            db.TicketNotifications.Add(newNotification);

            db.SaveChanges();
            await SendEmailAsync(newNotification);
        }

        private void GenerateTicketChangeNotification(Ticket ticket)
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