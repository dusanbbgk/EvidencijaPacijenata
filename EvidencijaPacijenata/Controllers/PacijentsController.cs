using EvidencijaPacijenata.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class PacijentsController : Controller
    {
        private DBZUstanovaEntities db = new DBZUstanovaEntities();

        // GET: Pacijents
        public ActionResult Index()
        {
            return View(db.Korisniks.OfType<Pacijent>().ToList());
        }
        public ActionResult Pretraga(string pretraga)
        {
            if (Session["IDLekara"] != null)
            {
                int IDLekara = Convert.ToInt32(Session["IDLekara"]);
                if (pretraga != null)
                {
                    if (Session["Specijalizacija"] != null)
                        return View(db.pretragaPacijenata(pretraga, IDLekara, 1).ToList());
                    else
                        return View(db.pretragaPacijenata(pretraga, IDLekara, 0).ToList());
                }
                else
                {
                    pretraga = "";
                    if (Session["Specijalizacija"] != null)
                        return View(db.pretragaPacijenata(pretraga, IDLekara, 1).ToList());
                    else
                        return View(db.pretragaPacijenata(pretraga, IDLekara, 0).ToList());
                }
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public ActionResult ZadrziNaOdeljenju(int id)
        {
            var pacijent = db.Korisniks.OfType<Pacijent>().SingleOrDefault(p => p.ID == id);
            if (pacijent != null)
            {
                int IDLekara = Convert.ToInt32(Session["IDLekara"]);
                pacijent.IDOdeljenja = db.Korisniks.OfType<LekarSpecijalista>().First(l => l.ID == IDLekara).IDOdeljenja;
                db.Entry(pacijent).Property("IDOdeljenja").IsModified = true;
                db.SaveChanges();
            }
            return RedirectToAction("Pretraga", "Pacijents");
        }

        public ActionResult OtpustiSaOdeljenja(int id)
        {
            var pacijent = db.Korisniks.OfType<Pacijent>().SingleOrDefault(p => p.ID == id);
            if (pacijent != null)
            {
                pacijent.IDOdeljenja = null;
                db.Entry(pacijent).Property("IDOdeljenja").IsModified = true;
                db.SaveChanges();
            }
            return RedirectToAction("Pretraga", "Pacijents");
        }






        // GET: Pacijents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacijent pacijent = db.Korisniks.OfType<Pacijent>().SingleOrDefault(p => p.ID == id);
            if (pacijent == null)
            {
                return HttpNotFound();
            }
            return View(pacijent);
        }

        // GET: Pacijents/Create
        public ActionResult Create()
        {
            if (Session["IDPacijenta"] != null || Session["IDLekara"] != null)
                return RedirectToAction("Index", "Home");
            else
            {
                ViewData["KrvnaGrupa"] = new SelectList(
                    new List<SelectListItem>
                    {
                        new SelectListItem { Text = "A+", Value = "A+" },
                        new SelectListItem { Text = "A-", Value = "A-" },
                        new SelectListItem { Text = "B+", Value = "B+" },
                        new SelectListItem { Text = "B-", Value = "B-" },
                        new SelectListItem { Text = "AB+", Value = "AB+" },
                        new SelectListItem { Text = "AB-", Value = "AB-" },
                        new SelectListItem { Text = "0+", Value = "0+" },
                        new SelectListItem { Text = "0-", Value = "0-" }
                    }, "Value", "Text");
                ViewData["Pol"] = new SelectList(
                    new List<SelectListItem>
                    {
                        new SelectListItem { Text = "M", Value = "M" },
                        new SelectListItem { Text = "Ž", Value = "Ž" },
                    }, "Value", "Text");
                return View();
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult ProveriEmail(string Email) {
            try
            {
                return Json(!db.Korisniks.OfType<Pacijent>().Any(p => p.Email == Email), JsonRequestBehavior.AllowGet);
                //return Json(!IsEmailExists(Email));
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public JsonResult ProveriBZK(string KorisnickoIme)
        {
            try
            {
                return Json(!db.Korisniks.OfType<Pacijent>().Any(p => p.KorisnickoIme == KorisnickoIme), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
        public JsonResult ProveriJMBG(string JMBG)
        {
            try
            {
                return Json(!db.Korisniks.OfType<Pacijent>().Any(p => p.JMBG == JMBG), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }


        // POST: Pacijents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Ime,Prezime,KorisnickoIme,Lozinka,JMBG,NosilacOsiguranja,SrodstvoSaNosiocem,IDOdeljenja,IDUstanove,KrvnaGrupa,Pol,Adresa,Telefon,Email,IstekOsiguranja,Odobren")] Pacijent pacijent)
        {
            DateTime dt = DateTime.Now;
            DateTime dateOnly = dt.Date;
            pacijent.IstekOsiguranja = dateOnly.AddMonths(6);
            if (ModelState.IsValid)
            {
                db.Korisniks.Add(pacijent);
                db.SaveChanges();
                if (Session["IDPacijenta"] == null)
                    return RedirectToAction("Index", "Home");
                return RedirectToAction("Index");
            }
            var KrvnaGrupa = new SelectList(
                    new List<SelectListItem>
                    {
                        new SelectListItem { Text = "A+", Value = "A+" },
                        new SelectListItem { Text = "A-", Value = "A-" },
                        new SelectListItem { Text = "B+", Value = "B+" },
                        new SelectListItem { Text = "B-", Value = "B-" },
                        new SelectListItem { Text = "AB+", Value = "AB+" },
                        new SelectListItem { Text = "AB-", Value = "AB-" },
                        new SelectListItem { Text = "0+", Value = "0+" },
                        new SelectListItem { Text = "0-", Value = "0-" }
                    }, "Value", "Text");
            ViewData["KrvnaGrupa"] = KrvnaGrupa;
            var Pol = new SelectList(
                new List<SelectListItem>
                {
                        new SelectListItem { Text = "M", Value = "M" },
                        new SelectListItem { Text = "Ž", Value = "Ž" },
                }, "Value", "Text");
            ViewData["Pol"] = Pol;
            return View(pacijent);
        }

        // GET: Pacijents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacijent pacijent = db.Korisniks.OfType<Pacijent>().SingleOrDefault(p => p.ID == id);
            if (pacijent == null)
            {
                return HttpNotFound();
            }
            return View(pacijent);
        }

        // POST: Pacijents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Ime,Prezime,KorisnickoIme,Lozinka,JMBG,NosilacOsiguranja,SrodstvoSaNosiocem,IDOdeljenja,IDUstanove,KrvnaGrupa,Pol,Adresa,Telefon,Email,IstekOsiguranja,Odobren")] Pacijent pacijent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pacijent).State = EntityState.Modified;
                db.SaveChanges();
                if (Session["IDPacijenta"] != null)
                    return RedirectToAction("Details", new { id = Session["IDPacijenta"] });
                return RedirectToAction("Index");
            }
            return View(pacijent);
        }

        // GET: Pacijents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pacijent pacijent = db.Korisniks.OfType<Pacijent>().SingleOrDefault(p => p.ID == id);
            if (pacijent == null)
            {
                return HttpNotFound();
            }
            return View(pacijent);
        }

        // POST: Pacijents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pacijent pacijent = db.Korisniks.OfType<Pacijent>().SingleOrDefault(p => p.ID == id);
            db.Korisniks.Remove(pacijent);
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
