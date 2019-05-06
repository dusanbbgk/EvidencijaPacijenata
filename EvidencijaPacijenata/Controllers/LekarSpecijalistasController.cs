﻿using EvidencijaPacijenata.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class LekarSpecijalistasController : Controller
    {
        private DBZUstanovaBetaEntities db = new DBZUstanovaBetaEntities();
        string encryptpw;
        // GET: LekarSpecijalistas
        public ActionResult Index()
        {
            return Session["IDAdmina"] != null ? View(db.Korisniks.OfType<LekarSpecijalista>().ToList()) : (ActionResult)RedirectToAction("Index", "Home");
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
            if (Session["IDAdmina"] != null)
            {
                ViewBag.IDUstanove = new SelectList(db.Ustanovas.ToList(), "ID", "Naziv");
                List<SelectListItem> izbor = new List<SelectListItem>();
                izbor.Add(new SelectListItem { Text = "--- Izaberite odeljenje ---", Value = "0" });
                ViewBag.IDOdeljenja = new SelectList(izbor, "Value", "Text");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Odeljenja(string IDUstanove)
        {
            int idUstanove = Convert.ToInt32(IDUstanove);
            SelectList IDOdeljenja = new SelectList(from od in db.Odeljenjes
                                                    where od.IDUstanove == idUstanove
                                                    select od, "ID", "Naziv");
            return Json(IDOdeljenja);
        }

        // POST: LekarSpecijalistas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Ime,Prezime,KorisnickoIme,Lozinka,DatumRodjenja,IDOdeljenja,Licenca,Slika,Specijalizacija")] LekarSpecijalista lekarSpecijalista, HttpPostedFileBase file)
        {
            encryption(lekarSpecijalista.Lozinka);
            lekarSpecijalista.Lozinka = encryptpw;
            if (file != null && file.ContentLength > 0)
                try
                {
                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Imgs/Lekari"), lekarSpecijalista.KorisnickoIme));
                    string path = Path.Combine(Server.MapPath("~/Imgs/Lekari/" + lekarSpecijalista.KorisnickoIme),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            if (ModelState.IsValid)
            {
                db.Korisniks.Add(lekarSpecijalista);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDUstanove = new SelectList(db.Ustanovas.ToList(), "ID", "Naziv");
            List<SelectListItem> izbor = new List<SelectListItem>();
            izbor.Add(new SelectListItem { Text = "--- Izaberite odeljenje ---", Value = "0" });
            ViewBag.IDOdeljenja = new SelectList(izbor, "Value", "Text");
            return View(lekarSpecijalista);
        }

        private void encryption(string lozinka)
        {
            string strmsg = String.Empty;
            byte[] encode = new byte[lozinka.Length];
            encode = Encoding.UTF8.GetBytes(lozinka);
            strmsg = Convert.ToBase64String(encode);
            encryptpw = strmsg;

        }

        // GET: LekarSpecijalistas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["IDAdmina"] != null || id == Convert.ToInt32(Session["IDLekara"]))
            {
                LekarSpecijalista lekarSpecijalista = db.Korisniks.OfType<LekarSpecijalista>().SingleOrDefault(l => l.ID == id);
                if (lekarSpecijalista == null)
                {
                    return HttpNotFound();
                }
                return View(lekarSpecijalista);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: LekarSpecijalistas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Ime,Prezime,KorisnickoIme,Lozinka,DatumRodjenja,IDOdeljenja,Licenca,Slika,Specijalizacija")] LekarSpecijalista lekarSpecijalista)
        {
            if (ModelState.IsValid)
            {
                ModelState.Remove("Lozinka");
                ModelState.Remove("IDOdeljenja");
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
            string path = Server.MapPath(@"~/Imgs/Lekari/" + lekarSpecijalista.KorisnickoIme);
            Directory.Delete(path, true);
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
