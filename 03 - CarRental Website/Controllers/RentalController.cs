using SunshineRentals.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SunshineRentals.Controllers
{
    
    public class RentalController : Controller
    {
        private CarRentalEntities db = new CarRentalEntities();

        //
        // GET: /Rental/

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            //var itemList = new Array();
            //ViewBag.Manufactors = new Array({"",""});
            int userId = 0;
            string user = HttpContext.User.Identity.Name;
            using (UsersLogic logic = new UsersLogic())
            {
                userId = logic.GetUserId(user);
            }
            var rentals = db.Rentals.Include(r => r.Fleet);//.Where(r => r.UserId == userId);
            //WhoofBarbershop.RentalMetaData _rentals;
            //rentals
            //IQueryable<RentalViewModel> _rentals;
            //_rentals = (IQueryable<RentalViewModel>)rentals;

            //var RentalUsers = new List<_rentalUsers>();
            Dictionary<int, string> D = new Dictionary<int, string>();
            List<Tuple<int, string>> rentalUsers = new List<Tuple<int, string>>();
            foreach (var item in rentals)
            {
                var _userId = item.UserId;
                using (UsersLogic logic = new UsersLogic())
                {
                    var userName = logic.GetUserFullName(_userId);
                    rentalUsers.Add(new Tuple<int, string>(item.RentalId, userName.Split('=')[1].ToString()));
                    D.Add(item.RentalId, userName.Split('=')[1].ToString());
                }
            }
            ViewBag.RentalUsers = D;
            return View(rentals.ToList());
        }


        [Authorize(Roles = "Admin")]
        public ActionResult IndexByFleet(int id, string name)
        {
            ViewBag.LicencePlate = name;
            var rentals = db.Rentals.Include(r => r.Fleet).Where(r => r.FleetId == id);

            Dictionary<int, string> rentalUsers = new Dictionary<int, string>();
            foreach (var item in rentals)
            {
                var _userId = item.UserId;
                using (UsersLogic logic = new UsersLogic())
                {
                    var userName = logic.GetUserFullName(_userId);
                    rentalUsers.Add(item.RentalId, userName.Split('=')[1].ToString());
                }
            }
            ViewBag.RentalUsers = rentalUsers;
            return View(rentals.ToList());
        }


        [Authorize(Roles = "Manager")]
        public ActionResult CarReturn()
        {
            var rentals = db.Rentals.Include(r => r.Fleet);
            return View(rentals.ToList());
        }

        [Authorize(Roles = "Manager")]
        public ActionResult CarReturnDetails(int id = 0)
        {
            Rental rental = db.Rentals.Find(id);
            if (rental == null)
            {
                return HttpNotFound();
            }
            ViewBag.FleetId = new SelectList(db.Fleets, "FleetId", "LicencePlate", rental.FleetId);
            ViewBag.FleetList = new SelectList(db.Fleets.ToList(), "FleetId", "LicencePlate");
            return View(rental);
        }

        //
        // POST: /Rental/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CarReturnDetails(Rental rental)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rental).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CarReturn");
            }
            ViewBag.FleetId = new SelectList(db.Fleets, "FleetId", "LicencePlate", rental.FleetId);
            return View(rental);
        }

        //[Authorize]
        public ActionResult Search()
        {
            SearchViewModel model = new SearchViewModel();
            model.Year = 0;
            model.Transmission = -1;
            return View(model);
        }

        public ActionResult ReloadSearch(int Transmission, int Year, string MamufId, string Model, string FromDt, string ToDt, string Keywords)
        {
            SearchViewModel model = new SearchViewModel();
            //model.User = User;
            model.Transmission = Transmission;
            model.Year = Year;
            model.MamufId = MamufId;
            model.Model = Model;
            model.FromDt = FromDt;//.Substring(5, 2) + "/" + FromDt.Substring(8, 2) + "/" + FromDt.Substring(0, 4);
            model.ToDt = ToDt;
            model.Keywords = Keywords;

            return View("Search", model);
        }

        [Authorize]
        public ActionResult SearchHistory()
        {
            return View();
        }

        [Authorize]
        public ActionResult Orders()
        {
            return View();
        }

        [Authorize]
        public ActionResult History()
        {
            return View();
        }


        //
        // GET: /Rental/Details/5

        public ActionResult Details(int id = 0)
        {
            Rental rental = db.Rentals.Find(id);
            if (rental == null)
            {
                return HttpNotFound();
            }
            return View(rental);
        }

        //
        // GET: /Rental/Create

        public ActionResult Create()
        {
            ViewBag.FleetId = new SelectList(db.Fleets, "FleetId", "LicencePlate");
            return View();
        }

        //
        // POST: /Rental/Create

        [HttpPost]
        public ActionResult Create(Rental rental)
        {
            if (ModelState.IsValid)
            {
                db.Rentals.Add(rental);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FleetId = new SelectList(db.Fleets, "FleetId", "LicencePlate", rental.FleetId);
            return View(rental);
        }

        //
        // GET: /Rental/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Rental rental = db.Rentals.Find(id);
            if (rental == null)
            {
                return HttpNotFound();
                //throw new HttpException(404, "Not Found");
            }
            ViewBag.FleetId = new SelectList(db.Fleets, "FleetId", "LicencePlate", rental.FleetId);
            return View(rental);
        }

        //
        // POST: /Rental/Edit/5

        [HttpPost]
        public ActionResult Edit(Rental rental)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rental).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FleetId = new SelectList(db.Fleets, "FleetId", "LicencePlate", rental.FleetId);
            return View(rental);
        }

        //
        // GET: /Rental/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Rental rental = db.Rentals.Find(id);
            if (rental == null)
            {
                return HttpNotFound();
            }
            return View(rental);
        }

        //
        // POST: /Rental/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Rental rental = db.Rentals.Find(id);
            db.Rentals.Remove(rental);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //public ActionResult OrderForm()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult OrderForm(OrderViewModel orderViewModel)
        //{
        //    return View();
        //}

        [Authorize]
        public ActionResult OrderForm(int orderNum, int id, DateTime from, DateTime to)
        {

            OrderViewModel order = new OrderViewModel();
            order.DateFrom = from;
            order.DateTo = to;
            order.OrderNum = orderNum;

            var car = db.GetCarForRent(id).
                Select(c => new
                {
                    c.FleetId,
                    c.LicencePlate,
                    c.Year,
                    c.Auto,
                    c.Manufactor,
                    c.Model,
                    c.Kilometrage,
                    c.Image,
                    c.DailyRentalRate,
                    c.PenaltyDailyRate
                }).ToList();

            order.DayRate = car[0].DailyRentalRate;
            order.OverdueRate = car[0].PenaltyDailyRate;
            order.ImageUrl = car[0].Image;
            order.Manufactor = car[0].Manufactor;
            order.Model = car[0].Model;
            order.EstimatedCost = (decimal)(to - from).TotalDays * order.DayRate;
            order.LicencePlate = car[0].LicencePlate;
            order.FleetId = car[0].FleetId;

            return View(order);
        }

        [Authorize]
        [HttpPost]
        public ActionResult OrderForm(OrderViewModel order)
        {
            if (!ModelState.IsValid)
                return View();

            // אין שגיאת ולידציה - בצע רישום משתמש חדש
            using (OrderLogic logic = new OrderLogic())
                logic.WriteOrder(order.FleetId, order.UserId, order.DateFrom, order.DateTo);
                
 
            return RedirectToAction("Search", "Rental");
            //return View(order);
        }


        public JsonResult GetManufactorList()
        {
            using (CarRentalEntities db = new CarRentalEntities())
            {
                var ManufactorList = db.Manufactors.Select(c => new { c.ManufactorId, c.Name }).ToList();
                return Json(ManufactorList, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetModelByManufactorList(int ManId)
        {
            using (CarRentalEntities db = new CarRentalEntities())
            {
                var ModelList = db.Vehicles.Where(c => c.ManufactorId == ManId).Select(c => new { c.Model }).ToList();
                return Json(ModelList, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult SearchCars(int? Transmission, int? Year, int? MamufId, string Model, DateTime? From, DateTime? To, string Keywords)
        public JsonResult SearchCars(int? Transmission, int? Year, int? MamufId, string Model, string From, string To, string Keywords)
        {
            string model = "0";
            if (!string.IsNullOrEmpty(Model.ToString()))
                model = Model;
            //using (WhoofEntities db = new WhoofEntities())
            //{
            //WhoofEntities db = new WhoofEntities();
            var CarsTable = db.GetAvailableCars(Transmission, Year, MamufId, model, DateTime.Parse(From), DateTime.Parse(To), Keywords).
                Select(c => new
                {
                    c.FleetId,
                    c.LicencePlate,
                    c.Year,
                    c.Auto,
                    c.Manufactor,
                    c.Model,
                    c.Kilometrage,
                    c.Image,
                    c.DailyRentalRate,
                    c.PenaltyDailyRate
                });

            return Json(CarsTable, JsonRequestBehavior.AllowGet);
            //}
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult PlaceOrder(int? CarId, string User, string From, string To)
        {
            DateTime f = DateTime.Parse(From);
            DateTime t = DateTime.Parse(To);
            var rentals = db.Rentals.Include(r => r.Fleet).
                Where(r => r.FleetId == CarId).
                Where(r => r.StartRentalDate == f).
                Where(r => r.EndRentalDate == t);
            foreach (var item in rentals)
            {
                var _rentalId = item.RentalId;
                if (!string.IsNullOrEmpty(_rentalId.ToString()))
                {
                    var duplicate = -1;
                    return Json(duplicate, JsonRequestBehavior.AllowGet);
                }
            }

            var order = db.InsertOrder(CarId, User, DateTime.Parse(From), DateTime.Parse(To)).//.Select(r => new { r.RentalId });
            Select(c => new
                {
                    c.Value
                });

            return Json(order, JsonRequestBehavior.AllowGet);
            //}
        }
    }
}