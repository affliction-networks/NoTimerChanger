using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoTimerChanger.Timer
{
    public class CountdownDuration
    {
        public long Seconds { get; private set; }

        public bool Tick()
        {
            TimeSpan Time = TimeSpan.FromSeconds(Seconds);
            
            if (Time.Days == 0 && Time.Hours == 0 && Time.Minutes == 0 && Time.Seconds == 0)
            {
                return false;
            }
            Seconds--;

            return true;
        }

        public string GetOutput()
        {
            TimeSpan Time = TimeSpan.FromSeconds(Seconds);
            return string.Format("{0}d {1}h {2}m {3}s", Time.Days, Time.Hours, Time.Minutes, Time.Seconds);
        }

        public static CountdownDuration Parse(long secondsToGo)
        {
            return new CountdownDuration
            {
                Seconds = secondsToGo
            };
        }
    }
}
