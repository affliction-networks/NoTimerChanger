using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoTimerChanger.Timer
{
    class TimerUtils
    {
        public TimerParseResult parseTime(String result)
        {
            long days = 0;
            long hours = 0;
            long minutes = 0;
            long seconds = 0;
            if (result.Contains("d"))
            {
                bool done = false;
                int daysIndex = result.LastIndexOf("d");
                int index = result.LastIndexOf("d") - 1;
                int indexFirst = index;
                while (!done)
                {
                    if (indexFirst < 0)
                    {
                        indexFirst = 0;
                        break;
                    }
                    char c = result[indexFirst];
                    if (!Regex.IsMatch(c.ToString(), "^[0-9]*$"))
                    {
                        done = true;
                        break;
                    }
                    indexFirst--;
                }
                string daysString = result.MySubString(indexFirst, daysIndex);
                try
                {
                    days = long.Parse(daysString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to parse those days... You fucked up somehow");
                }
            }
            if (result.Contains("h"))
            {
                bool done = false;
                int hoursIndex = result.LastIndexOf("h");
                int index = result.LastIndexOf("h") - 1;
                int indexFirst = index;
                while (!done)
                {
                    if (indexFirst < 0)
                    {
                        indexFirst = 0;
                        break;
                    }
                    char c = result[indexFirst];
                    if (!Regex.IsMatch(c.ToString(), "^[0-9]*$"))
                    {
                        done = true;
                        break;
                    }
                    indexFirst--;
                }
                string hoursString = result.MySubString(indexFirst, hoursIndex);
                try
                {
                    hours = long.Parse(hoursString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to parse those hours... You fucked up somehow");
                }
            }
            if (result.Contains("m"))
            {
                bool done = false;
                int minutesIndex = result.LastIndexOf("m");
                int index = result.LastIndexOf("m") - 1;
                int indexFirst = index;
                while (!done)
                {
                    if (indexFirst < 0)
                    {
                        indexFirst = 0;
                        break;
                    }
                    char c = result[indexFirst];
                    if (!Regex.IsMatch(c.ToString(), "^[0-9]*$"))
                    {
                        done = true;
                        break;
                    }
                    indexFirst--;
                }
                string minutesString = result.MySubString(indexFirst, minutesIndex);
                try
                {
                    minutes = long.Parse(minutesString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to parse those minutes... You fucked up somehow");
                }
            }
            if (result.Contains("s"))
            {
                bool done = false;
                int secondsIndex = result.LastIndexOf("s");
                int index = result.LastIndexOf("s") - 1;
                int indexFirst = index;
                while (!done)
                {
                    if (indexFirst < 0)
                    {
                        indexFirst = 0;
                        break;
                    }
                    char c = result[indexFirst];
                    if (!Regex.IsMatch(c.ToString(), "^[0-9]*$"))
                    {
                        done = true;
                        break;
                    }
                    indexFirst--;
                }
                string secondsString = result.MySubString(indexFirst, secondsIndex);
                try
                {
                    seconds = long.Parse(secondsString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to parse those seconds... You fucked up somehow");
                }
            }
            return new TimerParseResult(days, hours, minutes, seconds);
        }
    }
}
