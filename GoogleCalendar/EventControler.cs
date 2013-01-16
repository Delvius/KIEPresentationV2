using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using GoogleCalendar.Data;

namespace GoogleCalendar
{
    public class EventControler
    {
        private CalendarService Service = CalendarService.Instance;
        
        /// <summary>
        /// La liste d'event du calendrier
        /// </summary>
        private List<Event> mEvents;

        /// <summary>
        /// Le calandrier
        /// </summary>
        public CalendarListEntry Calendar { get; private set; }

        public EventControler(CalendarListEntry calendar)
        {
            Calendar = calendar;
        }


        public List<Event> Events
        {
            get
            {
                return mEvents;
            }
        }


        /// <summary>
        /// Permet de retourner tous les events d'un calendrier
        /// </summary>
        /// <returns>La liste des events du calendrier</returns>
        public async Task<List<Event>> GetAllEvents()
        {
            mEvents = new List<Event>();
            if (!String.IsNullOrEmpty(Calendar.Id))
            {
                var result = await Service.Get("https://www.googleapis.com/calendar/v3/calendars/" + Calendar.Id + "/events");
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Events));
                mEvents = new List<Event>();
                while (result != null)
                {
                    Events events = (Events)deserializer.ReadObject(result);
                    if (events.Items != null) 
                        mEvents.AddRange(events.Items);
                    if (String.IsNullOrEmpty(events.NextPageToken))
                        return mEvents;
                    result = await Service.Get("https://www.googleapis.com/calendar/v3/calendars/" + Calendar.Id + "/events?pageToken=" + events.NextPageToken);
                }
            }
            return mEvents;
        }

        /// <summary>
        /// Permet d'insérer un event dans un calendrier
        /// </summary>
        /// <param name="evt">L'event qu'on doit insérer.</param>
        public async Task<Event> InsertEvent(Event evt)
        {
            if (!String.IsNullOrEmpty(Calendar.Id))
            {
                StreamContent eventJson = new StreamContent(Service.Serialize(evt));
                eventJson.Headers.Add("Content-Type", "application/json;charset=utf-8");
                Task<string> test = eventJson.ReadAsStringAsync();
                Stream result = await Service.Post("https://www.googleapis.com/calendar/v3/calendars/" + Calendar.Id + "/events", eventJson);
                if (result != null)
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Event));
                    Event evtret = (Event)deserializer.ReadObject(result);
                    mEvents.Add(evtret);
                    return evtret;
                }
            }
            return null;
        }

        /// <summary>
        /// Permet de mettre à jour un event.
        /// </summary>
        /// <param name="evt">Le event avec les modifications à appliquer.</param>
        public async Task<Event> UpdateEvent(Event evt)
        {
            if (!String.IsNullOrEmpty(Calendar.Id) && !String.IsNullOrEmpty(evt.Id))
            {
                StreamContent eventJson = new StreamContent(Service.Serialize(evt));
                eventJson.Headers.Add("Content-Type", "application/json;charset=utf-8");
                Stream result = await Service.Put("https://www.googleapis.com/calendar/v3/calendars/" + Calendar.Id + "/events/" + evt.Id, eventJson);
                if (result != null)
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Event));
                    Event evtret = (Event)deserializer.ReadObject(result);
                    mEvents.Remove(mEvents.Single(e => e.Id == evt.Id));
                    mEvents.Add(evtret);
                    return evtret;
                }
            }
            return null;
        }

        /// <summary>
        /// Permet de supprimer un event 
        /// </summary>
        /// <param name="evt">L'event à supprimer</param>
        public async void DeleteEvent(Event evt)
        {
            if (!String.IsNullOrEmpty(Calendar.Id) && !String.IsNullOrEmpty(evt.Id))
            {
                Stream result = await Service.Delete("https://www.googleapis.com/calendar/v3/calendars/" + Calendar.Id + "/events/" + evt.Id);
                if (result != null)
                {
                    //mEvents.Remove(mEvents.Single(e => evt.Id == e.Id));
                }
            }
        }

        /// <summary>
        /// Permet récupérer un event via son id
        /// </summary>
        /// <param name="id">L'id de l'event que l'on veut</param>
        /// <returns>L'event souhaité</returns>
        public async Task<Event> GetEvent(string id)
        {
            if (!String.IsNullOrEmpty(id) && ! String.IsNullOrEmpty(Calendar.Id))
            {
                Stream result = await Service.Get("https://www.googleapis.com/calendar/v3/calendars/"+Calendar.Id+"/events/" + id);
                if (result != null)
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(Event));
                    Event evt = (Event)deserializer.ReadObject(result);
                    return evt;
                }
            }
            return null;
        }
    }
}
