using System.ComponentModel.DataAnnotations;

//namespace CarRental_Website.Areas.Users.Models
namespace SunshineRentals.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Missing ID Number")]
        [StringLength(15, ErrorMessage = "Field exceeded max length (15)")]
        [Display(Name = "ID Number")]
        public string IdNumber { get; set; }

        [Required(ErrorMessage = "Missing First Name")]
        [StringLength(50, ErrorMessage = "Field exceeded max length (50)")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Missing Last Name")]
        [StringLength(50, ErrorMessage = "Field exceeded max length (50)")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Missing Username")]
        [StringLength(50, ErrorMessage = "Field exceeded max length (50)")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Missing Password")]
        [StringLength(50, ErrorMessage = "Field exceeded max length (50)")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(50, ErrorMessage = "Field exceeded max length (50)")]
        [Required(ErrorMessage = "Missing Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match!")]
        public string ConfirmPassword { get; set; }

        [StringLength(100, ErrorMessage = "Field exceeded max length (100)")]
        [RegularExpression("^[a-zA-Z]([a-zA-Z0-9]|.|-|_)+@[a-zA-Z]+(.[a-zA-Z]+){1,2}$", ErrorMessage = "Invalid Email pattern (user@domail.suffix)")]
        [Display(Name = "Email")]
        public string eMail { get; set; }
    }
}