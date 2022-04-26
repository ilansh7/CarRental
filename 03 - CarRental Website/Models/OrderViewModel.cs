using System;
using System.ComponentModel.DataAnnotations;


namespace SunshineRentals.Models
{
    public class OrderViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        //[Required(ErrorMessage = "Missing Start Date")]
        //[StringLength(50, ErrorMessage = "Al Tagzim")]
        [Display(Name = "From Date")]
        public DateTime DateFrom { get; set; }

        //[Required(ErrorMessage = "Missing End Date")]
        //[StringLength(50, ErrorMessage = "Al Tagzim")]
        [Display(Name = "To Date")]
        public DateTime DateTo { get; set; }

        [Display(Name = "Rate / Day")]
        public decimal DayRate { get; set; }

        [Display(Name = "Overdue Rate")]
        public decimal OverdueRate { get; set; }

        [Display(Name = "Total Estimated Cost")]
        public decimal EstimatedCost { get; set; }//= DayRate * (DateTo - DateTo);

        public string Manufactor { get; set; }
        public string Model { get; set; }
        public string ImageUrl { get; set; }
        public int FleetId { get; set; }
        public string LicencePlate { get; set; }
        public int OrderNum { get; set; }
    }
   
}