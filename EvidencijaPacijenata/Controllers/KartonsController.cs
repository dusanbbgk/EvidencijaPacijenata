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
    public class KartonsController : Controller
    {
        private DBZUstanovaEntities db = new DBZUstanovaEntities();

        // GET: Kartons
        public ActionResult Index()
        {
            var kartons = db.Kartons.Include(k => k.Korisnik).Include(k => k.Korisnik1);
            return View(kartons.ToList());
        }

        // GET: Kartons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Karton karton = db.Kartons.SingleOrDefault(k => k.IDPacijenta == id);
            if (karton == null)
            {
                return HttpNotFound();
            }
            return View(karton);
        }

        // GET: Kartons/Create
        public ActionResult Create()
        {
            ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime");
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime");
            return View();
        }

        // POST: Kartons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDPacijenta,IDLekara,DatumVremeNalaza,Disanje,Puls,TelesnaTemperatura,KrvniPritisak,Mokraca,Stolica,KrvaSlika,SpecificniPregled")] Karton karton)
        {
            if (ModelState.IsValid)
            {
                db.Kartons.Add(karton);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime", karton.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime", karton.IDPacijenta);
            return View(karton);
        }

        // GET: Kartons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Karton karton = db.Kartons.SingleOrDefault(k => k.IDPacijenta == id);
            if (karton == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime", karton.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime", karton.IDPacijenta);
            return View(karton);
        }

        // POST: Kartons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDPacijenta,IDLekara,DatumVremeNalaza,Disanje,Puls,TelesnaTemperatura,KrvniPritisak,Mokraca,Stolica,KrvaSlika,SpecificniPregled")] Karton karton)
        {
            if (ModelState.IsValid)
            {
                db.Entry(karton).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime", karton.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime", karton.IDPacijenta);
            return View(karton);
        }

        // GET: Kartons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Karton karton = db.Kartons.SingleOrDefault(k => k.IDPacijenta == id);
            if (karton == null)
            {
                return HttpNotFound();
            }
            return View(karton);
        }

        // POST: Kartons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Karton karton = db.Kartons.SingleOrDefault(k => k.IDPacijenta == id);
            db.Kartons.Remove(karton);
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
