using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Wray_Tracker.Helper;
using Wray_Tracker.Models;
using Wray_Tracker.ViewModels;

namespace Wray_Tracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserProjectHelper projHelper = new UserProjectHelper();
        private TicketHelper ticketHelper = new TicketHelper();
        private HistoryHelper historyHelper = new HistoryHelper();
        private HistoryDisplayHelper historyDisplayHelper = new HistoryDisplayHelper();
        private NotificationHelper notificationHelper = new NotificationHelper();
        private RecordManager recordManager = new RecordManager();

        [Authorize]
        public ActionResult Dashboard(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return View("Error");
            }
            return View(ticket);
        }

        // GET: Tickets
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Index()
        {
            //var tickets = db.Tickets.Include(t => t.Developer).Include(t => t.Project).Include(t => t.Submitter);
            var ticketIndexVMs = new List<TicketIndexVM>();

            var allTickets = db.Tickets.ToList();
            foreach (var ticket in allTickets)
            {
                ticketIndexVMs.Add(new TicketIndexVM
                {
                    Ticket = ticket,
                    TicketStatus = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId)
                });
            }

            return View(ticketIndexVMs);
            //return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        public ActionResult TicketDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create(int? projectId, Ticket ticket)
        {
           
            // I need to somehow produce a list of only my Projects and then put that list into the SelectList
            var myUserId = User.Identity.GetUserId();
            var myProjects = projHelper.ListUserProjects(myUserId);


            if (projectId == null)
            {
                ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            }


            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");

            var newTicket = new Ticket();
            if (projectId != null)
            {
                newTicket.ProjectId = (int)projectId;
            }



            return View(newTicket);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create([Bind(Include = "Id,ProjectId,TicketTypeId,TicketPriorityId,Title,Description")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.SubmitterId = User.Identity.GetUserId();
                ticket.TicketStatusId = db.TicketStatus.FirstOrDefault(t => t.Name == "New").Id;
                ticket.Created = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Dashboard", "Home");
            }
            // I need to somehow produce a list of only my Projects and then put that list into the SelectList
            var myUserId = User.Identity.GetUserId();
            var myProjects = projHelper.ListUserProjects(myUserId);

            if (ticket.ProjectId == 0)
            {
                ViewBag.ProjectId = new SelectList(myProjects, "Id", "Name");
            }

            ViewBag.DeveloperId = new SelectList(ticketHelper.AssignableDevelopers(ticket.ProjectId), "Id", "FullName", ticket.DeveloperId);
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Developer, Submitter, Manager, Admin")]

        // Custom Action Filter that checks before you allow user in to edit

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ticket ticket = db.Tickets.Find(id);

            var currentUserId = User.Identity.GetUserId();

            // I need some additional, more granular security to determine whether this is my 
            // "this is my ticket" depends on your role
            // If I am a Developer or Submitter:
            var authorized = true;
            if ((User.IsInRole("Developer") && ticket.DeveloperId != currentUserId) ||
                (User.IsInRole("Submitter") && ticket.SubmitterId != currentUserId))
            {
                authorized = false;

            }

            if (!authorized)
            {
                TempData["UnAuthorizedTicketAccess"] = $"You are not authorized to Edit Ticket {id}";
                return RedirectToAction("Dashboard", "Home");
            }

            // If I am a Project Manager
            if (User.IsInRole("Manager"))
            {
                var myTickets = db.Projects.Where(p => p.ManagerId == currentUserId).SelectMany(p => p.Tickets).Select(t => t.Id).ToList();

            }

            if (ticket == null)
            {
                return HttpNotFound();
            }


            ViewBag.DeveloperId = new SelectList(ticketHelper.AssignableDevelopers(ticket.ProjectId), "Id", "FullName", ticket.DeveloperId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProjectId,TicketTypeId,TicketStatusId,TicketPriorityId,SubmitterId,DeveloperId,Title,Description,Created,")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                if (User.IsInRole("Developer") && userId == ticket.DeveloperId || User.IsInRole("Admin") || User.IsInRole("Manager") && user.Projects.Any(p => p.Id == ticket.ProjectId) || User.IsInRole("Submitter") && userId == ticket.SubmitterId)
                {
                    // I want to use AsNoTracking() to get a Momento Ticket object
                    // clean disconnected view
                    var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                    ticket.Updated = DateTime.Now;
                    db.Entry(ticket).State = EntityState.Modified;
                    db.SaveChanges();

                    var newTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                    // Holds and maintains historyHelper and notificationHelper
                    await recordManager.ManageChangeRecords(oldTicket, newTicket);

                }
                else
                {
                    return View("Error");
                }


                return RedirectToAction("Index", "TicketHistories");
            }

            ViewBag.DeveloperId = new SelectList(ticketHelper.AssignableDevelopers(ticket.ProjectId), "Id", "FullName", ticket.DeveloperId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.SubmitterId = new SelectList(db.Users, "Id", "FirstName", ticket.SubmitterId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            return View(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
