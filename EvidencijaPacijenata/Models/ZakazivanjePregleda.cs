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

    public partial class ZakazivanjePregleda
    {
        public int ID { get; set; }
        [DisplayName("Pacijent")]
        public int IDPacijenta { get; set; }
        [DisplayName("Lekar")]
        public int IDLekara { get; set; }
        [DisplayName("Datum pregleda")]
        public System.DateTime DatumPregleda { get; set; }
        [DisplayName("Vreme pregleda")]
        public System.TimeSpan VremePregleda { get; set; }
        [DisplayName("Datum zakazivanja pregleda")]
        public System.DateTime DatumZakazivanja { get; set; }
        public Nullable<int> ZavrsenPregled { get; set; }
        [DisplayName("Lekar")]
        public virtual Korisnik Korisnik { get; set; }
        [DisplayName("Pacijent")]
        public virtual Korisnik Korisnik1 { get; set; }
    }
}
