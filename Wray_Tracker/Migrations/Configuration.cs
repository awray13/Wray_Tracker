namespace Wray_Tracker.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Wray_Tracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Wray_Tracker.Helper;
    using System.Web.Configuration;

    internal sealed class Configuration : DbMigrationsConfiguration<Wray_Tracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Wray_Tracker.Models.ApplicationDbContext context)
        {
            #region Create roles
            var roleManager = new RoleManager<IdentityRole>(
                                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "Manager"))
            {
                roleManager.Create(new IdentityRole { Name = "Manager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }
            #endregion

            #region Add Users and Assign Roles

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var demoPassword = WebConfigurationManager.AppSettings["DemoPassword"];
            
            // Assigning Roles of the Admin and Manager
            if (!context.Users.Any(u => u.Email == "aricks1986@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "aricks1986@gmail.com",
                    Email = "aricks1986@gmail.com",
                    FirstName = "Ashton",
                    LastName = "Wray",
                    DisplayName = "Ashton"
                };

                // This line creates the User in the DB
                userManager.Create(user, "M@urice1");

                // This line attaches the Role of Admin to this specific user
                userManager.AddToRoles(user.Id, "Admin");
            }

                // Same thing goes for the Manager
                if (!context.Users.Any(u => u.Email == "JasonTwichell@coderfoundry.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "JasonTwichell@coderfoundry.com",
                    Email = "JasonTwichell@coderfoundry.com",
                    FirstName = "Jason",
                    LastName = "Twichell",
                    DisplayName = "Prof"
                };

                // This line creates the User in the DB
                userManager.Create(user, "Abc&123!");

                // This line attaches the Role of Manager to this specific user
                userManager.AddToRoles(user.Id, "Manager");
            }

            // And for now since I don't know if this right
            // This will be for the Developer
            if (!context.Users.Any(u => u.Email == "ARussell@coderfoundry.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "ARussell@coderfoundry.com",
                    Email = "ARussell@coderfoundry.com",
                    FirstName = "Andrew",
                    LastName = "Russell",
                    DisplayName = "Stache"
                };

                // This line creates the User in the DB
                userManager.Create(user, "Abc&123!");

                // This line attaches the Role of Developer to this specific user
                userManager.AddToRoles(user.Id, "Developer");
            }

            // Also for the Submitter
            if (!context.Users.Any(u => u.Email == "wraytesting@gmail.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "wraytesting@gmail.com",
                    Email = "wraytesting@gmail.com",
                    FirstName = "Ashton",
                    LastName = "Wray",
                    DisplayName = "Ash"
                };

                // This line creates the User in the DB
                userManager.Create(user, "M@urice1");

                // This line attaches the Role of Submitter to this specific user
                userManager.AddToRoles(user.Id, "Submitter");
            }

            // Demo Users
            if (!context.Users.Any(u => u.Email == "demoadmin@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "demoadmin@mailinator.com",
                    Email = "demoadmin@mailinator.com",
                    FirstName = "Bob",
                    LastName = "Tester",
                    DisplayName = "Bob",
                    EmailConfirmed = true
                };

                // This line creates the User in the DB
                userManager.Create(user, demoPassword);

                // This line attaches the Role of Submitter to this specific user
                userManager.AddToRoles(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.Email == "demopm@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "demopm@mailinator.com",
                    Email = "demopm@mailinator.com",
                    FirstName = "Dave",
                    LastName = "Tester",
                    DisplayName = "Dave",
                    EmailConfirmed = true
                };

                // This line creates the User in the DB
                userManager.Create(user, demoPassword);

                // This line attaches the Role of Submitter to this specific user
                userManager.AddToRoles(user.Id, "Manager");
            }

            if (!context.Users.Any(u => u.Email == "demodev@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "demodev@mailinator.com",
                    Email = "demodev@mailinator.com",
                    FirstName = "Mary",
                    LastName = "Tester",
                    DisplayName = "Mary",
                    EmailConfirmed = true
                };

                // This line creates the User in the DB
                userManager.Create(user, demoPassword);

                // This line attaches the Role of Submitter to this specific user
                userManager.AddToRoles(user.Id, "Developer");
            }

            if (!context.Users.Any(u => u.Email == "demosub@mailinator.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "demosub@mailinator.com",
                    Email = "demosub@mailinator.com",
                    FirstName = "Jamie",
                    LastName = "Tester",
                    DisplayName = "Jamie",
                    EmailConfirmed = true
                };

                // This line creates the User in the DB
                userManager.Create(user, demoPassword);

                // This line attaches the Role of Submitter to this specific user
                userManager.AddToRoles(user.Id, "Submitter");
            }
            #endregion


            #region Load up Ticket Types

            context.TicketTypes.AddOrUpdate(
                t => t.Name,
                    new TicketType { Name = "Defect" },
                    new TicketType { Name = "Software"},
                    new TicketType { Name = "Hardware" },
                    new TicketType { Name = "UI" },
                    new TicketType { Name = "Other" }

                );


            #endregion

            #region Load up Ticket Priorities

            context.TicketPriorities.AddOrUpdate(
                t => t.Name,
                    new TicketPriority { Name = "Immediate" },
                    new TicketPriority { Name = "High" },
                    new TicketPriority { Name = "Medium" },
                    new TicketPriority { Name = "Low" },
                    new TicketPriority { Name = "On Hold" }
                );


            #endregion

            #region Load up Ticket Status

            context.TicketStatus.AddOrUpdate(
                t => t.Name,
                    new TicketStatus { Name = "New" },
                    new TicketStatus { Name = "Assigned"},
                    new TicketStatus { Name = "Resolved" },
                    new TicketStatus { Name = "Reopened" },
                    new TicketStatus { Name = "Archived" }
                );


            #endregion

            #region Seed a Demo Project

            context.Projects.AddOrUpdate(
                t => t.Name,
                new Project { 
                    Name = "Seeded Project", 
                    Description = "Seeded project to make sure to always have a Project in place ", 
                    Created = DateTime.Now
                });

            context.SaveChanges();
            #endregion

            #region Seed a Demo Ticket
            // Store the newly created Project in a variable in case we need properties from it
            var seededProjectId = context.Projects.FirstOrDefault(p => p.Name == "Seeded Project").Id;

            // I have made the decision that this Seeded Ticket will be a Type of 'Defect'
            var seededTicketTypeId = context.TicketTypes.FirstOrDefault(t => t.Name == "Defect").Id;

            // I have decided that this weeded Ticket will be an Immediate priority
            var seededTicketPriorityId = context.TicketPriorities.FirstOrDefault(t => t.Name == "Immediate").Id;

            var seededTicketStatusId = context.TicketStatus.FirstOrDefault(t => t.Name == "New").Id;

            var seededSubmitterId = context.Users.FirstOrDefault(u => u.Email == "wraytesting@gmail.com").Id;

            // I have to associate this submitter with the project
            var projHelper = new UserProjectHelper();
            projHelper.AddUserToProject(seededSubmitterId, seededProjectId);


            context.Tickets.AddOrUpdate(
                t => t.Title,
                new Ticket() { 
                    Title = "Seeded Ticket 1",
                    Description = "A well formed description of some defect",
                    Created = DateTime.Now,
                    ProjectId = seededProjectId,
                    TicketTypeId = seededTicketTypeId,
                    TicketPriorityId = seededTicketPriorityId,
                    TicketStatusId = seededTicketStatusId,
                    SubmitterId = seededSubmitterId
                });

            #endregion
            

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
