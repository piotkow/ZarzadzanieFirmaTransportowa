//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Firma_Transportowa
{
    using System;
    using System.Collections.Generic;
    
    public partial class konta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public konta()
        {
            this.kierowcy = new HashSet<kierowcy>();
            this.klienci = new HashSet<klienci>();
        }
    
        public int idKonta { get; set; }
        public string email_klienta { get; set; }
        public string login { get; set; }
        public string haslo { get; set; }
        public string rola { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<kierowcy> kierowcy { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<klienci> klienci { get; set; }
    }
}
