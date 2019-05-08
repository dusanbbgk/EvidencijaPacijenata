using EvidencijaPacijenata.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class VestisController : Controller
    {
        private DBZUstanovaBetaEntities db = new DBZUstanovaBetaEntities();

        // GET: Vestis
        public ActionResult Index()
        {
            return View(db.Vestis.ToList());
        }

        // GET: Vestis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vesti vesti = db.Vestis.Find(id);
            if (vesti == null)
            {
                return HttpNotFound();
            }
            return View(vesti);
        }

        // GET: Vestis/Create
        public ActionResult Create()
        {
            return Session["IDAdmina"] != null ? View() : (ActionResult)RedirectToAction("Index", "Home");
        }

        // POST: Vestis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Naslov,Tekst,DatumObjave,Slika")] Vesti vesti, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Imgs/Vesti"), vesti.ID.ToString()));
                    string path = Path.Combine(Server.MapPath("~/Imgs/Vesti/" + vesti.ID.ToString()),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                }
                catch (Exception ex)
                {
                    Session["Obavestenje"] = "ERROR:" + ex.Message.ToString();
                }

            if (ModelState.IsValid)
            {
                db.Vestis.Add(vesti);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vesti);
        }

        // GET: Vestis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["IDAdmina"] != null)
            {
                Vesti vesti = db.Vestis.Find(id);
                if (vesti == null)
                {
                    return HttpNotFound();
                }
                return View(vesti);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Vestis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Naslov,Tekst,DatumObjave,Slika")] Vesti vesti, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    Directory.CreateDirectory(Path.Combine(Server.MapPath("~/Imgs/Vesti"), vesti.ID.ToString()));
                    string path = Path.Combine(Server.MapPath("~/Imgs/Vesti/" + vesti.ID.ToString()),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                }
                catch (Exception ex)
                {
                    Session["Obavestenje"] = "ERROR:" + ex.Message.ToString();
                }

            if (ModelState.IsValid)
            {
                db.Entry(vesti).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vesti);
        }

        // GET: Vestis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["IDAdmina"] != null)
            {
                Vesti vesti = db.Vestis.Find(id);
                if (vesti == null)
                {
                    return HttpNotFound();
                }
                return View(vesti);
            }
            return RedirectToAction("Index", "Home");
        }

        // POST: Vestis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vesti vesti = db.Vestis.Find(id);
            string path = Server.MapPath(@"~/Imgs/Vesti/" + vesti.ID.ToString());
            Directory.Delete(path, true);
            db.Vestis.Remove(vesti);
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
