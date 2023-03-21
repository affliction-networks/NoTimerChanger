using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoTimerChanger.Timer
{
    public class TimerParseResult
    {
        public long days;
        public long hours;
        public long minutes;
        public long seconds;

        public TimerParseResult(long d, long h, long m, long s)
        {
            this.days = d;
            this.hours = h;
            this.minutes = m;
            this.seconds = s;
        }
    }
}
