using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockifyAPI
{
    internal class DisplayedTime
    {
        public DisplayedTime() { _totalTime = new(); }

        public DisplayedTime(string description, TimeSpan time)
        {
            Description = description;
            _totalTime = time;
        }


        public string Description { get; set; }
        public string TaskName { get; set; }
        public string ProjectName { get; set; }

        private TimeSpan _totalTime;
        public TimeSpan TotalTime
        {
            get { return _totalTime; }
        }


        public void AddTime(TimeSpan timeSpan)
        {
            _totalTime = TotalTime.Add(timeSpan);
        }
    }
}
