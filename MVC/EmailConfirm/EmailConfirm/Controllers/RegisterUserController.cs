using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Security;
using System.Web.Mvc;
using EmailConfirm.Models;

namespace EmailConfirm.Controllers
{
    public class RegisterUserController : Controller
    {
        NotMarketPlaceEntities _context = new NotMarketPlaceEntities();
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login login)
        {
                if (_context.Registers.Any(x => x.EmailId == login.Email && x.Password == login.Password))
                {
                    var result = _context.Registers.Where(x => x.EmailId == login.Email).FirstOrDefault();
                    if (result.Code == "True")
                    {
                        FormsAuthentication.SetAuthCookie(login.Email, false);
                        return RedirectToAction("SearchNotes", "Notes");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Complete Your Email Verification");
                    }
                }
                else if (login.Email == "Admin" && login.Password == "Admin")
                {
                    FormsAuthentication.SetAuthCookie(login.Email, false);
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password");
                }
            
            return View();
        }
        [HttpPost]
        public ActionResult Register(Register model)
        {
            Guid Code = Guid.NewGuid();
            model.Code = "False";
            _context.Registers.Add(model);
            _context.SaveChanges();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("nishivpatel40@gmail.com");
            mail.To.Add(model.EmailId);
            mail.Subject = "confirmation Email";
            mail.IsBodyHtml = true;
            string content = "Hello, <br>" + model.FirstName +" " + model.LastName+ " <br><br>";
            content += "Regards, <br>" + model.FirstName;
            
            Guid activationCode = Guid.NewGuid();
            
            string body = "Hello " + model.FirstName + ",";
            body += "<br /><br />Thank you for signing up with us. Please click on below link to verify your email address and to do login.";
            body += "<br /><a href = '" + string.Format("{0}://{1}/RegisterUser/Activation/{2}", Request.Url.Scheme, Request.Url.Authority,model.Id) + "'>Click here to activate your account.</a>";
           
            body += "<br/><br/>Regards,<br/>NoteMarketPlace";
            mail.Body = body;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            NetworkCredential nc = new NetworkCredential("nishivpatel40@gmail.com", "nishu450");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = nc;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(mail);
            ViewBag.Message = "Mail Sent";
            return View("Login");
        }

        public ActionResult Activation(int id) {
            var result = _context.Registers.Where(x => x.Id == id).FirstOrDefault();
            result.Code = "True";
            _context.Entry(result).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();
            return View("Login");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult UserProfile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserProfile(UserProfile model1)
        {
            var name = User.Identity.Name;
            model1.User_Id = 1020;
            var currenttime = DateTime.UtcNow;
            model1.Created_Date = currenttime;
            model1.Modified_Date = currenttime;
            model1.Created_By = name;
            model1.Modified_By = name;
            model1.User_email = name;
            _context.UserProfiles.Add(model1);
            _context.SaveChanges();
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(RegDetail model)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("nishivpatel40@gmail.com");
            mail.To.Add(model.EmailId);
            mail.Subject = "New Temporary Password has been created for you";
            mail.IsBodyHtml = true;
            var password = Membership.GeneratePassword(8, 1);
            string content = "Hello,<br><br>We have generated a new password for you<br>Password: " + password + " <br><br>";
            content += "Regards, <br>NoteMarketPlace";

            mail.Body = content;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            NetworkCredential nc = new NetworkCredential("nishivpatel40@gmail.com", "nishu450");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = nc;
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(mail);
            TempData["testmsg"] = " Requested Successfully ";
            return View("Login");
        }
    }
}