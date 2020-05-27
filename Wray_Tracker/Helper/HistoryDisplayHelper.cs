using Microsoft.Owin.Security.Provider;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Wray_Tracker.Models;

namespace Wray_Tracker.Helper
{
    public class HistoryDisplayHelper
    {
        public static string DisplayData(TicketHistory ticketHistory)
        {
            var db = new ApplicationDbContext();

            var data = "";

            data = db.Users.FirstOrDefault(u => u.Id == ticketHistory.UserId).FullName;

            return data;

        }   
    }
}

