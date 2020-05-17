using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Wray_Tracker.Helper;
using Wray_Tracker.Models;
using Wray_Tracker.ViewModels;

namespace Wray_Tracker.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserProjectHelper projHelper = new UserProjectHelper();
        private TicketHelper ticketHelper = new TicketHelper();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            var myUserId = User.Identity.GetUserId();
            var assigned = new DashboardVM();
            

            assigned.MyProjects = projHelper.ListUserProjects(myUserId).ToList();
            assigned.MyTickets = ticketHelper.ListMyTickets(myUserId).ToList();


            return View(assigned);
            
        }

        public ActionResult About()
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

        public ActionResult Contact()
        {


            ViewBag.Message = "Your contact page.";

            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ContactAsync(EmailModel model)
        {
            try
            {

                var emailAddress = WebConfigurationManager.AppSettings["emailto"];

                var emailFrom = ($"{model.From} <{emailAddress} >");

                var finalBody = $"{model.Name} has sent you the following message <br /> {model.Body} <hr /> {WebConfigurationManager.AppSettings["LegalText"]}";

                var email = new MailMessage(emailFrom, emailAddress)
                {
                    Subject = model.Subject,
                    Body = finalBody,
                    IsBodyHtml = true
                };

                var emailSvc = new EmailService();
                await emailSvc.SendAsync(email);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Task.FromResult(0);
            }
            return View(new EmailModel());
        }
    }
}