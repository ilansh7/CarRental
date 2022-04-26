using System;
using System.ComponentModel.DataAnnotations;

namespace SunshineRentals.Models
{
    //[MetadataType(typeof(VehicleMetaData))]
    public class RentalViewModel
    {
        [Display(Name = "Order Number")]
        public int RentalId { get; set; }

        public int FleetId { get; set; }
        public int UserId { get; set; }
        [Display(Name = "Rental Start Date")]
        public System.DateTime StartRentalDate { get; set; }

        [Display(Name = "Rental End Date")]
        public System.DateTime EndRentalDate { get; set; }

        [Display(Name = "Rental Actual End Date")]
        public Nullable<System.DateTime> ActualEndRental { get; set; }

        public virtual Fleet Fleet { get; set; }

        public string FullUserName { get; set; }
    }
}
