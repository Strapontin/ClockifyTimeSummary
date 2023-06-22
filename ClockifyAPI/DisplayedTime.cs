using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockifyAPI
{
    internal class DisplayedTime
    {
        public DisplayedTime() { _time = new(); }

        public DisplayedTime(string description, TimeSpan time)
        {
            Description = description;
            _time = time;
        }


        public string Description { get; set; }

        private TimeSpan _time;
        public TimeSpan Time
        {
            get { return _time; }
        }


        public void AddTime(TimeSpan timeSpan)
        {
            _time = Time.Add(timeSpan);
        }
    }
}
