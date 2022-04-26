using System;
using System.ComponentModel.DataAnnotations;

//namespace CarRental_Website.Areas.Users.Models
namespace SunshineRentals.Models
{
    public enum dogBreed
    {
        [Display(Name = "Afghan Hound")]
        Afghan_Hound,
        Akita,
        [Display(Name = "American Bulldog")]
        American_Bulldog,
        [Display(Name = "American Pit Bull Terrier")]
        American_Pit_Bull_Terrier,
    }

    public enum timeFrame
    {
        _09a00,
        _10a00,
        _11a00,
        _12a00,
        _13a00,
        _14a00,
        _15a00,
        _16a00,
    }

    public enum eventTypes
    {
        AntiFleas_treatment,
        Hair_Cut,
        Franch_manicure,
        Nails_Trim,
    }

    public struct ConvertEnum
    {
        public int Value { get; set; }
        public String Text { get; set; }
    }

    public class EventViewModel
    {
        public int EventID { get; set; }

        public int UserId { get; set; }

        public string Subject { get; set; }

        //[Required(ErrorMessage = "MIssing Model")]
        //[MaxLength(100, ErrorMessage = "Model cannot exceed 50 characters")]
        public string Description { get; set; }

        //[Range(1970, 2015, ErrorMessage = "Yaer must be between 1970 to 2015")]
        //[RegularExpression(@"^.{4,}$", ErrorMessage = "Field contains 4 digits only")]
        public DateTime Start { get; set; }

        //public DateTime End { get; set; }
        public int DurationInMin { get; set; }

        public string ThemeColor { get; set; }

        public bool IsFullDay { get; set; }
        //public virtual Event Events { get; set; }

        public dogBreed DogBreed { get; set; }

        public timeFrame TimeFrame { get; set; }

        //public string EventType { get; set; }
    }
}