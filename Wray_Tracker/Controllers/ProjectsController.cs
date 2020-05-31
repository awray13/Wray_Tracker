using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wray_Tracker.Helper;
using Wray_Tracker.Models;
using Wray_Tracker.ViewModels;

namespace Wray_Tracker.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserProjectHelper projHelper = new UserProjectHelper();
        private UserRoleHelper roleHelper = new UserRoleHelper();
        private AssignmentHelper assignHelper = new AssignmentHelper();
        private TicketHelper ticketHelper = new TicketHelper();

        [Authorize(Roles = "Admin, Manager")]
        public ActionResult AssignUsers(int projectId)
        {

            ViewBag.ProjectId = projectId;

            if (User.IsInRole("Admin"))
            {
                var pmId = db.Projects.FirstOrDefault(p => p.Id == projectId).ManagerId;
                ViewBag.ManagerId = new SelectList(roleHelper.UsersInRole("Manager"), "Id", "FullName", pmId); 
            }
            else
            {
                var subIds = assignHelper.UsersOnProjectInRole(projectId, "Submitter").Select(u => u.Id);
                ViewBag.SubmitterIds = new MultiSelectList(roleHelper.UsersInRole("Submitter"), "Id", "FullName", subIds); // has fourth parameter showing

                var devIds = assignHelper.UsersOnProjectInRole(projectId, "Developer").Select(u => u.Id);
                ViewBag.DeveloperIds = new MultiSelectList(roleHelper.UsersInRole("Developer"), "Id", "FullName", devIds); // has fourth parameter showing

            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult AssignUsers(int projectId, string managerId, List<string> submitterIds, List<string> developerIds)
        {
            if (User.IsInRole("Admin"))
            {
                // Dealing with PMs
                // Remove current PM and then add the selected PM
                var project = db.Projects.Find(projectId);
                project.ManagerId = managerId;
                db.SaveChanges();
            }
            else
            {
                // Dealing with Subs and Devs

                // Remove all Subs on Project
                foreach (var user in assignHelper.UsersOnProjectInRole(projectId, "Submitter"))
                {
                    projHelper.RemoveUserFromProject(user.Id, projectId);
                }

                // Add back all the selected Subs
                if (submitterIds != null)
                {
                    foreach(var submitterId in submitterIds)
                    {
                        projHelper.AddUserToProject(submitterId, projectId);
                    }
                }

                // Remove all Devs on Project
                foreach (var user in assignHelper.UsersOnProjectInRole(projectId, "Developer"))
                {
                    projHelper.RemoveUserFromProject(user.Id, projectId);
                }
                // Add back all the selected Devs
                if (developerIds != null)
                {
                    foreach (var developerId in developerIds)
                    {
                        projHelper.AddUserToProject(developerId, projectId);
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("Dashboard", "Home");
        }

        // Get: Manage Project Assignments
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult ManageProjectAssignments()
        {
            var emptyCustomUserDataList = new List<CustomUserData>();

            // I have decided that it should work this way...
            var users = db.Users.ToList();

            // Load up a Multi Select List of Users
            ViewBag.UserIds = new MultiSelectList(users, "Id", "FullName");

            // Load up a Multi Select List Of Projects
            ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name");

            // Load up the View Model
            foreach (var user in users)
            {
                emptyCustomUserDataList.Add(new CustomUserData
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    RoleName = roleHelper.ListUserRoles(user.Id).FirstOrDefault() ?? "Unassigned",
                    ProjectNames = projHelper.ListUserProjects(user.Id).Select(p => p.Name).ToList()
                });
            }

            return View(emptyCustomUserDataList);
        }

        // Post: Manage Project Assignments
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult ManageProjectAssignments(List<string> userIds, List<int> projectIds)
        {
            // If and only if I have chosen at least one person will I do the following operations...
            if(userIds == null || projectIds == null)
            {
                return RedirectToAction("ManageProjectAssignments");
            }

            // I can simply add each of the selected Users to each of the Selected Projects
            foreach(var userId in userIds)
            {
                foreach (var projectId in projectIds)
                {
                    projHelper.AddUserToProject(userId, projectId);
                }
            }
            return RedirectToAction("ManageProjectAssignments");


        }

        // GET: Manage Project Level Users Remove
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult ManageProjectLevelUsersRemove(int id)
        {
            var userIds = projHelper.UsersOnProject(id).Select(u => u.Id).ToList();

            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email", userIds);

            ViewBag.ProjectIds = new MultiSelectList(db.Projects, "Id", "Name", userIds);

            return View(db.Projects.Find(id));
        }

        // POST: Manage Project Level Users Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult ManageProjectLevelUsersRemove(List<string> userIds, int projectIds)
        {

            foreach (var memberId in userIds)
            {
                projHelper.RemoveUserFromProject(memberId, projectIds);
            }

            return RedirectToAction("ManageProjectAssignments");
        }

        // GET: Projects
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        [Authorize]
        public ActionResult ProjectDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ViewBag.PMName = db.Users.Find(project.ManagerId).FullName;

            Project project = db.Projects.Find(id);

            if (project == null)
            {
                return View("Error");
            }
            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Create()
        {
            ViewBag.ManagerId = new SelectList(roleHelper.UsersInRole("Manager"), "Id", "FullName");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Create([Bind(Include ="Id,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {

                //if (project.ManagerId == null)
                //{
                //    project.ManagerId = User.Identity.GetUserId();
                //}

                //project.ManagerId = User.Identity.GetUserId(); 
                project.Created = DateTime.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return View("Error");
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ManagerId,Created,Updated,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                project.Updated = DateTime.Now;
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin, Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
            db.SaveChanges();
            return RedirectToAction("Index");
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
