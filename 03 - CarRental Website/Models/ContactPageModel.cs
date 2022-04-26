using System.ComponentModel.DataAnnotations;

//namespace CarRental_Website.Areas.Users.Models
namespace SunshineRentals.Models
{
    public class ContactPageModel
    {
        [Required(ErrorMessage = "Missing User Name")]
        [StringLength(50, ErrorMessage = "Field exceeded max length (50)")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Missing Phone")]
        [StringLength(12, ErrorMessage = "Field exceeded max length (12)")]
        [RegularExpression("^0[0-9]{1,2}-[2-9][0-9]{6}$", ErrorMessage = "Invalid phone pattern (0X(X)-XXXXXXX)")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Missing Email")]
        [StringLength(100, ErrorMessage = "Field exceeded max length (100)")]
        //[RegularExpression("^[a-zA-Z]([a-zA-Z0-9]|.|-|_)+@[a-zA-Z]+(._-[a-zA-Z0-9]+){1,}$", ErrorMessage = "Invalid Email pattern (user@domail.suffix)")]
        [RegularExpression("^[a-zA-Z]([a-zA-Z0-9]|.|-|_)+@[a-zA-Z]+(.[a-zA-Z]+){1,2}$", ErrorMessage = "Invalid Email pattern (user@domail.suffix)")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Details of request")]
        public string Notes { get; set; }

        public int SuccessSend { get; set; }
        public string Error { get; set; }

        public string Year { get; set; }
    }
}