using StokTakip.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace StokTakip.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        StokTakipDBEntities1 db = new StokTakipDBEntities1();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogIn(User p)
        {
            var info = db.User.FirstOrDefault(x=>x.Mail==p.Mail && x.Password==p.Password);
            if (info!=null)
            {
                FormsAuthentication.SetAuthCookie(info.Mail, false);
                Session["Mail"] = info.Mail.ToString();
                Session["Name"] = info.Name.ToString();
                Session["Surname"] = info.Surname.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.error = "EMail ya da Şifre Hatalı";
            }
            return View();
        }
        //public ActionResult User(int id)
        //{
        //    var user = db.User.Where(u => u.Id == id).ToList();
        //    return View(user);
        //}
        public ActionResult User()
        {
            var mail = (string)Session["Mail"];
            var info = db.User.FirstOrDefault(u => u.Mail == mail);
            return View(info);
        }
        [HttpPost]
        public ActionResult User(User userData)
        {
            //var mail = (string)Session["Mail"];
            //var user = db.User.Where(u => u.Mail == mail).FirstOrDefault();
            //user.Name = userData.Name;
            //user.Surname = userData.Surname;
            //user.Mail = userData.Mail;
            //user.Password = userData.Password;

            db.Entry(userData).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return View(userData);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("LogIn", "Account");
        }
        [HttpGet]
        public ActionResult PasswordReset()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PasswordReset(User user)
        {
            var email = db.User.Where(x => x.Mail == user.Mail).FirstOrDefault();
            if(email!=null)
            {
                Random rndm = new Random();
                int newPassword = rndm.Next(11111,99999);
                email.Password = Convert.ToString(newPassword);
                db.SaveChanges();
                //WebMail.SmtpServer = "smtp.gmail.com";
                //WebMail.EnableSsl = true;
                //WebMail.UserName = "zeynebpbp@hotmail.com";
                //WebMail.Password = "zeyneb123";
                //WebMail.SmtpPort = 587;
                //WebMail.Send(user.Mail,"ŞİFRE YENİLEME",("Yeni Şifreniz: " + newPassword.ToString()));

                SmtpClient client = new SmtpClient("smtp.gmail.com",587);
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("zeynebbilgetay@gmail.com", "12345");

                MailMessage message = new MailMessage();
                message.From = new MailAddress("zeynebbilgetay@gmail.com", "ŞİFRE YENİLEME");
                message.To.Add(user.Mail);
                message.IsBodyHtml = true;
                message.Subject = "Şifre Yenileme";
                message.Body += "Merhaba" + user.Name + "<br/> Yeni Şifreniz: " + email.Password;
                //NetworkCredential network = new NetworkCredential("zeynebbilgetay@gmail.com", "12345");
                //client.Credentials = network;
                client.Send(message);

                ViewBag.warning = "Yeni Şifreniz Mailinize Gönderilmiştir!!!";
            }
            else
            {
                ViewBag.warning = "Tekrar deneyiniz!!!";
            }
            return View();
        }
    }
}