using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EvidencijaPacijenata.Models
{
    public class SviTermini
    {
        public List<SelectListItem> termini { get; set; }
        public SviTermini() {
            termini = new List<SelectListItem>();
        }
        public void napuniListu() {
            var dateNow = DateTime.Now;
            var pocetak = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 8, 0, 0);
            var kraj = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day, 16, 0, 0);
            while (DateTime.Compare(pocetak, kraj) <= 0)
            {
                termini.Add(new SelectListItem { Text = pocetak.ToString("hh:mm"), Value = pocetak.ToString("hh:mm") });
                pocetak = pocetak.AddMinutes(20);
            }
        }
    }
}