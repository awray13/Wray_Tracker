using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using Wray_Tracker.Models;
using Wray_Tracker.ViewModels;

namespace Wray_Tracker.Controllers
{
    
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
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