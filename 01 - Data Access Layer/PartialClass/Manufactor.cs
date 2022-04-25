using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SunshineRentals
{
    [MetadataType(typeof(ManufactorMetaData))]
    public partial class Manufactor
    {
        private class ManufactorMetaData
        {
            public int ManufactorId { get; set; }

            [MaxLength(50, ErrorMessage = "Field exceeded max length (50)")]
            [Required(ErrorMessage = "Missing Manufactor Name")]
            [Display(Name = "Manufactor Name")]
            public string Name { get; set; }

            public virtual ICollection<Vehicle> Vehicles { get; set; }
        }
    }
}

