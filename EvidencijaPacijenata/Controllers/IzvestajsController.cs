using EvidencijaPacijenata.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class IzvestajsController : Controller
    {
        private DBZUstanovaBetaEntities db = new DBZUstanovaBetaEntities();

        // GET: Izvestajs
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                if (Session["IDPacijenta"] != null)
                {
                    if (id == Convert.ToInt32(Session["IDPacijenta"]))
                    {
                        return View((from i in db.Izvestajs
                                     where i.IDPacijenta == id
                                     orderby i.DatumPregleda
                                     select i).ToList());
                    }
                    return RedirectToAction("Index", "Home");
                }
                else if (Session["IDLekara"] != null)
                {
                    int IDLekara = Convert.ToInt32(Session["IDLekara"]);
                    if (Session["Specijalizacija"] == null)
                    {
                        var IDUstanove = (from lop in db.Korisniks.OfType<LekarOpstePrakse>()
                                          join o in db.Odeljenjes on lop.IDOdeljenja equals o.ID
                                          join u in db.Ustanovas on o.IDUstanove equals u.ID
                                          where lop.ID == IDLekara
                                          select u).First().ID;
                        return View((from i in db.Izvestajs
                                     join p in db.Korisniks.OfType<Pacijent>() on i.IDPacijenta equals p.ID
                                     where i.IDPacijenta == id && p.IDUstanove == IDUstanove
                                     orderby i.DatumPregleda
                                     select i).ToList());
                    }
                    else
                    {
                        var IDOdeljenja = (from ls in db.Korisniks.OfType<LekarSpecijalista>()
                                           where ls.ID == IDLekara
                                           select ls).First().IDOdeljenja;
                        return View((from i in db.Izvestajs
                                     join p in db.Korisniks.OfType<Pacijent>() on i.IDPacijenta equals p.ID
                                     where i.IDPacijenta == id && p.IDOdeljenja == IDOdeljenja
                                     orderby i.DatumPregleda
                                     select i).ToList());
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Session["IDAdmina"] != null)
                {
                    var izvestajs = db.Izvestajs.Include(i => i.Korisnik).Include(i => i.Korisnik1);
                    return View(izvestajs.ToList());
                }
                return RedirectToAction("Index", "Home");
            }
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
        public ActionResult Create(int? id)
        {
            if (id.HasValue)
            {
                if (Session["IDLekara"] != null)
                {
                    var IDLekara = Convert.ToInt32(Session["IDLekara"]);
                    ViewBag.IDPacijenta = new SelectList(from p in db.Korisniks.OfType<Pacijent>() where p.ID == id select p, "ID", "ImePrezime");
                    if (Session["Specijalizacija"] == null)
                        ViewBag.IDLekara = new SelectList(db.Korisniks.OfType<LekarOpstePrakse>().Where(lop => lop.ID == IDLekara), "ID", "ImePrezime");
                    else
                        ViewBag.IDLekara = new SelectList(db.Korisniks.OfType<LekarSpecijalista>().Where(ls => ls.ID == IDLekara), "ID", "ImePrezime");
                    return View();
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                if (Session["IDAdmina"] != null)
                {
                    ViewBag.IDLekara = new SelectList(db.Korisniks.OfType<LekarSpecijalista>(), "ID", "ImePrezime");
                    ViewBag.IDPacijenta = new SelectList(db.Korisniks.OfType<Pacijent>(), "ID", "ImePrezime");
                    return View();
                }
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Izvestajs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDPacijenta,IDLekara,DatumPregleda,Dijagnoza")] Izvestaj izvestaj)
        {
            DateTime dt = DateTime.Now;
            DateTime dateOnly = dt.Date;
            izvestaj.DatumPregleda = dateOnly;
            if (ModelState.IsValid)
            {
                db.Izvestajs.Add(izvestaj);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDLekara = new SelectList(db.Korisniks.OfType<LekarSpecijalista>().Where(l => l.ID == izvestaj.IDLekara), "ID", "ImePrezime");
            ViewBag.IDPacijenta = new SelectList(db.Korisniks.OfType<Pacijent>().Where(p => p.ID == izvestaj.IDPacijenta), "ID", "ImePrezime");
            return View(izvestaj);
        }

        // GET: Izvestajs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["IDAdmina"] != null || Session["IDLekara"] != null)
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
                ViewBag.IDLekara = new SelectList(db.Korisniks.OfType<LekarSpecijalista>(), "ID", "ImePrezime", izvestaj.IDLekara);
                ViewBag.IDPacijenta = new SelectList(db.Korisniks.OfType<Pacijent>(), "ID", "ImePrezime", izvestaj.IDPacijenta);
                return View(izvestaj);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Izvestajs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDPacijenta,IDLekara,DatumPregleda,Dijagnoza")] Izvestaj izvestaj)
        {
            DateTime dt = DateTime.Now;
            DateTime dateOnly = dt.Date;
            izvestaj.DatumPregleda = dateOnly;
            if (ModelState.IsValid)
            {
                db.Entry(izvestaj).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDLekara = new SelectList(db.Korisniks.OfType<LekarSpecijalista>(), "ID", "ImePrezime", izvestaj.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks.OfType<Pacijent>(), "ID", "ImePrezime", izvestaj.IDPacijenta);
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
