using EvidencijaPacijenata.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class UstanovasController : Controller
    {
        private DBZUstanovaEntities db = new DBZUstanovaEntities();

        // GET: Ustanovas
        public ActionResult Index()
        {
            return View(db.Ustanovas.ToList());
        }

        // GET: Ustanovas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ustanova ustanova = db.Ustanovas.Find(id);
            if (ustanova == null)
            {
                return HttpNotFound();
            }
            return View(ustanova);
        }

        // GET: Ustanovas/Create
        public ActionResult Create()
        {
            return Session["IDAdmina"] != null ? View() : (ActionResult)RedirectToAction("Index", "Home");
        }

        // POST: Ustanovas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Naziv,Adresa,Telefon,Email,Slika")] Ustanova ustanova)
        {
            if (ModelState.IsValid)
            {
                db.Ustanovas.Add(ustanova);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ustanova);
        }

        // GET: Ustanovas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["IDAdmina"] != null)
            {
                Ustanova ustanova = db.Ustanovas.Find(id);
                if (ustanova == null)
                {
                    return HttpNotFound();
                }
                return View(ustanova);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Ustanovas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Naziv,Adresa,Telefon,Email,Slika")] Ustanova ustanova)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ustanova).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ustanova);
        }

        // GET: Ustanovas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["IDAdmina"] != null)
            {
                Ustanova ustanova = db.Ustanovas.Find(id);
                if (ustanova == null)
                {
                    return HttpNotFound();
                }
                return View(ustanova);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Ustanovas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ustanova ustanova = db.Ustanovas.Find(id);
            db.Ustanovas.Remove(ustanova);
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
