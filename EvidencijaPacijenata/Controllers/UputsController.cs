using EvidencijaPacijenata.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class UputsController : Controller
    {
        private DBZUstanovaEntities db = new DBZUstanovaEntities();

        // GET: Uputs
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                if (Session["IDPacijenta"] != null)
                {
                    if (id == Convert.ToInt32(Session["IDPacijenta"]))
                    {
                        return View((from u in db.Uputs
                                     where u.IDPacijenta == id
                                     orderby u.DatumPregleda
                                     select u).ToList());
                    }
                    return RedirectToAction("Index", "Home");
                }
                if (Session["Specijalizacija"] != null && id == Convert.ToInt32(Session["IDLekara"]))
                {
                    DateTime danas = DateTime.Now.Date;
                    return View(db.Uputs.Where(u => u.IDLekaraKome == id && u.DatumPregleda == danas).ToList());
                }
                return RedirectToAction("Index", "Home");
            }
            if (Session["IDAdmina"] != null)
            {
                var uputs = db.Uputs.Include(u => u.Korisnik).Include(u => u.Korisnik1).Include(u => u.Odeljenje).Include(u => u.Korisnik2);
                return View(uputs.ToList());
            }
            return RedirectToAction("Index", "Home");
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
        public ActionResult Create(int? id)
        {
            //ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime");
            if (id != null)
            {
                if (Session["IDLekara"] != null)
                {
                    if (Session["Specijalizacija"] == null)
                    {
                        int IDLekaraOd = Convert.ToInt32(Session["IDLekara"]);
                        ViewBag.IDPacijenta = new SelectList(from p in db.Korisniks.OfType<Pacijent>()
                                                             where p.ID == id
                                                             select p, "ID", "Ime");
                        ViewBag.IDLekaraOD = new SelectList(from lop in db.Korisniks.OfType<LekarOpstePrakse>()
                                                            where lop.ID == IDLekaraOd
                                                            select lop, "ID", "Ime");
                        ViewBag.IDUstanove = new SelectList(db.Ustanovas, "ID", "Naziv");
                        List<SelectListItem> izbor = new List<SelectListItem>();
                        izbor.Add(new SelectListItem { Text = "--- Izaberite odeljenje ---", Value = "0" });
                        ViewBag.IDOdeljenja = new SelectList(izbor, "Value", "Text");
                        List<SelectListItem> izbor2 = new List<SelectListItem>();
                        izbor2.Add(new SelectListItem { Text = "--- Izaberite lekara ---", Value = "0" });
                        ViewBag.IDLekaraKome = new SelectList(izbor2, "Value", "Text");

                    }
                    else
                    {
                        int IDLekaraOd = Convert.ToInt32(Session["IDLekara"]);
                        ViewBag.IDPacijenta = new SelectList(from p in db.Korisniks.OfType<Pacijent>()
                                                             where p.ID == id
                                                             select p, "ID", "Ime");
                        ViewBag.IDLekaraOD = new SelectList(from lop in db.Korisniks.OfType<LekarSpecijalista>()
                                                            where lop.ID == IDLekaraOd
                                                            select lop, "ID", "Ime");
                        ViewBag.IDUstanove = new SelectList(db.Ustanovas, "ID", "Naziv");
                        List<SelectListItem> izbor = new List<SelectListItem>();
                        izbor.Add(new SelectListItem { Text = "--- Izaberite odeljenje ---", Value = "0" });
                        ViewBag.IDOdeljenja = new SelectList(izbor, "Value", "Text");
                        List<SelectListItem> izbor2 = new List<SelectListItem>();
                        izbor2.Add(new SelectListItem { Text = "--- Izaberite lekara ---", Value = "0" });
                        ViewBag.IDLekaraKome = new SelectList(izbor2, "Value", "Text");
                    }
                }
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
        [HttpPost]
        public ActionResult Lekari(string IDOdeljenja)
        {
            int IDLekara = Convert.ToInt32(Session["IDLekara"]);
            int idOdeljenja = Convert.ToInt32(IDOdeljenja);
            SelectList IDLekaraKome = new SelectList(from od in db.Korisniks.OfType<LekarSpecijalista>()
                                                     where od.IDOdeljenja == idOdeljenja && od.ID != IDLekara
                                                     select od, "ID", "Ime");
            return Json(IDLekaraKome);
        }
        // POST: Uputs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDPacijenta,IDLekaraOd,IDLekaraKome,DatumPregleda,IDOdeljenja,ZavrsenPregled")] Uput uput)
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
