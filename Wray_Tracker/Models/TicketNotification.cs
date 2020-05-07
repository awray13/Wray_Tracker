using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wray_Tracker.Models
{
    public class TicketNotification
    {
        public int Id { get; set; }

        public int TicketId { get; set; }

        // The user you caused the notification to be generated
        public string SenderId { get; set; }

        // The user the notification is sent to
        public string RecipientId { get; set; }

        public bool IsRead { get; set; }

        public string NotificationBody { get; set; }

        public DateTime Created { get; set; }

        public virtual Ticket Ticket { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        public virtual ApplicationUser Recipient { get; set; }

    }
}