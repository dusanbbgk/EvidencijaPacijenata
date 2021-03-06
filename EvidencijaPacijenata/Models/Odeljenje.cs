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

    public partial class Odeljenje
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Odeljenje()
        {
            this.Lekars = new HashSet<Lekar>();
            this.Pacijents = new HashSet<Pacijent>();
            this.Uputs = new HashSet<Uput>();
        }

        public int ID { get; set; }
        public int IDUstanove { get; set; }
        [DisplayName("Naziv odeljenja")]
        public string Naziv { get; set; }
        [DisplayName("Slobodnih mesta na odeljenju")]
        public int SlobodnihMesta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lekar> Lekars { get; set; }
        public virtual Ustanova Ustanova { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pacijent> Pacijents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Uput> Uputs { get; set; }
    }
}
