using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wray_Tracker.Models;

namespace Wray_Tracker.Helper
{
    public class AssignmentHelper
    {
        private UserRoleHelper rolesHelper = new UserRoleHelper();
        private UserProjectHelper projHelper = new UserProjectHelper();

        public ICollection<ApplicationUser> UsersOnProjectInRole(int projectId, string roleName)
        {
            var users = new List<ApplicationUser>();

            var projUsers = projHelper.UsersOnProject(projectId);
            foreach (var user in projUsers)
            {
                if (rolesHelper.ListUserRoles(user.Id).FirstOrDefault() == roleName)
                {
                    users.Add(user);
                }
            }

            return users;
        }
    }
}