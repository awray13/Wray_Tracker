using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Wray_Tracker.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        [NotMapped]
        public string FullName
        {
            get
            {
                // string interpelation
                return $"{LastName}, {FirstName}";
            }
        }

        public virtual ICollection<Project> Projects { get; set; }

        public virtual ICollection<TicketComment> Comments { get; set; }

        public virtual ICollection<TicketAttachment> Attachments { get; set; }

        public virtual ICollection<TicketHistory> TicketHistories { get; set; }

        public ApplicationUser()
        {
            Projects = new HashSet<Project>();
            Comments = new HashSet<TicketComment>();
            Attachments = new HashSet<TicketAttachment>();
            TicketHistories = new HashSet<TicketHistory>();
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Ticket> Tickets { get; set; }

        public DbSet<TicketComment> TicketComments { get; set; }

        public DbSet<TicketAttachment> TicketAttachments { get; set; }

        public DbSet<TicketHistory> TicketHistories { get; set; }

        public DbSet<TicketPriority> TicketPriorities { get; set; }

        public DbSet<TicketStatus> TicketStatus { get; set; }

        public DbSet<TicketType> TicketTypes { get; set; }

        public DbSet<TicketNotification> TicketNotifications { get; set; }

    }
}