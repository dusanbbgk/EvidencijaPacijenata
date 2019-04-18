using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EvidencijaPacijenata.Models;

namespace EvidencijaPacijenata.Controllers
{
    public class LekarOpstePraksesController : Controller
    {
        private DBZUstanovaEntities db = new DBZUstanovaEntities();

        // GET: LekarOpstePrakses
        public ActionResult Index()
        {
            return View(db.Korisniks.ToList());
        }

        // GET: LekarOpstePrakses/Details/5
        public ActionResult Details(int? id)
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

        // GET: LekarOpstePrakses/Create
        public ActionResult Create()
        {
            return View();
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
            LekarOpstePrakse lekarOpstePrakse = db.Korisniks.OfType<LekarOpstePrakse>().SingleOrDefault(l => l.ID == id);
            if (lekarOpstePrakse == null)
            {
                return HttpNotFound();
            }
            return View(lekarOpstePrakse);
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
