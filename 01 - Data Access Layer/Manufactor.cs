//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SunshineRentals
{
    using System;
    using System.Collections.Generic;
    
    public partial class Manufactor
    {
        public Manufactor()
        {
            this.Vehicles = new HashSet<Vehicle>();
        }
    
        public int ManufactorId { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<Vehicle> Vehicles { get; set; }
    }
}
