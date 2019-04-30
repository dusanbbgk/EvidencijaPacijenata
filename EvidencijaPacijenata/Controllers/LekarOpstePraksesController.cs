using EvidencijaPacijenata.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class LekarOpstePraksesController : Controller
    {
        private DBZUstanovaEntities db = new DBZUstanovaEntities();

        // GET: LekarOpstePrakses
        public ActionResult Index()
        {
            if (Session["IDAdmina"] != null)
            {
                return View(db.Korisniks.OfType<LekarOpstePrakse>().ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: LekarOpstePrakses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["IDAdmina"] != null || id == Convert.ToInt32(Session["IDLekara"]))
            {
                LekarOpstePrakse lekarOpstePrakse = db.Korisniks.OfType<LekarOpstePrakse>().SingleOrDefault(l => l.ID == id);
                if (lekarOpstePrakse == null)
                {
                    return HttpNotFound();
                }
                return View(lekarOpstePrakse);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: LekarOpstePrakses/Create
        public ActionResult Create()
        {
            if (Session["IDAdmina"] != null)
            {
                ViewBag.IDUstanove = new SelectList(db.Ustanovas.ToList(), "ID", "Naziv");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: LekarOpstePrakses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Ime,Prezime,KorisnickoIme,Lozinka,IDOdeljenja,Licenca")] LekarOpstePrakse lekarOpstePrakse)
        {
            if (ModelState.IsValid)
            {
                db.Korisniks.Add(lekarOpstePrakse);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lekarOpstePrakse);
        }

        // GET: LekarOpstePrakses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["IDAdmina"] != null || id == Convert.ToInt32(Session["IDLekara"]))
            {
                LekarOpstePrakse lekarOpstePrakse = db.Korisniks.OfType<LekarOpstePrakse>().SingleOrDefault(l => l.ID == id);
                if (lekarOpstePrakse == null)
                {
                    return HttpNotFound();
                }
                return View(lekarOpstePrakse);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: LekarOpstePrakses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Ime,Prezime,KorisnickoIme,Lozinka,IDOdeljenja,Licenca")] LekarOpstePrakse lekarOpstePrakse)
        {
            if (ModelState.IsValid)
            {
                ModelState.Remove("Lozinka");
                ModelState.Remove("IDOdeljenja");
                db.Entry(lekarOpstePrakse).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lekarOpstePrakse);
        }

        // GET: LekarOpstePrakses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LekarOpstePrakse lekarOpstePrakse = db.Korisniks.OfType<LekarOpstePrakse>().SingleOrDefault(l => l.ID == id);
            if (lekarOpstePrakse == null)
            {
                return HttpNotFound();
            }
            return View(lekarOpstePrakse);
        }

        // POST: LekarOpstePrakses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LekarOpstePrakse lekarOpstePrakse = db.Korisniks.OfType<LekarOpstePrakse>().SingleOrDefault(l => l.ID == id);
            db.Korisniks.Remove(lekarOpstePrakse);
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
