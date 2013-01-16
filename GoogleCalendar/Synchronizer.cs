using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleCalendar
{
    public class Synchronizer
    {
        private EventControler Controler;
        private CancellationTokenSource TokenSource;
        public delegate void EventUpdated(List<Data.Event> list);

        public event EventUpdated Inform;

        public Synchronizer(EventControler controler)
        {
            TokenSource = new CancellationTokenSource();
            Controler = controler;
        }
        
        /// <summary>
        /// Allow to start the google synchronizer
        /// </summary>
        public void Start()
        {
            CancellationToken ct = TokenSource.Token;
            Task.Factory.StartNew(TaskLoop, ct);
        }

        /// <summary>
        /// Allow to strop the google synchronizer
        /// </summary>
        public void Stop()
        {
            TokenSource.Cancel();
        }

        /// <summary>
        /// The loop of the synchronizer
        /// </summary>
        public async void TaskLoop()
        {
            while (!TokenSource.IsCancellationRequested)
            {
                await Task.Delay(TimeSpan.FromMinutes(5));
                List<Data.Event> list = await Controler.GetAllEvents();
                Inform(list);
            }
        }
    }
}
