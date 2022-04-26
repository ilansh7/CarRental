using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SunshineRentals
{
    
    [MetadataType(typeof(EventViewMetaData))]
    public partial class EventView
    {
        private class EventViewMetaData
        {
            public int EventId { get; set; }

            public string EventSubject { get; set; }

            public string EventType { get; set; }

            public DateTime EventStartDate { get; set; }

            public DateTime EventEndDate { get; set; }

            public int EventDuration { get; set; }
        }
    }
}
