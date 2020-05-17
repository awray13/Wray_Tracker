using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wray_Tracker.Helper;
using Wray_Tracker.Models;
using Wray_Tracker.ViewModels;

namespace Wray_Tracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRoleHelper userRoleHelper = new UserRoleHelper();

        // GET: Admin
        public ActionResult ManageRoles()
        {
            var viewData = new List<CustomUserData>();

            var users = db.Users.ToList();

            foreach (var user in users)
            {
                var newUserData = new CustomUserData();

                newUserData.FirstName = user.FirstName;
                newUserData.LastName = user.LastName;
                newUserData.Email = user.Email;
                newUserData.RoleName = userRoleHelper.ListUserRoles(user.Id).FirstOrDefault() ?? "Unassigned";

                viewData.Add(newUserData);

            }

            // Right hand side control: This data will be used to power a Dropdown List in the View
            ViewBag.RoleName = new SelectList(db.Roles, "Name", "Name");

            // Left hand side control: This data will be used to power ListBox in the View
            ViewBag.UserIds = new MultiSelectList(db.Users, "Id", "Email");

            return View(viewData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageRoles(List<string> userIds, string roleName)
        {
            // Use our RoleHelper to actually  assign the role to the person or persons selected
            if (userIds != null)
            {
                foreach (var userId in userIds)
                {
                    // First I need to determine what Role if any the user is in
                    var userRole = userRoleHelper.ListUserRoles(userId).FirstOrDefault();

                    if (!string.IsNullOrEmpty(userRole))
                    {
                        // Second I have to remove each of the selected users from their current Role
                        userRoleHelper.RemoveUserFromRole(userId, userRole);
                    }

                    if (!string.IsNullOrEmpty(roleName))
                    {
                        // Then I will add each selected user to the selected Role
                        userRoleHelper.AddUserToRole(userId, roleName);
                    }
                }
            }

            return RedirectToAction("ManageRoles");
        }

    }

}