using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace SunshineRentals.Controllers
{
    [Authorize(Roles = "Admin")]
    public class VehicleController : Controller
    {
        private CarRentalEntities db = new CarRentalEntities();

        //
        // GET: /Vehicle/

        public ActionResult Index()
        {
            var vehicles = db.Vehicles.Include(v => v.Manufactor).OrderBy(v => v.Manufactor.Name).OrderBy(v => v.Model);
            return View(vehicles.ToList());
        }

        //
        // GET: /Vehicle/Details/5

        //public ActionResult Details(int id = 0)
        //{
        //    Vehicle vehicle = db.Vehicles.Find(id);
        //    if (vehicle == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(vehicle);
        //}

        //
        // GET: /Vehicle/Create

        public ActionResult Create()
        {
            ViewBag.ManufactorId = new SelectList(db.Manufactors, "ManufactorId", "Name");
            ViewBag.ErrorMessage = "";
            return View();
        }

        //
        // POST: /Vehicle/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Vehicle vehicle)
        {
            int VehicleId = 0;
            using (RentalsLogic logic = new RentalsLogic())
            {
                VehicleId = logic.IsVehicleExsist(vehicle.ManufactorId, vehicle.Model, vehicle.Year, vehicle.Transmission);
            }

            if (VehicleId > 0)
            {
                ViewBag.ErrorMessage = "This type of Vehicle already exsist.";
                ViewBag.ManufactorId = new SelectList(db.Manufactors, "ManufactorId", "Name", vehicle.ManufactorId);
                return View(vehicle);
            }

            if (ModelState.IsValid)
            {
                db.Vehicles.Add(vehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManufactorId = new SelectList(db.Manufactors, "ManufactorId", "Name", vehicle.ManufactorId);
            return View(vehicle);
        }

        //
        // GET: /Vehicle/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ViewBag.ErrorMessage = "";
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.ManufactorId = new SelectList(db.Manufactors, "ManufactorId", "Name", vehicle.ManufactorId);
            return View(vehicle);
        }

        //
        // POST: /Vehicle/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Vehicle vehicle)
        {
            if (ModelState.IsValid)
            {
                int VehicleId = 0;
                using (RentalsLogic logic = new RentalsLogic())
                {
                    VehicleId = logic.IsVehicleExsist(vehicle.ManufactorId, vehicle.Model, vehicle.Year, vehicle.Transmission);
                }

                if (VehicleId != 0 && VehicleId != vehicle.TypeId)
                {
                    ViewBag.ErrorMessage = "This type of Vehicle already exsist.";
                    ViewBag.ManufactorId = new SelectList(db.Manufactors, "ManufactorId", "Name", vehicle.ManufactorId);
                    return View(vehicle);
                }

                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManufactorId = new SelectList(db.Manufactors, "ManufactorId", "Name", vehicle.ManufactorId);
            return View(vehicle);
        }

        //
        // GET: /Vehicle/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            if (vehicle == null)
            {
                return HttpNotFound();
            }
            return View(vehicle);
        }

        //
        // POST: /Vehicle/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vehicle vehicle = db.Vehicles.Find(id);
            db.Vehicles.Remove(vehicle);
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