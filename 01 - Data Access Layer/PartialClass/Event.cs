using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SunshineRentals
{
    
    [MetadataType(typeof(EventMetaData))]
    public partial class Event
    {
        private class EventMetaData
        {
            public int EventID { get; set; }

            public int UserId { get; set; }

            public string Subject { get; set; }

            [Display(Name = "Event Description")]
            public string Description { get; set; }

            [Display(Name = "Event Starting at")]
            //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm}")]
            public DateTime Start { get; set; }

            //[Display(Name = "Event Ends")]
            //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
            //public DateTime End { get; set; }

            public int DurationInMin { get; set; }

            public string ThemeColor { get; set; }

            [Display(Name = "Full day appointment")]
            public bool IsFullDay { get; set; }

            //public virtual ICollection<Event> Events { get; set; }

            //public string EventType { get; set; }
        }
    }
}
