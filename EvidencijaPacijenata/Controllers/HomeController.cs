using EvidencijaPacijenata.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["resetPass"] != null)
            {
                ViewBag.resetPass = Session["resetPass"];
                Session["resetPass"] = null;
            }
            if (TempData["info"] != null)
            {
                ViewBag.info = TempData["info"];
                TempData.Clear();
            }
            return View();
        }
        [HttpPost]
        public ActionResult PacijentLogin(string KorisnickoIme, string Lozinka)
        {
            DateTime dt = DateTime.Now;
            DateTime dateOnly = dt.Date;
            using (DBZUstanovaEntities model = new DBZUstanovaEntities())
            {
                var pacijent = model.Korisniks.OfType<Pacijent>().SingleOrDefault(p => p.KorisnickoIme == KorisnickoIme && p.Lozinka == Lozinka);
                if (pacijent != null)
                {
                    if (pacijent.Odobren == 0)
                    {
                        TempData["info"] = "Nalog Vam nije odobren!";
                        return RedirectToAction("Index");
                    }
                    else if (string.Compare(dateOnly.ToString("yyyy-MM-dd"), pacijent.IstekOsiguranja.ToString("yyyy-MM-dd")) > 0)
                    {
                        TempData["info"] = "Osiguranje Vam je isteklo!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Session["IDPacijenta"] = pacijent.ID;
                        Session["ImePrezime"] = pacijent.Ime + " " + pacijent.Prezime;
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["info"] = "Pacijent nije pronađen u bazi!";
                    return RedirectToAction("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult LekarLogin(string KorisnickoIme, string Lozinka) {
            using (DBZUstanovaEntities model = new DBZUstanovaEntities())
            {
                LekarOpstePrakse LOP = model.Korisniks.OfType<LekarOpstePrakse>().SingleOrDefault(k => k.KorisnickoIme == KorisnickoIme && k.Lozinka == Lozinka);
                if (LOP != null)
                {
                    Session["IDLekara"] = LOP.ID;
                    return RedirectToAction("Index");
                }
                else
                {
                    LekarSpecijalista LS = model.Korisniks.OfType<LekarSpecijalista>().SingleOrDefault(k => k.KorisnickoIme == KorisnickoIme && k.Lozinka == Lozinka);
                    if (LS != null)
                    {
                        Session["IDLekara"] = LS.ID;
                        Session["Specijalizacija"] = LS.Specijalizacija;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["info"] = "Lekar nije pronađen u bazi!";
                        return RedirectToAction("Index");
                    }
                }
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}