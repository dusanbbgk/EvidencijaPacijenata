using EvidencijaPacijenata.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class HomeController : Controller
    {
        private DBZUstanovaBetaEntities db = new DBZUstanovaBetaEntities();
        public ActionResult Index()
        {
            if (Session["Obavestenje"] != null)
            {
                ViewBag.Obavestenje = Session["Obavestenje"];
                Session["Obavestenje"] = null;
            }
            return View(db.Vestis.Take(3).OrderByDescending(v => v.DatumObjave).ToList());
        }
        public ActionResult About()
        {
            return View(db.ONamaPodacis.First());
        }

        public ActionResult Contact()
        {
            return View(db.ONamaPodacis.First());
        }
        [HttpPost]
        public ActionResult PacijentLogin(string KorisnickoIme, string Lozinka)
        {
            Lozinka = EncryptPass.EncryptFunc(Lozinka);
            DateTime dt = DateTime.Now;
            DateTime dateOnly = dt.Date;

            var pacijent = db.Korisniks.OfType<Pacijent>().SingleOrDefault(p => p.KorisnickoIme == KorisnickoIme && p.Lozinka == Lozinka);
            if (pacijent != null)
            {
                if (pacijent.Odobren == 0)
                {
                    Session["Obavestenje"] = "Nalog Vam nije odobren!";
                    return RedirectToAction("Index");
                }
                else if (string.Compare(dateOnly.ToString("yyyy-MM-dd"), pacijent.IstekOsiguranja.ToString("yyyy-MM-dd")) > 0)
                {
                    Session["Obavestenje"] = "Osiguranje Vam je isteklo!";
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["IDPacijenta"] = pacijent.ID;
                    Session["ImePrezime"] = pacijent.ImePrezime;
                    return RedirectToAction("Index");
                }
            }
            else
            {
                Session["Obavestenje"] = "Pacijent nije pronađen u bazi!";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult LekarLogin(string KorisnickoIme, string Lozinka)
        {
            Lozinka = EncryptPass.EncryptFunc(Lozinka);
            LekarOpstePrakse LOP = db.Korisniks.OfType<LekarOpstePrakse>().SingleOrDefault(k => k.KorisnickoIme == KorisnickoIme && k.Lozinka == Lozinka);
            if (LOP != null)
            {
                Session["IDLekara"] = LOP.ID;
                Session["ImePrezime"] = LOP.ImePrezime;
                return RedirectToAction("Index");
            }
            else
            {
                LekarSpecijalista LS = db.Korisniks.OfType<LekarSpecijalista>().SingleOrDefault(k => k.KorisnickoIme == KorisnickoIme && k.Lozinka == Lozinka);
                if (LS != null)
                {
                    Session["IDLekara"] = LS.ID;
                    Session["ImePrezime"] = LS.ImePrezime;
                    Session["Specijalizacija"] = LS.Specijalizacija;
                    Session["IDOdeljenjaLekara"] = LS.IDOdeljenja;
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["Obavestenje"] = "Lekar nije pronađen u bazi!";
                    return RedirectToAction("Index");
                }
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
        public ActionResult ResetPassword()
        {
            if (Session["IDPacijenta"] == null && Session["IDLekara"] == null && Session["IDAdmina"] == null)
            {
                if (Session["Obavestenje"] != null)
                {
                    ViewBag.info = Session["Obavestenje"];
                    Session["Obavestenje"] = null;
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ResetPass(string KorisnickoIme, string Email, string Lozinka)
        {
            Lozinka = EncryptPass.EncryptFunc(Lozinka);
            Pacijent proveraPodataka = db.Korisniks.OfType<Pacijent>().SingleOrDefault(p => p.KorisnickoIme == KorisnickoIme && p.Email == Email);
            if (proveraPodataka == null)
            {
                Session["Obavestenje"] = "Korisničko ime i/ili Email adresa nisu pronađeni u bazi";
                return RedirectToAction("Index");
            }
            proveraPodataka.Lozinka = Lozinka;
            db.Entry(proveraPodataka).State = EntityState.Modified;
            db.SaveChanges();
            Session["Obavestenje"] = "Uspešno promenjena lozinka";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Admin()
        {
            if (Session["IDAdmina"] == null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult AdminLogin(string KorisnickoIme, string Lozinka)
        {
            Lozinka = EncryptPass.EncryptFunc(Lozinka);
            Administrator admin = db.Korisniks.OfType<Administrator>().SingleOrDefault(k => k.KorisnickoIme == KorisnickoIme && k.Lozinka == Lozinka);
            if (admin != null)
            {
                Session["IDAdmina"] = admin.ID;
                Session["ImePrezime"] = admin.ImePrezime;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                Session["Obavestenje"] = "Admin nije pronađen u bazi";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}