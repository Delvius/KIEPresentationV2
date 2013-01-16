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
    /// <summary>
    /// Classe permettant de gérer les calendriers
    /// </summary>
    public class CalendarControler
    {

        private CalendarService Service = CalendarService.Instance;
        
        /// <summary>
        /// La liste de calendrier
        /// </summary>
        private List<CalendarListEntry> mCalendars;

        /// <summary>
        /// Constructeur
        /// </summary>
        public CalendarControler()
        {
        }

        public IEnumerable<CalendarListEntry> Calendars
        {
            get
            {
                return mCalendars;
            }
        }

        /// <summary>
        /// Permet de retourner tous les calendriers
        /// </summary>
        /// <returns>La liste des calendriers</returns>
        public async Task<List<CalendarListEntry>> GetAllCalendars()
        {
            Stream result = await Service.Get("https://www.googleapis.com/calendar/v3/users/me/calendarList");
            mCalendars = new List<CalendarListEntry>();
            while (result != null)
            {
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CalendarList));
                CalendarList calendarList = (CalendarList)deserializer.ReadObject(result);
                mCalendars.AddRange(calendarList.Items.Where(cal => cal.AccessRole == "owner"));
               
                if(String.IsNullOrEmpty(calendarList.NextPageToken))
                    break;
                result = await Service.Get("https://www.googleapis.com/calendar/v3/users/me/calendarList?pageToken=" + calendarList.NextPageToken + "?key=AIzaSyB0c32CnNAjYQegkIBRS-QXhF2i9NxH4CM");
            }
            return mCalendars;
        }

        /// <summary>
        /// Permet d'insérer un calendrier
        /// </summary>
        /// <param name="calendar">Le calendrier qu'on doit insérer.</param>
        public async Task<Calendar> InsertCalendar(CalendarListEntry calendar)
        {
            Calendar simplecal = calendar;

            StreamContent calendarJson = new StreamContent(Service.Serialize(simplecal));
            calendarJson.Headers.Add("Content-Type", "application/json;charset=utf-8");
            Stream result = await Service.Post("https://www.googleapis.com/calendar/v3/calendars", calendarJson);
            if (result != null)
            {
                DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CalendarListEntry));
                CalendarListEntry cal = (CalendarListEntry)deserializer.ReadObject(result);
               // mCalendars.Add(cal);
                return cal;
            }
            return null;
        }

        /// <summary>
        /// Permet de mettre à jour un calendrier.
        /// </summary>
        /// <param name="calendar">Le calendrier avec les modifications à appliquer.</param>
        public async Task<Calendar> UpdateCalendar(CalendarListEntry calendar)
        {
            Calendar simplecal = calendar;


            if (!String.IsNullOrEmpty(simplecal.Id))
            {
                StreamContent calendarJson = new StreamContent(Service.Serialize(simplecal));
                calendarJson.Headers.Add("Content-Type", "application/json;charset=utf-8");

                //Stream result = await Service.Put("https://www.googleapis.com/calendar/v3/users/me/calendarList/" + calendar.Id, calendarJson);

                Stream result = await Service.Put("https://www.googleapis.com/calendar/v3/calendars/" + calendar.Id, calendarJson);
               
                
                if (result != null)
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CalendarListEntry));
                    CalendarListEntry cal = (CalendarListEntry)deserializer.ReadObject(result);
                  //  mCalendars.Remove(mCalendars.Single(c => c.Id == calendar.Id));
                   // mCalendars.Add(cal);
                    return cal;
                }
            }
            return null;
        }

        /// <summary>
        /// Permet de supprimer un calendrier 
        /// </summary>
        /// <param name="calendar">Le calendrier à supprimer</param>
        public async void DeleteCalendar(CalendarListEntry calendar)
        {
            if (!String.IsNullOrEmpty(calendar.Id))
            {
                StreamContent calendarJson = new StreamContent(Service.Serialize(calendar));
                calendarJson.Headers.Add("Content-Type", "application/json;charset=utf-8");
                Stream result = await Service.Delete("https://www.googleapis.com/calendar/v3/users/me/calendarList/"+calendar.Id);
                if (result != null)
                {
                   // mCalendars.Remove(mCalendars.Single(cal => calendar.Id == cal.Id));
                }
            }
            
        }

        /// <summary>
        /// Permet de connaître un calendrier en fonction de son id
        /// </summary>
        /// <param name="id">L'id du calendrier que l'on veut</param>
        /// <returns>Le calendrier souhaité</returns>
        public async Task<CalendarListEntry> GetCalendar(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                Stream result = await Service.Get("https://www.googleapis.com/calendar/v3/users/me/calendarList/" + id);
                if (result != null)
                {
                    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(CalendarListEntry));
                    CalendarListEntry cal = (CalendarListEntry)deserializer.ReadObject(result);
                    return cal;
                }
            }
            return null;
        }
    }
}
