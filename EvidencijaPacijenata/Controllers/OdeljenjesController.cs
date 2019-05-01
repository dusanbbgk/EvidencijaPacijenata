using EvidencijaPacijenata.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class OdeljenjesController : Controller
    {
        private DBZUstanovaBetaEntities db = new DBZUstanovaBetaEntities();

        // GET: Odeljenjes
        public ActionResult Index()
        {
            var odeljenjes = db.Odeljenjes.Include(o => o.Ustanova);
            return View(odeljenjes.ToList());
        }

        // GET: Odeljenjes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Odeljenje odeljenje = db.Odeljenjes.Find(id);
            if (odeljenje == null)
            {
                return HttpNotFound();
            }
            return View(odeljenje);
        }

        // GET: Odeljenjes/Create
        public ActionResult Create()
        {
            if (Session["IDAdmina"] != null)
            {
                ViewBag.IDUstanove = new SelectList(db.Ustanovas, "ID", "Naziv");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Odeljenjes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDUstanove,Naziv,SlobodnihMesta")] Odeljenje odeljenje)
        {
            if (ModelState.IsValid)
            {
                db.Odeljenjes.Add(odeljenje);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDUstanove = new SelectList(db.Ustanovas, "ID", "Naziv", odeljenje.IDUstanove);
            return View(odeljenje);
        }

        // GET: Odeljenjes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["IDAdmina"] != null)
            {
                Odeljenje odeljenje = db.Odeljenjes.Find(id);
                if (odeljenje == null)
                {
                    return HttpNotFound();
                }
                ViewBag.IDUstanove = new SelectList(db.Ustanovas, "ID", "Naziv", odeljenje.IDUstanove);
                return View(odeljenje);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Odeljenjes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDUstanove,Naziv,SlobodnihMesta")] Odeljenje odeljenje)
        {
            if (ModelState.IsValid)
            {
                db.Entry(odeljenje).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUstanove = new SelectList(db.Ustanovas, "ID", "Naziv", odeljenje.IDUstanove);
            return View(odeljenje);
        }

        // GET: Odeljenjes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["IDAdmina"] != null)
            {
                Odeljenje odeljenje = db.Odeljenjes.Find(id);
                if (odeljenje == null)
                {
                    return HttpNotFound();
                }
                return View(odeljenje);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Odeljenjes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Odeljenje odeljenje = db.Odeljenjes.Find(id);
            db.Odeljenjes.Remove(odeljenje);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
