using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockifyAPI
{
    internal class DisplayedTime
    {
        public DisplayedTime() 
        {
            if (_totalTime == null)
            {
                _totalTime = new();
            }

            TaskName = String.Empty;
            ProjectName = String.Empty;
        }

        public DisplayedTime(TimeSpan time):base()
        {
            _totalTime = time;
        }


        public string TaskName { get; set; }
        public string ProjectName { get; set; }

        private TimeSpan _totalTime;
        public TimeSpan TotalTime
        {
            get { return _totalTime; }
        }

        private TimeSpan _intervalTime;
        public TimeSpan IntervalTime
        {
            set { _intervalTime = value; }
            get { return _intervalTime; }
        }


        public void AddTotalTime(TimeSpan timeSpan)
        {
            _totalTime = TotalTime.Add(timeSpan);
        }
    }
}
