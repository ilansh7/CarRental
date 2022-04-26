using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunshineRentals
{
    public class ResultsForFreeSlot
    {
        public int SlotAvaiable { get; set; }
    }

    public class ResultsForAllEvents
    {
        public int eventId { get; set; }
        public string eventType { get; set; }
        public DateTime eventStartDate  { get; set; }
        public DateTime eventEndDate { get; set; }
        public int eventDuration { get; set; }
        //{ get; set; }
}

    public class EventLogic : BaseLogic
    {
        #region Invitation

        //public IEnumerable<ResultsForFreeSlot> AskForFreeSlot(int enentTypeId, DateTime enentStart)
        public bool AskForFreeSlot(int enentTypeId, DateTime enentStart)
        {
            //DateTime enentEnd = enentStart.AddMinutes(eventDuration);
            //var isSlotAvailable = DB.GetAvailableSlot(enentTypeId, enentStart);
            //var CurEvent = DB.Events.Where(f => f.Start == enentStart && f.DurationInMin == eventDuration)
            //var CurEvent = DB.Events
            //    .Join(DB.EventExtentions, evType => int.Parse(evType.Description),
            //                                exType => exType.EventExtentionId, (evType, exType) => new
            //                                {
            //                                    EventId = evType.EventID,
            //                                    EventSubject = evType.Subject,
            //                                    EventDesc = evType.Description,
            //                                    ExEventType = exType.Ext_String_1,
            //                                    EventStartDate = evType.Start,
            //                                    ExEventDuration = exType.Ext_Numeric_1,//DATEADD(minute, 60 * [Ext_Numeric_1], [Start]) as ,
            //                                    EventDuration = evType.DurationInMin 
            //                                }).Where(x => x.EventDesc != null)
            //                                  .Select(x => new ResultsForAllEvents()
            //                                    {
            //                                      eventId = x.EventId,
            //                                      eventType = x.ExEventType,
            //                                      eventStartDate = x.EventStartDate,
            //                                      eventEndDate = x.EventStartDate,//.AddMinutes(Double.Parse((Decimal)60 * (x.ExEventDuration))),
            //                                      eventDuration = x.EventDuration
            //                                  }).FirstOrDefault();
            //    .Select(f => f.EventID).FirstOrDefault().ToString();

            //var CurEvent = DB.vwAllEvents.ToList();
            bool isSlotAvailable = true;
            int tempp = 0;
            var duration = DB.EventExtentions.Where(x => x.EventExtentionId == enentTypeId).Select(x => x.Ext_Numeric_1).FirstOrDefault().ToString();
            double d;
            Double.TryParse(duration, out d);
            d = 60 * d;
            DateTime eventLegth = enentStart.AddMinutes(d);
            foreach (var item in DB.vwAllEvents.ToList())
            {
                int isOnTheEdge = 0;
                if (item.EventStartDate < enentStart && enentStart < item.EventEndDate)
                {
                    isSlotAvailable = false;
                    break;
                }
                if (item.EventStartDate == enentStart || enentStart == item.EventEndDate)
                {
                    isOnTheEdge++;
                }
                if (item.EventStartDate < eventLegth && eventLegth < item.EventEndDate)
                {
                    isSlotAvailable = false;
                    break;
                }
                if (item.EventStartDate == eventLegth || eventLegth == item.EventEndDate)
                {
                    isOnTheEdge++; 
                }
                //if (!isSlotAvailable)
                //{
                    if (isOnTheEdge < 2)
                    {
                        continue;
                    }
                    else
                    {
                        isSlotAvailable = false;
                        break;
                    }
                //}
            }
            //return CurEvent;// DB.Events.Any(r => r.Subject == carId);
            var sqlParams = new[]{
               new SqlParameter("eventTypeId", enentTypeId),
               new SqlParameter("fromDate", enentStart)
            };
            var slotAvailable = DB.Database.SqlQuery<ResultsForFreeSlot>("GetAvailableSlot @eventTypeId, @fromDate", sqlParams);
            sqlParams = null;
            //DB.Database.SqlQuery.sqlParams = null; ;
            return isSlotAvailable;// string.Empty;
            
            
        }
        //this.Database.SqlQuery<YourEntityType>("storedProcedureName",params);

        #endregion
    }
}
