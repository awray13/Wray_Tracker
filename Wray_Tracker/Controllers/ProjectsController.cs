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
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserProjectHelper projHelper = new UserProjectHelper();

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
                    ProjectNames = projHelper.ListUserProjects(user.Id).Select(p => p.Name).ToList()
                });
            }

            return View(emptyCustomUserDataList);
        }

        // Post: Manage Project Assignments
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        // Get: ManageProjectLevelUsers
        public ActionResult ManageProjectLevelUsers(int id)
        {
            var userIds = projHelper.UsersOnProject(id).Select(u => u.Id).ToList();
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email", userIds);
            return View(db.Projects.Find(id));
        }

        // Post: ManageProjectLevelUsers
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageProjectLevelUsers(List<string> userIds, int projectId)
        {
            var projMemeberIds = projHelper.UsersOnProject(projectId).Select(u => u.Id).ToList();
            foreach (var memeberId in projMemeberIds)
            {
                projHelper.RemoveUserFromProject(memeberId, projectId);
            }

            if (userIds != null)
            {
                foreach (var userId in userIds)
                {
                    projHelper.AddUserToProject(userId, projectId);
                }

            }

            return RedirectToAction("ManageProjectLevelUsers", new { id = projectId });

        }

        // GET: Projects
        public ActionResult Index()
        {
            return View(db.Projects.ToList());
        }

        // GET: Projects/Details/5
        public ActionResult Details(int? id)
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

        // GET: Projects/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,ManagerId,Created,Updated,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                
                project.Created = DateTime.Now;
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(project);
        }

        // GET: Projects/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,ManagerId,Created,Updated,IsArchived")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: Projects/Delete/5
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
