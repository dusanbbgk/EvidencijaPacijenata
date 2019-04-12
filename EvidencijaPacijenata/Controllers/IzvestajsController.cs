﻿using System;
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
    public class IzvestajsController : Controller
    {
        private DBZUstanovaEntities db = new DBZUstanovaEntities();

        // GET: Izvestajs
        public ActionResult Index()
        {
            var izvestajs = db.Izvestajs.Include(i => i.Korisnik).Include(i => i.Korisnik1);
            return View(izvestajs.ToList());
        }

        // GET: Izvestajs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Izvestaj izvestaj = db.Izvestajs.Find(id);
            if (izvestaj == null)
            {
                return HttpNotFound();
            }
            return View(izvestaj);
        }

        // GET: Izvestajs/Create
        public ActionResult Create()
        {
            ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime");
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime");
            return View();
        }

        // POST: Izvestajs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDPacijenta,IDLekara,DatumPregleda,Dijagnoza")] Izvestaj izvestaj)
        {
            if (ModelState.IsValid)
            {
                db.Izvestajs.Add(izvestaj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime", izvestaj.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime", izvestaj.IDPacijenta);
            return View(izvestaj);
        }

        // GET: Izvestajs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Izvestaj izvestaj = db.Izvestajs.Find(id);
            if (izvestaj == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime", izvestaj.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime", izvestaj.IDPacijenta);
            return View(izvestaj);
        }

        // POST: Izvestajs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDPacijenta,IDLekara,DatumPregleda,Dijagnoza")] Izvestaj izvestaj)
        {
            if (ModelState.IsValid)
            {
                db.Entry(izvestaj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime", izvestaj.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime", izvestaj.IDPacijenta);
            return View(izvestaj);
        }

        // GET: Izvestajs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Izvestaj izvestaj = db.Izvestajs.Find(id);
            if (izvestaj == null)
            {
                return HttpNotFound();
            }
            return View(izvestaj);
        }

        // POST: Izvestajs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Izvestaj izvestaj = db.Izvestajs.Find(id);
            db.Izvestajs.Remove(izvestaj);
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
