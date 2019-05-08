//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EvidencijaPacijenata.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class Pacijent : Korisnik
    {
        [Required(ErrorMessage = "Polje je obavezno")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "Nije u dobrom formatu")]
        [Remote("ProveriJMBG", "Pacijents", HttpMethod = "POST", ErrorMessage = "Postoji u bazi")]
        public string JMBG { get; set; }
        [DisplayName("Nosilac osiguranja")]
        public string NosilacOsiguranja { get; set; }
        [DisplayName("Srodstvo sa nosiocem osiguranja")]
        public string SrodstvoSaNosiocem { get; set; }
        public Nullable<int> IDOdeljenja { get; set; }
        public Nullable<int> IDUstanove { get; set; }
        [Required(ErrorMessage = "Polje je obavezno")]
        [DisplayName("Krvna grupa")]
        public string KrvnaGrupa { get; set; }
        [Required(ErrorMessage = "Polje je obavezno")]
        public string Pol { get; set; }
        [Required(ErrorMessage = "Polje je obavezno")]
        public string Adresa { get; set; }
        [Required(ErrorMessage = "Polje je obavezno")]
        [RegularExpression(@"^(06(\d{7,8})|(\d\/\d{6,7})|(\d\/\d{3}-\d{3,4})|(\d-\d{6,7})|(\d\-\d{3}-\d{3,4}))|(01(\d{7,8})|(\d\/\d{6,7})|(\d\/\d{3}-\d{3,4})|(\d-\d{6,7})|(\d\-\d{3}-\d{3,4}))$", ErrorMessage = "Nije dobar format")]
        public string Telefon { get; set; }
        [Required(ErrorMessage = "Polje je obavezno")]
        [EmailAddress(ErrorMessage = "E-mail adresa nije validna")]
        [Remote("ProveriEmail", "Pacijents", HttpMethod = "POST", ErrorMessage = "Postoji u bazi")]
        public string Email { get; set; }
        [DisplayName("Osiguranje")]
        public System.DateTime IstekOsiguranja { get; set; }
        public int Odobren { get; set; }

        public virtual Odeljenje Odeljenje { get; set; }
        public virtual Ustanova Ustanova { get; set; }
    }
}
