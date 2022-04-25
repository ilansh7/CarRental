using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineRentals
{
    [MetadataType(typeof(EventExtentionMetaData))]
    public partial class EventExtention
    {
        private class EventExtentionMetaData
        {
            public int EventExtentionId { get; set; }

            public string Type { get; set; }

            [Display(Name = "Event Type Description")]
            public string Description { get; set; }

            public string Ext_String_1 { get; set; }

            public string Ext_String_2 { get; set; }

            public decimal Ext_SNumeric_1 { get; set; }

            public DateTime Ext_Date_1 { get; set; }

        }
    }
}