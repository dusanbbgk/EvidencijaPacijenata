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
        string encryptpw;
        public ActionResult Index()
        {
            if (Session["UspesnaRegistracija"] != null) {
                ViewBag.registracija = Session["UspesnaRegistracija"];
                Session["UspesnaRegistracija"] = null;
            }
            if (TempData["NemaPregleda"] != null)
            {
                ViewBag.NemaPregleda = TempData["NemaPregleda"];
                TempData.Clear();
            }
            if (TempData["OdeljenjePuno"] != null)
            {
                ViewBag.OdeljenjePuno = TempData["OdeljenjePuno"];
                TempData.Clear();
            }
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
            if (Session["NemaKarton"] != null)
            {
                ViewBag.NemaKarton = Session["NemaKarton"];
                Session["NemaKarton"] = null;
            }
            return View(db.Vestis.Take(3).OrderByDescending(v => v.DatumObjave).ToList());
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
        [HttpPost]
        public ActionResult PacijentLogin(string KorisnickoIme, string Lozinka)
        {
            encryption(Lozinka);
            Lozinka = encryptpw;
            DateTime dt = DateTime.Now;
            DateTime dateOnly = dt.Date;

            var pacijent = db.Korisniks.OfType<Pacijent>().SingleOrDefault(p => p.KorisnickoIme == KorisnickoIme && p.Lozinka == Lozinka);
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
        private void encryption(string lozinka)
        {
            string strmsg = String.Empty;
            byte[] encode = new byte[lozinka.Length];
            encode = Encoding.UTF8.GetBytes(lozinka);
            strmsg = Convert.ToBase64String(encode);
            encryptpw = strmsg;
        }

        [HttpPost]
        public ActionResult LekarLogin(string KorisnickoIme, string Lozinka)
        {
            encryption(Lozinka);
            Lozinka = encryptpw;
            LekarOpstePrakse LOP = db.Korisniks.OfType<LekarOpstePrakse>().SingleOrDefault(k => k.KorisnickoIme == KorisnickoIme && k.Lozinka == Lozinka);
            if (LOP != null)
            {
                Session["IDLekara"] = LOP.ID;
                Session["ImePrezime"] = LOP.Ime + " " + LOP.Prezime;
                return RedirectToAction("Index");
            }
            else
            {
                LekarSpecijalista LS = db.Korisniks.OfType<LekarSpecijalista>().SingleOrDefault(k => k.KorisnickoIme == KorisnickoIme && k.Lozinka == Lozinka);
                if (LS != null)
                {
                    Session["IDLekara"] = LS.ID;
                    Session["ImePrezime"] = LS.Ime + " " + LS.Prezime;
                    Session["Specijalizacija"] = LS.Specijalizacija;
                    Session["IDOdeljenjaLekara"] = LS.IDOdeljenja;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["info"] = "Lekar nije pronađen u bazi!";
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
                if (TempData["info"] != null)
                {
                    ViewBag.info = TempData["info"];
                    TempData["info"] = null;
                }
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult ResetPass(string KorisnickoIme, string Email, string Lozinka)
        {
            encryption(Lozinka);
            Lozinka = encryptpw;
            Pacijent proveraPodataka = db.Korisniks.OfType<Pacijent>().SingleOrDefault(p => p.KorisnickoIme == KorisnickoIme && p.Email == Email);
            if (proveraPodataka == null)
            {
                TempData["info"] = "Korisničko ime i/ili Email adresa nisu pronađeni u bazi!";
                return RedirectToAction("Index");
            }
            proveraPodataka.Lozinka = Lozinka;
            db.Entry(proveraPodataka).State = EntityState.Modified;
            db.SaveChanges();
            Session["resetPass"] = "Uspešno promenjena lozinka!";
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
            encryption(Lozinka);
            Lozinka = encryptpw;
            Administrator admin = db.Korisniks.OfType<Administrator>().SingleOrDefault(k => k.KorisnickoIme == KorisnickoIme && k.Lozinka == Lozinka);
            if (admin != null)
            {
                Session["IDAdmina"] = admin.ID;
                Session["ImePrezime"] = admin.Ime + " " + admin.Prezime;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["info"] = "Admin nije pronađen u bazi!";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}