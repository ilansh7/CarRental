using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace SunshineRentals.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManufactorController : Controller
    {
        private CarRentalEntities db = new CarRentalEntities();

        //
        // GET: /Manufactor/

        
        public ActionResult Index()
        {
            return View(db.Manufactors.OrderBy(m => m.Name).ToList());
        }

        //
        // GET: /Manufactor/Details/5

        //[Authorize(Roles = "Manager")]
        //public ActionResult Details(int id = 0)
        //{
        //    Manufactor manufactor = db.Manufactors.Find(id);
        //    if (manufactor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(manufactor);
        //}

        //
        // GET: /Manufactor/Create

        public ActionResult Create()
        {
            ViewBag.ErrorMessage = "";
            return View();
        }

        //
        // POST: /Manufactor/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Manufactor manufactor)
        {
            bool isManufactorExist = false;
            using (RentalsLogic logic = new RentalsLogic())
            {
                isManufactorExist = logic.IsManufactorExsist(manufactor.Name);
            }

            if (isManufactorExist)
            {
                ViewBag.ErrorMessage = "Manufactor already exsist.";
                return View(manufactor);
            }

            if (ModelState.IsValid)
            {
                db.Manufactors.Add(manufactor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manufactor);
        }

        //
        // GET: /Manufactor/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ViewBag.ErrorMessage = "";
            Manufactor manufactor = db.Manufactors.Find(id);
            if (manufactor == null)
            {
                //throw new HttpException(404, "Not Found");
                return HttpNotFound();
            }
            return View(manufactor);
        }

        //
        // POST: /Manufactor/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Manufactor manufactor)
        {
            int ManufactorId = 0;
            using (RentalsLogic logic = new RentalsLogic())
            {
                ManufactorId = logic.GetManufactorId(manufactor.Name);
            }

            if (ManufactorId != manufactor.ManufactorId)
            {
                ViewBag.ErrorMessage = "Manufactor Name already exsist.";
                return View(manufactor);
            }

            if (ModelState.IsValid)
            {
                db.Entry(manufactor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manufactor);
        }

        //
        // GET: /Manufactor/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Manufactor manufactor = db.Manufactors.Find(id);
            if (manufactor == null)
            {
                return HttpNotFound();
            }
            return View(manufactor);
        }

        //
        // POST: /Manufactor/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Manufactor manufactor = db.Manufactors.Find(id);
            db.Manufactors.Remove(manufactor);
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