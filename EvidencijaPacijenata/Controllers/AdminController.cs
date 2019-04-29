using EvidencijaPacijenata.Models;
using System.Linq;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["IDAdmina"] == null)
                return View("Login");
            else
                return RedirectToAction("Index", "Home");
        }
        public ActionResult Login()
        {
            if (Session["IDAdmina"] == null)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult AdminLogin(string KorisnickoIme, string Lozinka)
        {
            using (DBZUstanovaEntities model = new DBZUstanovaEntities())
            {
                Administrator admin = model.Korisniks.OfType<Administrator>().SingleOrDefault(k => k.KorisnickoIme == KorisnickoIme && k.Lozinka == Lozinka);
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
}