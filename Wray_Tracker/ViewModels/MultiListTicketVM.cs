using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wray_Tracker.Models;

namespace Wray_Tracker.ViewModels
{
    public class MultiListTicketVM
    {
        public List<Ticket> ProjectTickets { get; set; }

        public List<Ticket> MyTickets { get; set; }

        public List<Ticket> AllTickets { get; set; }

    }
}