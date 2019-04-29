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
    public class LekarSpecijalistasController : Controller
    {
        private DBZUstanovaEntities db = new DBZUstanovaEntities();

        // GET: LekarSpecijalistas
        public ActionResult Index()
        {
            return View(db.Korisniks.OfType<LekarSpecijalista>().ToList());
        }

        // GET: LekarSpecijalistas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LekarSpecijalista lekarSpecijalista = db.Korisniks.OfType<LekarSpecijalista>().SingleOrDefault(l => l.ID == id);
            if (lekarSpecijalista == null)
            {
                return HttpNotFound();
            }
            return View(lekarSpecijalista);
        }

        // GET: LekarSpecijalistas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LekarSpecijalistas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Ime,Prezime,KorisnickoIme,Lozinka,IDOdeljenja,Licenca,Specijalizacija")] LekarSpecijalista lekarSpecijalista)
        {
            if (ModelState.IsValid)
            {
                db.Korisniks.Add(lekarSpecijalista);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lekarSpecijalista);
        }

        // GET: LekarSpecijalistas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LekarSpecijalista lekarSpecijalista = db.Korisniks.OfType<LekarSpecijalista>().SingleOrDefault(l => l.ID == id);
            if (lekarSpecijalista == null)
            {
                return HttpNotFound();
            }
            return View(lekarSpecijalista);
        }

        // POST: LekarSpecijalistas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Ime,Prezime,KorisnickoIme,Lozinka,IDOdeljenja,Licenca,Specijalizacija")] LekarSpecijalista lekarSpecijalista)
        {
            if (ModelState.IsValid)
            {
                //ModelState.Remove("Lozinka");
                db.Entry(lekarSpecijalista).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lekarSpecijalista);
        }

        // GET: LekarSpecijalistas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LekarSpecijalista lekarSpecijalista = db.Korisniks.OfType<LekarSpecijalista>().SingleOrDefault(l => l.ID == id);
            if (lekarSpecijalista == null)
            {
                return HttpNotFound();
            }
            return View(lekarSpecijalista);
        }

        // POST: LekarSpecijalistas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LekarSpecijalista lekarSpecijalista = db.Korisniks.OfType<LekarSpecijalista>().SingleOrDefault(l => l.ID == id);
            db.Korisniks.Remove(lekarSpecijalista);
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
