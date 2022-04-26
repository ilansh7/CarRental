using SunshineRentals.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using System.Linq;

namespace SunshineRentals.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private CarRentalEntities db = new CarRentalEntities();

        //
        // GET: /Home/

        public ActionResult Index()
        {
            //return View(db.Users.ToList());
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Index1()
        {
            ViewBag.ErrorMessage = "";
            return View(db.Users.OrderBy(u => u.UserName).ToList());
        }
        
        //
        //// GET: /Home/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    User user = db.Users.Find(id);
        //    if (user == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(user);
        //}

        ////
        //// GET: /Home/Create

        //[Authorize(Roles = "Admin")]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        ////
        //// POST: /Home/Create

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public ActionResult Create(User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Users.Add(user);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(user);
        //}

        //
        // GET: /Home/Edit/5

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ErrorMessage = "";
            return View(user);
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index1");
            }
            ViewBag.ErrorMessage = "";
            return View(user);
        }

        //
        // GET: /Home/Delete/5

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id = 0)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.ErrorMessage = "";
            return View(user);
        }

        //
        // POST: /Home/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.ErrorMessage = "";
            User user = db.Users.Find(id);
            using (UsersLogic logic = new UsersLogic())
            {
                string ret = logic.IsUserOccupied(user.UserName);
                return View(user);
            }
            
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index1");
        }

        public ActionResult Error404()
        {
            return View();
        }

        public ActionResult ContactPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ContactPage(ContactPageModel model)
        {
            // בדיקת ולידציה - אם יש שגיאה, הצג את אותו הדף
            if (!ModelState.IsValid)
                return View();

            MailMessage msg2Send = new MailMessage();
            //msg2Send.From = new MailAddress("91229.02@gmail.com", "Sunshine Rentals");
            msg2Send.From = new MailAddress("ilan.shchori@gmail.com", "Sunshine Rentals");
            msg2Send.To.Add(model.Email);
            msg2Send.IsBodyHtml = true;
            msg2Send.SubjectEncoding = Encoding.UTF8;
            msg2Send.BodyEncoding = Encoding.UTF8;
            msg2Send.Subject = "About your request from Sunshine Rentals";
            msg2Send.Body = "<div style=\"direction: ltr;\">Dear " + model.UserName + ".<br/>Thank you for your interest.<br/>Our representative will contact with you shortly.<br/>Sincerely, Sunshine Rentals.</div>";

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            //smtpClient.Credentials = new NetworkCredential("91229.02@gmail.com", "037100741");
            smtpClient.Credentials = new NetworkCredential("ilan.shchori@gmail.com", "********");

            try
            {
                smtpClient.Send(msg2Send);
                model.SuccessSend = 1;
                ViewBag.SendResponse = "";
                //model.Error = ".";
                //lblMessage.Text = "The email was succesfully sent.";
                //lblMessage.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                model.SuccessSend = 0;
                model.Error = ex.Message;
                ViewBag.SendResponse = ex.Message;
                //lblMessage.ForeColor = Color.Red;
            }

            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}