using Microsoft.AspNet.Identity;
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

    public class UserHelper
    {
        private ApplicationDbContext Db = new ApplicationDbContext();
        private string UserId { get; set; }

        public UserHelper()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId();
        }

        public string GetUserDisplayName()
        {
            return Db.Users.Find(UserId).DisplayName;
        }

        public string GetUserAvatar()
        {
            return Db.Users.Find(UserId).AvatarPath;
        }
    }
}
        