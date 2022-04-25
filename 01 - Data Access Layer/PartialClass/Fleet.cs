using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SunshineRentals
{
    [MetadataType(typeof(FleetMetaData))]
    public partial class Fleet
    {
        private class FleetMetaData
        {

            public int FleetId { get; set; }

            [MaxLength(20, ErrorMessage = "Licence Plate cannot exceed 20 characters")]
            [Required(ErrorMessage = "Missing Licence Plate")]
            public string LicencePlate { get; set; }

            [Required(ErrorMessage = "Missing Car Model")]
            public int TypeId { get; set; }

            [RegularExpression("^[0-9]+$", ErrorMessage = "Invalid number")]
            [Required(ErrorMessage = "Missing Kilometrage")]
            public int Kilometrage { get; set; }

            [Required(ErrorMessage = "Missing Image")]
            public string Image { get; set; }

            [Display(Name = "Ready To Use")]
            //[Required(ErrorMessage = "Missing Ready To Use Falg")]
            public bool ReadyToUse { get; set; }

            //public virtual Vehicle Vehicle { get; set; }
            public virtual ICollection<Rental> Rentals { get; set; }
        }
    }
}
