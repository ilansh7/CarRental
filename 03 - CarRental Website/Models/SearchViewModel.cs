using System.ComponentModel.DataAnnotations;

namespace SunshineRentals.Models
{
    public class SearchViewModel
    {
        public string User { get; set; }
        [Display(Name = "Transmission")]
        public int Transmission { get; set; }

        [Display(Name = "Year")]
        public int Year { get; set; }

        [Display(Name = "Mamufactor Name")]
        public string MamufId { get; set; }

        [Display(Name = "Model Name")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Date is Required.")]
        [Display(Name = "From Date")]
        public string FromDt { get; set; }

        [Required(ErrorMessage = "Date is Required.")]
        [Display(Name = "To Date")]
        public string ToDt { get; set; }

        [Display(Name = "Special Requests")]
        public string Keywords { get; set; }
    }
}