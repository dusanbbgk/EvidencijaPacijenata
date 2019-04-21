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
    public class UputsController : Controller
    {
        private DBZUstanovaEntities db = new DBZUstanovaEntities();

        // GET: Uputs
        public ActionResult Index()
        {
            var uputs = db.Uputs.Include(u => u.Korisnik).Include(u => u.Korisnik1).Include(u => u.Odeljenje).Include(u => u.Korisnik2);
            return View(uputs.ToList());
        }

        // GET: Uputs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uput uput = db.Uputs.Find(id);
            if (uput == null)
            {
                return HttpNotFound();
            }
            return View(uput);
        }

        // GET: Uputs/Create
        public ActionResult Create()
        {
            //ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime");
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime");
            ViewBag.IDOdeljenja = new SelectList(db.Odeljenjes, "ID", "Naziv");
            ViewBag.IDLekaraKome = new SelectList(db.Korisniks, "ID", "Ime");
            ViewBag.IDLekaraOd = new SelectList(db.Korisniks, "ID", "Ime");
            return View();
        }

        // POST: Uputs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDPacijenta,IDLekara,DatumPregleda,IDOdeljenja,ZavrsenPregled,IDLekaraOd,IDLekaraKome")] Uput uput)
        {
            if (ModelState.IsValid)
            {
                db.Uputs.Add(uput);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime", uput.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime", uput.IDPacijenta);
            ViewBag.IDOdeljenja = new SelectList(db.Odeljenjes, "ID", "Naziv", uput.IDOdeljenja);
            ViewBag.IDLekaraKome = new SelectList(db.Korisniks, "ID", "Ime", uput.IDLekaraKome);
            ViewBag.IDLekaraOd = new SelectList(db.Korisniks, "ID", "Ime", uput.IDLekaraOd);
            return View(uput);
        }

        // GET: Uputs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uput uput = db.Uputs.Find(id);
            if (uput == null)
            {
                return HttpNotFound();
            }
            //ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime", uput.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime", uput.IDPacijenta);
            ViewBag.IDOdeljenja = new SelectList(db.Odeljenjes, "ID", "Naziv", uput.IDOdeljenja);
            ViewBag.IDLekaraKome = new SelectList(db.Korisniks, "ID", "Ime", uput.IDLekaraKome);
            ViewBag.IDLekaraOd = new SelectList(db.Korisniks, "ID", "Ime", uput.IDLekaraOd);
            return View(uput);
        }

        // POST: Uputs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDPacijenta,IDLekara,DatumPregleda,IDOdeljenja,ZavrsenPregled,IDLekaraOd,IDLekaraKome")] Uput uput)
        {
            if (ModelState.IsValid)
            {
                db.Entry(uput).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime", uput.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime", uput.IDPacijenta);
            ViewBag.IDOdeljenja = new SelectList(db.Odeljenjes, "ID", "Naziv", uput.IDOdeljenja);
            ViewBag.IDLekaraKome = new SelectList(db.Korisniks, "ID", "Ime", uput.IDLekaraKome);
            ViewBag.IDLekaraOd = new SelectList(db.Korisniks, "ID", "Ime", uput.IDLekaraOd);
            return View(uput);
        }

        // GET: Uputs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Uput uput = db.Uputs.Find(id);
            if (uput == null)
            {
                return HttpNotFound();
            }
            return View(uput);
        }

        // POST: Uputs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Uput uput = db.Uputs.Find(id);
            db.Uputs.Remove(uput);
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
