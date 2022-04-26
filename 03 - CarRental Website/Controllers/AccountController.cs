using SunshineRentals.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SunshineRentals.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            // בדיקת ולידציה - אם יש שגיאה, הצג את אותו הדף
            if (!ModelState.IsValid)
                return View();

            // אין שגיאת ולידציה - בצע רישום משתמש חדש
            using (UsersLogic logic = new UsersLogic())
                logic.Register(registerViewModel.IdNumber, registerViewModel.FirstName, registerViewModel.LastName,
                                registerViewModel.UserName, registerViewModel.Password, registerViewModel.eMail);
            //Register(string id, string fname, string lname, string username, string password,  string email)
            // Logged-In-הגדר את המשתמש כ
            FormsAuthentication.SetAuthCookie(registerViewModel.UserName, false);

            return RedirectToAction("Index", "Account");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ErrorMessage = "Missing Username / Password";
                return View();
            }

            using (UsersLogic logic = new UsersLogic())
            {
                string url1 = HttpContext.Request.RawUrl;
                bool isUserExist = logic.IsUserExist(loginViewModel.UserName, loginViewModel.Password);
                if (isUserExist)
                {
                    //if (string.IsNullOrEmpty(ViewBag.CurrentUrl))
                        FormsAuthentication.RedirectFromLoginPage(loginViewModel.UserName, loginViewModel.RememberMe); // גלישה מיידית לדף שהפנה לכאן
                    //else
                    //{
                    //    string url = ViewBag.CurrentUrl;
                    //    ViewBag.CurrentUrl = string.Empty;
                    //    RedirectToAction(url);
                    //}
                }
                ViewBag.ErrorMessage = "Incorrect Username or Password";
                ViewBag.CurrentUrl = string.Empty;
                return View();
            }
        }

        [Authorize]
        public ActionResult Logout()
        {
            ViewBag.CurrentUrl = HttpContext.Request.RawUrl;
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public JsonResult GetUserId(string name)
        {
            using (CarRentalEntities db = new CarRentalEntities())
            {
                //var userName = db.Users.Any(u => u.UserName == name).Select(c => c. }).ToList();
                var userId = db.Users.Where(u => u.UserName == name).Select(s => new { s.UserId });
                return Json(userId, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
