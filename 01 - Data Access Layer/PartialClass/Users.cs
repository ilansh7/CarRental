using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SunshineRentals
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        private class UserMetaData
        {
            public int UserId { get; set; }
            [MaxLength(50, ErrorMessage = "Field exceeded max length (50)")]
            [Required(ErrorMessage = "Missing First Name")]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [MaxLength(50, ErrorMessage = "Field exceeded max length (50)")]
            [Required(ErrorMessage = "Missing Last Name")]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [MaxLength(15, ErrorMessage = "Field exceeded max length (15)")]
            [Required(ErrorMessage = "Missing ID Number")]
            [Display(Name = "ID Number")]
            public string IdNumber { get; set; }

            [MaxLength(50, ErrorMessage = "Field exceeded max length (50)")]
            [Required(ErrorMessage = "Missing User Name")]
            [Display(Name = "User Name")]
            public string UserName { get; set; }

            //[Display(Name = "Manufactor Name")]
            public string Password { get; set; }
            
            [StringLength(100, ErrorMessage = "Field exceeded max length (100)")]
            [RegularExpression("^[a-zA-Z]([a-zA-Z0-9]|.|-|_)+@[a-zA-Z]+(.[a-zA-Z0-9]+){1,2}$", ErrorMessage = "Invalid Email pattern (user@domail.suffix)")]
            [Display(Name = "Email")]
            public string eMail { get; set; }

            [Display(Name = "Birth Date")]
            public Nullable<System.DateTime> BirthDate { get; set; }
    
            public virtual ICollection<Role> Roles { get; set; }

        }
    }
}
