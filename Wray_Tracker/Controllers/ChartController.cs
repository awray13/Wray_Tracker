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
            colorList.Add("#00a65a");
            colorList.Add("#f39c12");
            colorList.Add("#00c0ef");
            colorList.Add("#3c8dbc");
            colorList.Add("#d2d6de");
            colorList.Add("#03fcf4");
            colorList.Add("#886896");

            var rand = new Random();

            var pieChartVM = new PieChartVM();
            var priorities = db.TicketPriorities.ToList();

            var dataKey = 0;

            pieChartVM.Data.Insert(dataKey, "Tickets");

            foreach (var priority in priorities)
            {
                dataKey++;

                var count = db.Tickets.Where(t => t.TicketPriorityId == priority.Id).Count();
                pieChartVM.Data.Insert(dataKey, count.ToString());
                pieChartVM.Labels.Add(priority.Name);
                pieChartVM.Colors.Add(colorList[rand.Next(0, colorList.Count)]);
            }

            return Json(pieChartVM);
        }

        //POST: Ticket Status Chart
        public JsonResult GetTicketStatusChartData()
        {
            var colorList = new List<string>();
            colorList.Add("#f56954");
            colorList.Add("#00a65a");
            colorList.Add("#f39c12");
            colorList.Add("#00c0ef");
            colorList.Add("#3c8dbc");
            colorList.Add("#d2d6de");
            colorList.Add("#03fcf4");
            colorList.Add("#886896");

            var rand = new Random();

            var pieChartVM = new PieChartVM();
            var statuses = db.TicketStatus.ToList();

            var dataKey = 0;

            pieChartVM.Data.Insert(dataKey, "Tickets");

            foreach (var status in statuses)
            {
                dataKey++;

                var count = db.Tickets.Where(t => t.TicketStatusId == status.Id).Count();
                pieChartVM.Data.Insert(dataKey, count.ToString());
                pieChartVM.Labels.Add(status.Name);
                pieChartVM.Colors.Add(colorList[rand.Next(0, colorList.Count)]);
            }

            return Json(pieChartVM);
        }

        //POST: Ticket Type Chart
        public JsonResult GetTicketTypeChartData()
        {
            var colorList = new List<string>();
            colorList.Add("#f56954");
            colorList.Add("#00a65a");
            colorList.Add("#f39c12");
            colorList.Add("#00c0ef");
            colorList.Add("#3c8dbc");
            colorList.Add("#d2d6de");
            colorList.Add("#03fcf4");
            colorList.Add("#886896");

            var rand = new Random();

            var pieChartVM = new PieChartVM();
            var types = db.TicketTypes.ToList();

            var dataKey = 0;

            pieChartVM.Data.Insert(dataKey, "Tickets");

            foreach (var type in types)
            {
                dataKey++;

                var count = db.Tickets.Where(t => t.TicketTypeId == type.Id).Count();
                pieChartVM.Data.Insert(dataKey, count.ToString());
                pieChartVM.Labels.Add(type.Name);
                pieChartVM.Colors.Add(colorList[rand.Next(0, colorList.Count)]);
            }

            return Json(pieChartVM);
        }
    }
}