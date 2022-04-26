using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace SunshineRentals.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FleetController : Controller
    {
        private CarRentalEntities db = new CarRentalEntities();

        //
        // GET: /Fleet/

        public ActionResult Index()
        {
            ViewBag.ErrorMessage = "";
            var fleets = db.Fleets.Include(f => f.Vehicle);
            return View(fleets.ToList());
        }

        //
        // GET: /Fleet/Create

        public ActionResult Create()
        {
            ViewBag.TypeId = new SelectList(db.Vehicles, "TypeId", "Model");
            ViewBag.ErrorMessage = "";
            return View();
        }

        //
        // POST: /Fleet/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fleet fleet)
        {
            if (ModelState.IsValid)
            {
                int FleetId = 0;
                using (RentalsLogic logic = new RentalsLogic())
                {
                    FleetId = logic.IsCarInFleet(fleet.TypeId, fleet.LicencePlate);
                }

                if (FleetId > 0)
                {
                    ViewBag.ErrorMessage = "Car already exsist in fleet.";
                    ViewBag.TypeId = new SelectList(db.Vehicles, "TypeId", "Model", fleet.TypeId);
                    return View(fleet);
                }

                db.Fleets.Add(fleet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TypeId = new SelectList(db.Vehicles, "TypeId", "Model", fleet.TypeId);
            return View(fleet);
        }

        //
        // GET: /Fleet/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Fleet fleet = db.Fleets.Find(id);
            if (fleet == null)
            {
                return HttpNotFound();
            }
            ViewBag.ErrorMessage = "";
            ViewBag.TypeId = new SelectList(db.Vehicles, "TypeId", "Model", fleet.TypeId);
            return View(fleet);
        }

        //
        // POST: /Fleet/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Fleet fleet)
        {
            if (ModelState.IsValid)
            {
                int FleetId = 0;
                using (RentalsLogic logic = new RentalsLogic())
                {
                    FleetId = logic.IsCarInFleet(fleet.TypeId, fleet.LicencePlate);
                }

                if (FleetId != 0 && FleetId != fleet.FleetId)
                {
                    ViewBag.ErrorMessage = "Car already exsist in fleet.";
                    ViewBag.TypeId = new SelectList(db.Vehicles, "TypeId", "Model", fleet.TypeId);
                    return View(fleet);
                }

                db.Entry(fleet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TypeId = new SelectList(db.Vehicles, "TypeId", "Model", fleet.TypeId);
            return View(fleet);
        }

        //
        // GET: /Fleet/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Fleet fleet = db.Fleets.Find(id);
            if (fleet == null)
            {
                return HttpNotFound();
            }
            return View(fleet);
        }

        //
        // POST: /Fleet/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Fleet fleet = db.Fleets.Find(id);
            db.Fleets.Remove(fleet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}