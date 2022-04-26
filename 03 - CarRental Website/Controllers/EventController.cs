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
    public class EventController : Controller
    {
        private CarRentalEntities db = new CarRentalEntities();

        //
        // GET: /Event/

        //[Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            //var itemList = new Array();
            //ViewBag.Manufactors = new Array({ "", "" });
            int userId = 0;
            string user = HttpContext.User.Identity.Name;
            bool isUserAdmin = false;
            using (UsersLogic logic = new UsersLogic())
            {
                userId = logic.GetUserId(user);
                isUserAdmin = (logic.IsUserOccupied(user) == "Admin");
            }
            IOrderedQueryable<Event> events = null;
            if (isUserAdmin)
            {
                events = db.Events.OrderBy(e => e.Start);
            }
            else
            {
                events = db.Events.Where(e => e.UserId == userId).OrderBy(e => e.Start);
            }
            //WhoofBarbershop.RentalMetaData _rentals;
            //rentals
            //IQueryable<RentalViewModel> _rentals;
            //_rentals = (IQueryable<RentalViewModel>)rentals;

            //var RentalUsers = new List<_rentalUsers>();
            Dictionary<int, string> EventTypes = new Dictionary<int, string>();
            //List<Tuple<int, string>> rentalUsers = new List<Tuple<int, string>>();
            foreach (var item in events)
            {
                var _eventTypeId = Int32.Parse(item.Description);
                var _eventType = db.EventExtentions.Where(x => x.EventExtentionId == _eventTypeId).Select(x => x.Ext_String_1).FirstOrDefault().ToString();

                EventTypes.Add(item.EventID, _eventType);
                //EventTypes.Add(item.EventID, item.Description);
                //using (UsersLogic logic = new UsersLogic())
                //{
                //    var userName = logic.GetUserFullName(_userId);
                //    //rentalUsers.Add(new Tuple<int, string>(item.RentalId, userName.Split('=')[1].ToString()));
                //    //D.Add(item.RentalId, userName.Split('=')[1].ToString());
                //}
            }
            ViewBag.EventTypes = EventTypes;
            ViewBag.CurrUserId = userId;
            return View(events.ToList());
            //return View();
        }

        //
        // GET: /Event/Create

        public ActionResult Create()
        {
            //Event events = db.Events.First(); //db.Events.First();
            EventExtention eventExt = db.EventExtentions.First();

            //if (events == null)
            //{
            //    return HttpNotFound();
            //}
            var mydog = new List<ConvertEnum>();
            foreach (dogBreed dog in Enum.GetValues(typeof(dogBreed)))
                mydog.Add(new ConvertEnum
                {
                    Value = (int)dog,
                    Text = dog.ToString().Replace("_", " ")
                });
            ViewBag.MyDogsEnum = mydog;

            ViewBag.MyTimeFrame = new SelectList(db.EventExtentions.Where(e => e.Type == "AppointmentSlot"), "EventExtentionId", "Ext_String_1", eventExt.EventExtentionId);
            ViewBag.MyEventTypes = new SelectList(db.EventExtentions.Where(e => e.Type == "Treatment"), "EventExtentionId", "Ext_String_1", eventExt.EventExtentionId);

            //ViewBag.TypeId = new SelectList(db.Vehicles, "TypeId", "Model");
            ViewBag.ErrorMessage = "";
            return View();
        }

        //
        // POST: /Event/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Event events)
        {
            EventExtention eventExt = db.EventExtentions.First();
            if (ModelState.IsValid)
            {
                bool FreeSlot = true;
                using (UsersLogic logic = new UsersLogic())
                {
                    events.UserId = logic.GetUserId(HttpContext.User.Identity.Name);
                }

                var askedStartTime = db.EventExtentions.Where(x => x.EventExtentionId == events.DurationInMin).Select(x => x.Ext_String_1).FirstOrDefault().ToString();
                var askedDate = events.Start.ToString("MM/dd/yyyy");
                events.Start = DateTime.Parse(string.Concat(askedDate, " ", askedStartTime, ":00"));

                using (EventLogic logic = new EventLogic())
                {
                    FreeSlot = logic.AskForFreeSlot(Int32.Parse(events.Description), events.Start);
                    //var results = logic.AskForFreeSlot(Int32.Parse(events.Description), events.Start).FirstOrDefault();
                    ////foreach (var item in results)
                    ////{
                    ////    var x = ((ResultsForFreeSlot)item).SlotAvaiable;
                    ////}
                    //var singleValue = results;//.FirstOrDefault();

                    //if (singleValue == null)
                    //{
                    //    FreeSlot = false;
                    //}
                    //else
                    //{
                    //    if (singleValue.SlotAvaiable == 0)
                    //    {
                    //        FreeSlot = false;
                    //    }
                    //}

                }

                int evExtId = 0;
                Int32.TryParse(events.Description, out evExtId);
                //var description = db.EventExtentions.Where(x => x.EventExtentionId == evExtId).Select(x => x.Ext_String_1).FirstOrDefault().ToString();
                var duration =  db.EventExtentions.Where(x => x.EventExtentionId == evExtId).Select(x => x.Ext_Numeric_1).FirstOrDefault().ToString();
                //events.Description = description;
                decimal d;
                Decimal.TryParse(duration, out d);
                d = 60 * d;
                events.DurationInMin = Convert.ToInt32(d);
                if (!FreeSlot)
                {
                    //EventExtention eventExt = db.EventExtentions.First();
                    ViewBag.ErrorMessage = "The time slot requested is occupied. Try new time slot.";
                    ViewBag.MyTimeFrame = new SelectList(db.EventExtentions.Where(e => e.Type == "AppointmentSlot"), "EventExtentionId", "Ext_String_1", eventExt.EventExtentionId);
                    ViewBag.MyEventTypes = new SelectList(db.EventExtentions.Where(e => e.Type == "Treatment"), "EventExtentionId", "Ext_String_1", eventExt.EventExtentionId);
                    //ViewBag.TypeId = new SelectList(db.Vehicles, "TypeId", "Model", fleet.TypeId);
                    return View(events);
                }

                db.Events.Add(events);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.MyTimeFrame = new SelectList(db.EventExtentions.Where(e => e.Type == "AppointmentSlot"), "EventExtentionId", "Ext_String_1", eventExt.EventExtentionId);
            //ViewBag.MyEventTypes = new SelectList(db.EventExtentions.Where(e => e.Type == "Treatment"), "EventExtentionId", "Ext_String_1", eventExt.EventExtentionId);
            return View();
        }


        //
        // GET: /Event/Delete/5
        public ActionResult Delete(int id = 0)
        {
            Event evnt = db.Events.Find(id);
            if (evnt == null)
            {
                return HttpNotFound();
            }
            return View(evnt);
        }
        
        //
        // POST: /Event/Delete/5

        [HttpPost, ActionName("Delete")]
        //[Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewBag.ErrorMessage = "";
            Event evnt = db.Events.Find(id);
            //using (UsersLogic logic = new UsersLogic())
            //{
            //    string ret = logic.IsUserOccupied(user.UserName);
            //    //return View(user);
            //}

            db.Events.Remove(evnt);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /Event/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Event events = db.Events.Find(id);
            EventExtention eventExt = db.EventExtentions.First();
            if (events == null)
            {
                return HttpNotFound();
            }
            ViewBag.ErrorMessage = "";
            ViewBag.MyEventTypes = new SelectList(db.EventExtentions.Where(e => e.Type == "Treatment"), "EventExtentionId", "Ext_String_1", eventExt.EventExtentionId);
            ViewBag.MyTimeFrame = new SelectList(db.EventExtentions.Where(e => e.Type == "AppointmentSlot"), "EventExtentionId", "Ext_String_1", eventExt.EventExtentionId);
            return View(events);
        }

        //
        // POST: /Fleet/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Event events)
        {
            if (ModelState.IsValid)
            {
                using (UsersLogic logic = new UsersLogic())
                {
                    events.UserId = logic.GetUserId(HttpContext.User.Identity.Name);
                }
                int evExtId = 0;
                Int32.TryParse(events.Description, out evExtId);
                var duration = db.EventExtentions.Where(x => x.EventExtentionId == evExtId).Select(x => x.Ext_Numeric_1).FirstOrDefault().ToString();
                decimal d;
                Decimal.TryParse(duration, out d);
                d = 60 * d;
                events.DurationInMin = Convert.ToInt32(d);
                //using (RentalsLogic logic = new RentalsLogic())
                //{
                //    FleetId = logic.IsCarInFleet(events.TypeId, events.LicencePlate);
                //}

                //if (FleetId != 0 && FleetId != events.FleetId)
                //{
                //    ViewBag.ErrorMessage = "Car already exsist in fleet.";
                //    ViewBag.TypeId = new SelectList(db.Vehicles, "TypeId", "Model", events.TypeId);
                //    return View(events);
                //}

                db.Entry(events).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.TypeId = new SelectList(db.Vehicles, "TypeId", "Model", events.TypeId);
            EventExtention eventExt = db.EventExtentions.First();
            ViewBag.MyTimeFrame = new SelectList(db.EventExtentions.Where(e => e.Type == "AppointmentSlot"), "EventExtentionId", "Ext_String_1", eventExt.EventExtentionId);
            ViewBag.MyEventTypes = new SelectList(db.EventExtentions.Where(e => e.Type == "Treatment"), "EventExtentionId", "Ext_String_1", eventExt.EventExtentionId);
            return View(events);
        }

        public JsonResult GetEvents()
        {
            using (CarRentalEntities dc = new CarRentalEntities())
            {
                var events = dc.Events.ToList();
                int userId = 0;
                using (UsersLogic logic = new UsersLogic())
                {
                    userId = logic.GetUserId(HttpContext.User.Identity.Name);
                }

                for (int i = 0; i < events.Count; i++)
                {
                    //events[i].UserId == userId ? events[i].ThemeColor = "" : events[i].ThemeColor = "";
                    if (events[i].UserId == userId)
                    { 
                        events[i].ThemeColor = ""; 
                    }
                    else
                    {
                        events[i].ThemeColor = "#fc0f03";
                    }
                }
            
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

    }
}
