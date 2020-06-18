using MVCAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCAssignment.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {      
                return View();
        }

        [HttpPost]
        public ActionResult Login(Login model)
        {
            using (var context = new UserEntities())
            {
                bool isValid = context.Users.Any(x => x.UserName == model.UserName && x.Password == model.Password);
                if (isValid)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    return RedirectToAction("Index", "Employees");
                }

                ModelState.AddModelError("", "Invalid username and password");
                return View();
            }
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(SignUp model)
        {
            User obj = new User();
            obj.UserName = model.UserName;
            obj.Password = model.Password;
            obj.Email = model.Email;
            using (var context = new UserEntities())
            {
                context.Users.Add(obj);
                context.SaveChanges();
            }
            return RedirectToAction("login");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        public ActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Forgot(SignUp model)
        {
            var context = new UserEntities();
            int portNumber = 587;
            bool enableSSL = true;
            string emailFromAddress = "padhariyaar@gmail.com"; //Sender Email Address  
            string emailPassword = "Ajay#9586"; //Sender Password  
            string smtpAddress = "smtp.gmail.com";
            string emailToAddress = model.Email; //Receiver Email Address  
                var password = context.Users.Where(x => x.Email == model.Email).FirstOrDefault();
                if (password == null)
                {
                       ModelState.AddModelError("", "You Enter Wrong Email");
            }
            string subject = "Hello " + password.UserName;
            string body = "Email : " + emailToAddress + "\n"
                            + "password : " + password.Password;
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(emailToAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, emailPassword);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }
            return RedirectToAction("login");
        }
    }
}