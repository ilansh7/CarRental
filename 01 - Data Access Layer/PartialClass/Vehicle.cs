using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SunshineRentals
{
    [MetadataType(typeof(VehicleMetaData))]
    public partial class Vehicle
    {
        private class VehicleMetaData
        {
            public int TypeId { get; set; }
            public int ManufactorId { get; set; }
            
            [Required(ErrorMessage = "MIssing Model")]
            [Display(Name = "Model Name")]
            [MaxLength(50, ErrorMessage = "Model cannot exceed 50 characters")]
            public string Model { get; set; }

            [Range(1970, 2015, ErrorMessage = "Yaer must be between 1970 to 2015")]
            [RegularExpression(@"^.{4,}$", ErrorMessage = "Field contains 4 digits only")]
            [Display(Name = "Year of manufactor")]
            [Required(ErrorMessage = "Missing Year of manufactor")]
            public int Year { get; set; }
            
            [Display(Name = "Transmission : check for automatic, uncheck for manual")]
            public bool Transmission { get; set; }

            [Required(ErrorMessage = "Missing Daily Rate")]
            [RegularExpression("^[1-9][0-9]*.?[0-9]+$", ErrorMessage = "Invalid number (>0)")]
            [Display(Name = "Daily Rate")]
            public decimal DailyRentalRate { get; set; }

            [Required(ErrorMessage = "Missing Penalty Rate")]
            [RegularExpression("^[1-9][0-9]*.?[0-9]+$", ErrorMessage = "Invalid number (>0) ")]
            [Display(Name = "Penalty Rate")]
            public decimal PenaltyDailyRate { get; set; }

            public virtual ICollection<Fleet> Fleets { get; set; }
            public virtual Manufactor Manufactor { get; set; }
        }
    }
}
