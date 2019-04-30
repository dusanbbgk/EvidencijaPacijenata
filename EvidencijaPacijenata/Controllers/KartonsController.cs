using EvidencijaPacijenata.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class KartonsController : Controller
    {
        private DBZUstanovaEntities db = new DBZUstanovaEntities();

        // GET: Kartons
        public ActionResult Index()
        {
            if (Session["IDAdmina"] != null)
            {
                var kartons = db.Kartons.Include(k => k.Korisnik).Include(k => k.Korisnik1);
                return View(kartons.ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Kartons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (Session["IDPacijenta"] != null)
            {
                if (id != Convert.ToInt32(Session["IDPacijenta"]))
                    return RedirectToAction("Index", "Home");
                Karton karton = db.Kartons.SingleOrDefault(k => k.IDPacijenta == id);
                if (karton == null)
                {
                    Session["NemaKarton"] = "Ne postoji karton za pacijenta!";
                    return RedirectToAction("Index", "Home");
                }
                return View(karton);
            }
            else if (Session["IDLekara"] != null || Session["IDAdmina"] != null)
            {
                Karton karton = db.Kartons.SingleOrDefault(k => k.IDPacijenta == id);
                if (karton == null)
                {
                    //Session["NemaKarton"] = "Ne postoji karton za pacijenta!";
                    //return RedirectToAction("Index", "Home");
                    return RedirectToAction("Create");
                }
                return View(karton);
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: Kartons/Create
        public ActionResult Create()
        {
            ViewBag.IDLekara = new SelectList(db.Korisniks.OfType<Lekar>(), "ID", "ImePrezime");
            ViewBag.IDPacijenta = new SelectList(db.Korisniks.OfType<Pacijent>(), "ID", "ImePrezime");
            return View();
        }

        // POST: Kartons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDPacijenta,IDLekara,DatumVremeNalaza,Disanje,Puls,TelesnaTemperatura,KrvniPritisak,Mokraca,Stolica,KrvaSlika,SpecificniPregled")] Karton karton)
        {
            DateTime dt = DateTime.Now;
            DateTime dateOnly = dt.Date;
            karton.DatumVremeNalaza = dateOnly;
            if (ModelState.IsValid)
            {
                db.Kartons.Add(karton);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDLekara = new SelectList(db.Korisniks.OfType<Lekar>(), "ID", "ImePrezime", karton.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks.OfType<Pacijent>(), "ID", "ImePrezime", karton.IDPacijenta);
            return View(karton);
        }

        // GET: Kartons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Karton karton = db.Kartons.Find(id);
            if (karton == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDLekara = new SelectList(db.Korisniks.OfType<Lekar>(), "ID", "ImePrezime", karton.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks.OfType<Pacijent>(), "ID", "ImePrezime", karton.IDPacijenta);
            return View(karton);
        }

        // POST: Kartons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDPacijenta,IDLekara,DatumVremeNalaza,Disanje,Puls,TelesnaTemperatura,KrvniPritisak,Mokraca,Stolica,KrvaSlika,SpecificniPregled")] Karton karton)
        {
            DateTime dt = DateTime.Now;
            DateTime dateOnly = dt.Date;
            karton.DatumVremeNalaza = dateOnly;
            if (ModelState.IsValid)
            {
                db.Entry(karton).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDLekara = new SelectList(db.Korisniks.OfType<Lekar>(), "ID", "ImePrezime", karton.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks.OfType<Pacijent>(), "ID", "ImePrezime", karton.IDPacijenta);
            return View(karton);
        }

        // GET: Kartons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Karton karton = db.Kartons.Find(id);
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
            Karton karton = db.Kartons.Find(id);
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
