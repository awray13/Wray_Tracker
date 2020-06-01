using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Wray_Tracker.Models
{
    public class Ticket
    {
        #region Ids Parents
        public int Id { get; set; }

        [Display(Name = "Project")]
        public int ProjectId { get; set; }

        [Display(Name = "Ticket Type")]
        public int TicketTypeId { get; set; }

        [Display(Name = "Ticket Status")]
        public int TicketStatusId { get; set; }

        [Display(Name = "Ticket Priority")]
        public int TicketPriorityId { get; set; }
        
        [Display(Name = "Submitter")]
        public string SubmitterId { get; set; }

        [Display(Name = "Developer")]
        public string DeveloperId { get; set; }
        #endregion
        #region Description
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public bool IsArchived { get; set; }
        #endregion

        #region Navigation Children
        public virtual Project Project { get; set; }

        public virtual ApplicationUser Submitter { get; set; } 

        public virtual ApplicationUser Developer { get; set; }

        public virtual ApplicationUser Manager { get; set; }

        public virtual TicketStatus TicketStatus { get; set; }

        public virtual TicketPriority TicketPriority { get; set; }

        public virtual TicketType TicketType { get; set; }

        public virtual ICollection<TicketAttachment> Attachments { get; set; }

        public virtual ICollection<TicketComment> Comments { get; set; }

        public virtual ICollection<TicketHistory> Histories { get; set; }

        public virtual ICollection<TicketNotification> Notifications { get; set; }
        #endregion

        // Constructor
        public Ticket()
        {
            Attachments = new HashSet<TicketAttachment>();
            Comments = new HashSet<TicketComment>();
            Histories = new HashSet<TicketHistory>();
            Notifications = new HashSet<TicketNotification>();
        }


    }
}