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
    
    public partial class LocationLookup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LocationLookup()
        {
            this.MBL_SightDetails = new HashSet<MBL_SightDetails>();
        }
    
        public int LocationID { get; set; }
        public string Name { get; set; }
        public string GridReference { get; set; }
        public Nullable<bool> boolManuallyAddedRecord { get; set; }
        public Nullable<bool> boolApprovedByAdmin { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MBL_SightDetails> MBL_SightDetails { get; set; }
    }
}