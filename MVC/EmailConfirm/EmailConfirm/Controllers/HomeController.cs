using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Demo.Models;
using EmailConfirm.Models;

namespace EmailConfirm.Controllers
{
    public class HomeController : Controller
    {
        private NotMarketPlaceEntities db = new NotMarketPlaceEntities(); 
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FAQPage()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Contact(contactus contact)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("nishivpatel40@gmail.com");
            mail.To.Add(contact.EmailId);
            mail.Subject = contact.subject;
            mail.IsBodyHtml = true;
            string content = "Hello, <br>" + contact.Comments + " <br><br>";
            content += "Regards, <br>" + contact.Name;
            mail.Body = content;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            NetworkCredential nc = new NetworkCredential("nishivpatel40@gmail.com", "nishu450");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = nc;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(mail);

            ViewBag.Message = "Mail Sent";

            return View();
        }
    }
}