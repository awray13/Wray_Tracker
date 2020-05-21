using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wray_Tracker.Models
{
    public class TicketComment
    {
        // The Primary Key for the Comment
        public int Id { get; set; }

        // This is the Primary Key of the Ticket entry that this comment belongs to
        public int TicketId { get; set; }

        // This is the Primary Key of the User that wrote this comment
        public string UserId { get; set; }

        public DateTime Created { get; set; }

        public string Body { get; set; }

        // Navigational properties
        public virtual Ticket Ticket { get; set; }

        public virtual ApplicationUser User { get; set; }


    }
}