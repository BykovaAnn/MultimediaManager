//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MultimediaManager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class FindingType
    {
        public FindingType()
        {
            this.Findings = new HashSet<Finding>();
        }
    
        public int FindingTypeID { get; set; }
        public string FingingTypeName { get; set; }
    
        public virtual ICollection<Finding> Findings { get; set; }
    }
}