using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wray_Tracker.Models;

namespace Wray_Tracker.Helper
{
    public class CustomComparer
    {
        public class TicketComparer : IEqualityComparer<ApplicationUser>
        {
            public bool Equals(ApplicationUser user1, ApplicationUser user2)
            {
                if (object.ReferenceEquals(user1, user2))
                {
                    return true;
                }
                if (user1 == null || user2 == null)
                {
                    return false;
                }

                return user1.Id == user2.Id;
            }

            public int GetHashCode(ApplicationUser item)
            {
                return item.Id.GetHashCode();
            }

        }
    }
}