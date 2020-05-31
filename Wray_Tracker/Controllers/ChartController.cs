using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wray_Tracker.Models;
using Wray_Tracker.ViewModels;

namespace Wray_Tracker.Controllers
{
    public class ChartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //POST: Ticket Priority Chart
        public JsonResult GetTicketPriorityChartData()
        {
            var colorList = new List<string>();
            colorList.Add("#f56954");
            colorList.Add("#8d3efa");
            colorList.Add("#f39c12");
            colorList.Add("#fa3e47");
            colorList.Add("#3c8dbc");
            colorList.Add("#d2d6de");
            colorList.Add("#03fcf4");
            colorList.Add("#886896");

            var rand = new Random();

            var pieChartVM = new PieChartVM();
            var priorities = db.TicketPriorities.ToList();

            foreach (var priority in priorities)
            {

                var count = db.Tickets.Where(t => t.TicketPriorityId == priority.Id).Count();
                count++;

                pieChartVM.Data.Add(count);
                pieChartVM.Labels.Add(priority.Name);
                pieChartVM.Colors.Add(colorList[rand.Next(0, colorList.Count)]);
            }

            return Json(pieChartVM);
        }

        //POST: Ticket Status Chart
        public JsonResult GetTicketStatusChartData()
        {
            var colorList = new List<string>();
            colorList.Add("#3bcc3e");
            colorList.Add("#3b42cc");
            colorList.Add("#f743b2");
            colorList.Add("#9741f2");
            colorList.Add("#3c8dbc");
            colorList.Add("#d2d6de");
            colorList.Add("#4287f5");
            colorList.Add("#e6f542");

            var rand = new Random();

            var pieChartVM = new PieChartVM();
            var statuses = db.TicketStatus.ToList();

            var dataKey = 0;

            pieChartVM.Data.Add(dataKey);

            foreach (var status in statuses)
            {

                var count = db.Tickets.Where(t => t.TicketStatusId == status.Id).Count();
                count++;

                pieChartVM.Data.Add(count);
                pieChartVM.Labels.Add(status.Name);
                pieChartVM.Colors.Add(colorList[rand.Next(0, colorList.Count)]);
            }

            return Json(pieChartVM);
        }

        //POST: Ticket Type Chart
        public JsonResult GetTicketTypeChartData()
        {
            var colorList = new List<string>();
            colorList.Add("#f2453f");
            colorList.Add("#f7c23b");
            colorList.Add("#bde851");
            colorList.Add("#fca63d");
            colorList.Add("#4ee2f2");
            colorList.Add("#4070f5");
            colorList.Add("#b43bff");
            colorList.Add("#fc3d79");

            var rand = new Random();

            var pieChartVM = new PieChartVM();
            var types = db.TicketTypes.ToList();


            foreach (var type in types)
            {

                var count = db.Tickets.Where(t => t.TicketTypeId == type.Id).Count();
                count++;

                pieChartVM.Data.Add(count);
                pieChartVM.Labels.Add(type.Name);
                pieChartVM.Colors.Add(colorList[rand.Next(0, colorList.Count)]);
            }

            return Json(pieChartVM);
        }
    }
}