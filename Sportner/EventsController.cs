using RestAPIClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Sportner.Dto;
using Sportner.Messages.EventMessages;
using Sportner.Models;

namespace Sportner
{
    class EventsController
    {
         private Client client = new Client();

         public async Task<List<EventPoint>> GetAllEventsPoints(){
             List<EventWithUserDto> response = await client.GetAllEvents();
             List<EventPoint> eventpoints = new List<EventPoint>();

             foreach (EventWithUserDto eventItem in response)
             {
                 Sports sport = (Sports)eventItem.Event.SportId;
                 PointType type = (PointType)eventItem.Event.EventTypeId;
                 eventpoints.Add(new EventPoint(sport, type, eventItem.Event.EventId, eventItem.Event.StartTime, eventItem.Event.Latitude, eventItem.Event.Longitude, eventItem.User, eventItem.Event));
             }
             return eventpoints;
             //List<EventDto> response = await client.GetAllEvents();
             //List<EventPoint> eventpoints = new List<EventPoint>();
             
             //foreach (EventDto eventItem in response)
             //{
             //    Sports sport = (Sports)eventItem.SportId;
             //    PointType type = (PointType)eventItem.EventTypeId;
             //    eventpoints.Add(new EventPoint(sport, type, eventItem.EventId, eventItem.StartTime, eventItem.Latitude, eventItem.Longitude));
             //}
             //return eventpoints;
         }

         public async Task<HttpStatusCode> AddEvent(EventDto eventIns)
         {
             HttpStatusCode status = await client.AddEvent(eventIns);
             return status;
         }
    }
}
