using EvidencijaPacijenata.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class ZakazivanjePregledasController : Controller
    {
        private DBZUstanovaEntities db = new DBZUstanovaEntities();

        // GET: ZakazivanjePregledas
        public ActionResult Index()
        {
            var zakazivanjePregledas = db.ZakazivanjePregledas.Include(z => z.Korisnik).Include(z => z.Korisnik1);
            return View(zakazivanjePregledas.ToList());
        }

        // GET: ZakazivanjePregledas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZakazivanjePregleda zakazivanjePregleda = db.ZakazivanjePregledas.Find(id);
            if (zakazivanjePregleda == null)
            {
                return HttpNotFound();
            }
            return View(zakazivanjePregleda);
        }

        // GET: ZakazivanjePregledas/Create
        public ActionResult Create()
        {
            if (Session["IDPacijenta"] != null)
            {
                var IDPacijenta = Convert.ToInt32(Session["IDPacijenta"]);
                var IDUstanove = (from x in db.Korisniks.OfType<Pacijent>()
                                  where x.ID == IDPacijenta
                                  select x.IDUstanove).SingleOrDefault();
                //ViewBag.IDLekara = new SelectList(db.Korisniks.OfType<LekarOpstePrakse>(), "ID", "Ime");
                ViewBag.IDLekara = new SelectList(db.Korisniks.OfType<LekarOpstePrakse>().Join(db.Odeljenjes,
                              lop => lop.ID,
                              od => od.ID,
                              (lop, od) => new { LekarOpstePrakse = lop, Odeljenje = od })
                          .Where(od1 => od1.Odeljenje.IDUstanove == IDUstanove)
                          .Select(lop1 => new
                          {
                              lop1.LekarOpstePrakse.ID,
                              lop1.LekarOpstePrakse.Ime,
                              lop1.LekarOpstePrakse.Prezime
                          }), "ID", "Ime", "Prezime");
                return View();
            }
            //ViewBag.IDPacijenta = new SelectList(db.Korisniks.OfType<Pacijent>(), "ID", "Ime");
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: ZakazivanjePregledas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDPacijenta,IDLekara,DatumPregleda,DatumZakazivanja,VremePregleda")] ZakazivanjePregleda zakazivanjePregleda)
        {
            if (ModelState.IsValid)
            {
                db.ZakazivanjePregledas.Add(zakazivanjePregleda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime", zakazivanjePregleda.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime", zakazivanjePregleda.IDPacijenta);
            return View(zakazivanjePregleda);
        }

        // GET: ZakazivanjePregledas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZakazivanjePregleda zakazivanjePregleda = db.ZakazivanjePregledas.Find(id);
            if (zakazivanjePregleda == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime", zakazivanjePregleda.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime", zakazivanjePregleda.IDPacijenta);
            return View(zakazivanjePregleda);
        }

        // POST: ZakazivanjePregledas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDPacijenta,IDLekara,DatumPregleda,DatumZakazivanja,VremePregleda")] ZakazivanjePregleda zakazivanjePregleda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(zakazivanjePregleda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDLekara = new SelectList(db.Korisniks, "ID", "Ime", zakazivanjePregleda.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks, "ID", "Ime", zakazivanjePregleda.IDPacijenta);
            return View(zakazivanjePregleda);
        }

        // GET: ZakazivanjePregledas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ZakazivanjePregleda zakazivanjePregleda = db.ZakazivanjePregledas.Find(id);
            if (zakazivanjePregleda == null)
            {
                return HttpNotFound();
            }
            return View(zakazivanjePregleda);
        }

        // POST: ZakazivanjePregledas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ZakazivanjePregleda zakazivanjePregleda = db.ZakazivanjePregledas.Find(id);
            db.ZakazivanjePregledas.Remove(zakazivanjePregleda);
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
