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
    public class VestisController : Controller
    {
        private DBZUstanovaEntities db = new DBZUstanovaEntities();

        // GET: Vestis
        public ActionResult Index()
        {
            return View(db.Vestis.ToList());
        }

        // GET: Vestis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vesti vesti = db.Vestis.Find(id);
            if (vesti == null)
            {
                return HttpNotFound();
            }
            return View(vesti);
        }

        // GET: Vestis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Vestis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Naslov,Tekst,DatumObjave,Slika")] Vesti vesti)
        {
            if (ModelState.IsValid)
            {
                db.Vestis.Add(vesti);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vesti);
        }

        // GET: Vestis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vesti vesti = db.Vestis.Find(id);
            if (vesti == null)
            {
                return HttpNotFound();
            }
            return View(vesti);
        }

        // POST: Vestis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Naslov,Tekst,DatumObjave,Slika")] Vesti vesti)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vesti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vesti);
        }

        // GET: Vestis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vesti vesti = db.Vestis.Find(id);
            if (vesti == null)
            {
                return HttpNotFound();
            }
            return View(vesti);
        }

        // POST: Vestis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vesti vesti = db.Vestis.Find(id);
            db.Vestis.Remove(vesti);
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
