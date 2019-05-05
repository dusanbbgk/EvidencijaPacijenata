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
        private DBZUstanovaBetaEntities db = new DBZUstanovaBetaEntities();

        // GET: ZakazivanjePregledas
        public ActionResult Index(int? id)
        {
            DateTime dt = DateTime.Now;
            DateTime dateOnly = dt.Date;
            if (id == null && Session["IDAdmina"] != null)
            {
                var zakazivanjePregledas = db.ZakazivanjePregledas.Include(z => z.Korisnik).Include(z => z.Korisnik1);
                return View(zakazivanjePregledas.ToList());
            }
            if (Session["IDLekara"] != null && Session["Specijalizacija"] == null)
            {
                if (Convert.ToInt32(Session["IDLekara"]) == id)
                {
                    var zakazaniPregledi = (from zp in db.ZakazivanjePregledas
                                            where zp.DatumPregleda == dateOnly && zp.IDLekara == id && zp.ZavrsenPregled == 0
                                            select zp);
                    return View(zakazaniPregledi.ToList());
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: ZakazivanjePregledas/Details/5
        public ActionResult Details(int? id)
        {
            if (TempData["PostojiZakazanPregled"] != null)
            {
                ViewBag.PostojiZakazanPregled = TempData["PostojiZakazanPregled"];
                TempData.Clear();
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["IDAdmina"] != null || (Session["IDLekara"] != null && Session["Specijalizacija"] == null))
            {
                ZakazivanjePregleda zakazivanjePregleda = db.ZakazivanjePregledas.Find(id);
                if (zakazivanjePregleda == null)
                {
                    return HttpNotFound();
                }
                return View(zakazivanjePregleda);
            }
            if (Session["IDPacijenta"] != null)
            {
                if (Convert.ToInt32(Session["IDPacijenta"]) == id)
                {
                    DateTime dt = DateTime.Now;
                    DateTime dateOnly = dt.Date;
                    var pregled = db.ZakazivanjePregledas.Where(z => z.IDPacijenta == id && z.DatumPregleda >= dateOnly).First();
                    if (pregled != null) {
                        ViewBag.Ustanova = (from l in db.Korisniks.OfType<LekarOpstePrakse>()
                                            join o in db.Odeljenjes on l.IDOdeljenja equals o.ID
                                            join u in db.Ustanovas on o.IDUstanove equals u.ID
                                            where l.ID == pregled.IDLekara
                                            select new { u.Naziv} ).First();
                        ViewBag.Odeljenje = (from l in db.Korisniks.OfType<LekarOpstePrakse>()
                                            join o in db.Odeljenjes on l.IDOdeljenja equals o.ID
                                            where l.ID == pregled.IDLekara
                                            select o.Naziv).First();
                        return View(pregled);
                    }
                    TempData["NemaPregleda"] = "Nemate zakazanih pregleda";
                }
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: ZakazivanjePregledas/Create
        public ActionResult Create()
        {
            if (Session["IDPacijenta"] != null)
            {
                var id = Convert.ToInt32(Session["IDPacijenta"]);
                DateTime dt = DateTime.Now;
                DateTime dateOnly = dt.Date;
                var pregled = db.ZakazivanjePregledas.Where(z => z.IDPacijenta == id && z.DatumPregleda >= dateOnly).FirstOrDefault();
                if (pregled != null)
                {
                    TempData["PostojiZakazanPregled"] = "Ne možete zakazati više od jednog pregleda pre nego što obavite već zakazan";
                    return RedirectToAction("Details", new { id });
                }
                var IDPacijenta = Convert.ToInt32(Session["IDPacijenta"]);
                var IDUstanove = (from x in db.Korisniks.OfType<Pacijent>()
                                  where x.ID == IDPacijenta
                                  select x.IDUstanove).SingleOrDefault();
                ViewBag.IDLekara = new SelectList(from k in db.Korisniks.OfType<LekarOpstePrakse>()
                                                  join o in db.Odeljenjes on k.IDOdeljenja equals o.ID
                                                  where o.IDUstanove == IDUstanove
                                                  select k, "ID", "ImePrezime");
                List<SelectListItem> izbor = new List<SelectListItem>();
                izbor.Add(new SelectListItem { Text = "--- Izaberite termin ---", Value = "0" });
                ViewBag.VremePregleda = new SelectList(izbor, "Value", "Text");
                ViewBag.DatumPregleda = DateTime.Now.Date.AddDays(1).ToString("yyyy-MM-dd");
                return View();
            }
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Termini(string IDLekara, string DatumPregleda)
        {
            DateTime datumPregleda = Convert.ToDateTime(DatumPregleda);
            int idLekara = Convert.ToInt32(IDLekara);
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

                int termin = db.slobodniTermini(idLekara, datumPregleda, vreme).Count();
                if (termin == 0)
                    slobodniTermini.Add(termini[i]);
            }
            SelectList slobodniTerminiLista = new SelectList(slobodniTermini, "Value", "Text");
            return Json(slobodniTerminiLista);
        }

        // POST: ZakazivanjePregledas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IDPacijenta,IDLekara,DatumPregleda,VremePregleda,DatumZakazivanja,ZavrsenPregled")] ZakazivanjePregleda zakazivanjePregleda)
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
            List<SelectListItem> izbor = new List<SelectListItem>();
            izbor.Add(new SelectListItem { Text = "--- Izaberite termin ---", Value = "0" });
            ViewBag.VremePregleda = new SelectList(izbor, "Value", "Text");
            ViewBag.DatumPregleda = DateTime.Now.Date.AddDays(1).ToString("yyyy-MM-dd");
            ViewBag.IDLekara = new SelectList(db.Korisniks.OfType<LekarOpstePrakse>(), "ID", "ImePrezime", zakazivanjePregleda.IDLekara);
            ViewBag.IDPacijenta = new SelectList(db.Korisniks.OfType<Pacijent>().Where(p => p.ID == zakazivanjePregleda.IDPacijenta), "ID", "ImePrezime");
            return View(zakazivanjePregleda);
        }

        // POST: ZakazivanjePregledas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDPacijenta,IDLekara,DatumPregleda,VremePregleda,DatumZakazivanja,ZavrsenPregled")] ZakazivanjePregleda zakazivanjePregleda)
        {
            if (ModelState.IsValid)
            {
                DateTime dt = DateTime.Now;
                DateTime dateOnly = dt.Date;
                zakazivanjePregleda.DatumZakazivanja = dateOnly;
                zakazivanjePregleda.IDPacijenta = Convert.ToInt32(Session["IDPacijenta"]);
                zakazivanjePregleda.ZavrsenPregled = 0;
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
