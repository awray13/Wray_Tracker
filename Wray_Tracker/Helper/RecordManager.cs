using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wray_Tracker.Models;

namespace Wray_Tracker.Helper
{
    public class RecordManager
    {
        private HistoryHelper historyHelper = new HistoryHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();

        public void ManageChangeRecords(Ticket oldTicket, Ticket newTicket)
        {
            historyHelper.ManageHistoryRecordCreation(oldTicket, newTicket);
            notificationHelper.ManageNotifications(oldTicket, newTicket);
        }
    }
}