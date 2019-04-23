using EvidencijaPacijenata.Models;
using System;
using System.Collections.Generic;
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
        public ActionResult Index(int? id)
        {
            DateTime dt = DateTime.Now;
            DateTime dateOnly = dt.Date;
            if (id == null && Convert.ToInt32(Session["IDAdmina"]) == id)
            {
                var zakazivanjePregledas = db.ZakazivanjePregledas.Include(z => z.Korisnik).Include(z => z.Korisnik1);
                return View(zakazivanjePregledas.ToList());
            }
            if (Convert.ToInt32(Session["IDLekara"]) == id)
            {
                var zakazaniPregledi = (from zp in db.ZakazivanjePregledas
                                        where zp.DatumPregleda == dateOnly && zp.IDLekara == id && zp.ZavrsenPregled == 0
                                        select zp);
                return View(zakazaniPregledi.ToList());
            }
            else
                return RedirectToAction("Index", "Home");
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
                ViewBag.IDLekara = new SelectList(from k in db.Korisniks.OfType<LekarOpstePrakse>()
                                                  join o in db.Odeljenjes on k.IDOdeljenja equals o.ID
                                                  where o.IDUstanove == IDUstanove
                                                  select new
                                                  {
                                                      k.ID,
                                                      k.Ime
                                                  }, "ID", "Ime");

                List<SelectListItem> termini = new List<SelectListItem>();
                var dateNow = DateTime.Now;
                var pocetak = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 8, 0, 0);
                var kraj = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 16, 0, 0);

                while (DateTime.Compare(pocetak, kraj) <= 0)
                {
                    termini.Add(new SelectListItem { Text = pocetak.ToString("hh:mm"), Value = pocetak.ToString("hh:mm") });
                    pocetak = pocetak.AddMinutes(20);
                }
                SelectList listaTermina = new SelectList(termini, "Value", "Text");
                ViewBag.VremePregleda = listaTermina;
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Termini(int IDLekara)
        {
            List<SelectListItem> termini = new List<SelectListItem>();
            var dateNow = DateTime.Now;
            var pocetak = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 8, 0, 0);
            var kraj = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 16, 0, 0);

            while (DateTime.Compare(pocetak, kraj) <= 0)
            {
                termini.Add(new SelectListItem { Text = pocetak.ToString("hh:mm"), Value = pocetak.ToString("hh:mm") });
                pocetak = pocetak.AddMinutes(20);
            }
            List<SelectListItem> slobodniTermini = new List<SelectListItem>();
            for (int i = 0; i < termini.Count; i++)
            {
                DateTime datum = Convert.ToDateTime(termini[i].Value);
                TimeSpan vreme = datum.TimeOfDay;
                var termin = db.ZakazivanjePregledas.SingleOrDefault(zp => zp.IDLekara == IDLekara && zp.VremePregleda == vreme);
                if(termin == null)
                    slobodniTermini.Add(/*new SelectListItem { Text = termini[i].Text, Value = termini[i].Value }*/termini[i]);
            }
            SelectList slobodniTerminiLista = new SelectList(slobodniTermini, "Value", "Text");

            //SelectList VremePregleda = new SelectList(from zp in db.ZakazivanjePregledas where zp.IDLekara == IDLekara select zp, "VremePregleda", "VremePregleda");
            return Json(slobodniTerminiLista);
        }
        // POST: ZakazivanjePregledas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDPacijenta,IDLekara,DatumPregleda,DatumZakazivanja,VremePregleda")] ZakazivanjePregleda zakazivanjePregleda)
        {
            DateTime dt = DateTime.Now;
            DateTime dateOnly = dt.Date;
            zakazivanjePregleda.DatumZakazivanja = dateOnly;
            zakazivanjePregleda.IDPacijenta = Convert.ToInt32(Session["IDPacijenta"]);
            zakazivanjePregleda.ZavrsenPregled = 0;
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
