using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wray_Tracker.Models;

namespace Wray_Tracker.ViewModels
{
    public class TicketIndexVM
    {
        public Ticket Ticket { get; set; }

        public IEnumerable<SelectListItem> TicketStatus { get; set; }

        public int TicketStatusId { get; set; }
    }
}