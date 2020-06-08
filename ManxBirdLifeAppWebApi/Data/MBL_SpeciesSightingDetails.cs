//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ManxBirdLifeAppWebApi.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class MBL_SpeciesSightingDetails
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MBL_SpeciesSightingDetails()
        {
            this.MBL_SpeciesSightingUploadFile = new HashSet<MBL_SpeciesSightingUploadFile>();
        }
    
        public int SpeciesSightingDetailsID { get; set; }
        public int SightDetailsID { get; set; }
        public int SpeciesID { get; set; }
        public int Number { get; set; }
        public string Approximation { get; set; }
        public bool boolSensitiveSpeciesRecord { get; set; }
        public string Details { get; set; }
        public Nullable<bool> boolOverrideSensitiveSpeciesRecord { get; set; }
    
        public virtual MBL_SightDetails MBL_SightDetails { get; set; }
        public virtual SpeciesLookup SpeciesLookup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MBL_SpeciesSightingUploadFile> MBL_SpeciesSightingUploadFile { get; set; }
    }
}
