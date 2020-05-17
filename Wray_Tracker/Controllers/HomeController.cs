using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
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
        private UserRoleHelper roleHelper = new UserRoleHelper();

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

        // GET: User Profile
        [HttpGet]
        public ActionResult EditProfile()
        {
            // Store the Primary Key of the User in the currentUserId variable
            var currentUserId = User.Identity.GetUserId();

            // The currentUser variable represents the entire User record with something like 20 columns...I don't need all of these
            var currentUser = db.Users.Find(currentUserId);
            var userProfileVM = new UserProfileVM();
            userProfileVM.Id = currentUser.Id;
            userProfileVM.FirstName = currentUser.FirstName;
            userProfileVM.LastName = currentUser.LastName;
            userProfileVM.DisplayName = currentUser.DisplayName;
            userProfileVM.MyProjects = projHelper.ListUserProjects(currentUserId).ToList();
            userProfileVM.MyTickets = ticketHelper.ListMyTickets(currentUserId).ToList();
            userProfileVM.Email = currentUser.Email;


            return View(userProfileVM);
        }

        // POST: User Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(UserProfileVM model)
        {
            // I want to get a tracked User record based on the incoming model.Id
            var currentUser = db.Users.Find(model.Id);
            currentUser.FirstName = model.FirstName;
            currentUser.LastName = model.LastName;
            currentUser.DisplayName = model.DisplayName;
            currentUser.Email = model.Email;
            currentUser.UserName = model.Email;

            db.SaveChanges();

            TempData["EditProfileMessage"] = $"Your data has been changed successfully";
            return RedirectToAction("About");
        }


        public ActionResult About()
        {
            // Store the Primary Key of the User in the currentUserId variable
            var currentUserId = User.Identity.GetUserId();

            // The currentUser variable represents the entire User record with something like 20 columns...I don't need all of these
            var currentUser = db.Users.Find(currentUserId);
            var userProfileVM = new UserProfileVM();
            userProfileVM.Id = currentUser.Id;
            userProfileVM.FirstName = currentUser.FirstName;
            userProfileVM.LastName = currentUser.LastName;
            userProfileVM.DisplayName = currentUser.DisplayName;
            userProfileVM.MyProjects = projHelper.ListUserProjects(currentUserId).ToList();
            userProfileVM.MyTickets = ticketHelper.ListMyTickets(currentUserId).ToList();
            userProfileVM.Email = currentUser.Email;


            return View(userProfileVM);
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