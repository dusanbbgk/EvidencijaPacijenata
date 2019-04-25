using EvidencijaPacijenata.Models;
using System;
using System.Data;
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
                return View((from p in db.Korisniks.OfType<Pacijent>()
                             join u in db.Ustanovas on p.IDUstanove equals u.ID
                             join o in db.Odeljenjes on u.ID equals o.IDUstanove
                             join l in db.Korisniks.OfType<Lekar>() on o.ID equals l.IDOdeljenja
                             where (p.Ime.Contains(pretraga) || p.Prezime.Contains(pretraga) || pretraga == null) && l.ID == IDLekara
                             select p).ToList());
            }
            else
                return RedirectToAction("Index", "Home");
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
            if (Session["IDPacijenta"] != null)
                return RedirectToAction("Index", "Home");
            return View();
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
