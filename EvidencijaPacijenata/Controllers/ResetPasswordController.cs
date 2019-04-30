using EvidencijaPacijenata.Models;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Controllers
{
    public class ResetPasswordController : Controller
    {
        // GET: ResetPassword
        public ActionResult Index()
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
        public ActionResult CheckForm(Pacijent pacijent)
        {
            using (DBZUstanovaEntities model = new DBZUstanovaEntities())
            {
                Pacijent proveraPodataka = model.Korisniks.OfType<Pacijent>().SingleOrDefault(p => p.KorisnickoIme == pacijent.KorisnickoIme && p.Email == pacijent.Email);
                if (proveraPodataka == null)
                {
                    TempData["info"] = "Korisničko ime i/ili Email adresa nisu pronađeni u bazi!";
                    return RedirectToAction("Index");
                }
                else
                {
                    proveraPodataka.Lozinka = pacijent.Lozinka;
                    if (ModelState.IsValid)
                    {
                        model.Entry(proveraPodataka).State = EntityState.Modified;
                        model.SaveChanges();
                        Session["resetPass"] = "Uspešno promenjena lozinka!";
                        return RedirectToAction("Index", "Home");
                    }
                    TempData["info"] = "Greška prilikom ažuriranja pacijenta!";
                    return RedirectToAction("Index");
                }
            }
        }
    }

}