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
    
    public partial class slobodniTermini_Result
    {
        public int ID { get; set; }
        public int IDPacijenta { get; set; }
        public int IDLekara { get; set; }
        public System.DateTime DatumPregleda { get; set; }
        public System.TimeSpan VremePregleda { get; set; }
        public System.DateTime DatumZakazivanja { get; set; }
        public Nullable<int> ZavrsenPregled { get; set; }
    }
}
