using System.ComponentModel.DataAnnotations;

//namespace CarRental_Website.Areas.Users.Models
namespace SunshineRentals.Models
{
    public class LoginViewModel
    {
        //public int UserId { get; set; }

        [Required(ErrorMessage = "Missing User Name")]
        [StringLength(50, ErrorMessage = "Field exceeded max length (50)")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Missing Password")]
        [StringLength(50, ErrorMessage = "Field exceeded max length (50)")]
        [DataType(DataType.Password)]
        [Display(Name = "Remember Me")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}