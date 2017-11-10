using System;
using System.Collections.Generic;

using Computer.API;

namespace ModuleTime
{
    public class ModuleTime : ModuleBase
    {
        [Setting]
        private string formatTime = @"hh:mm t \M";
        [Setting]
        private string formatDate = @"dddd, MMMM d\\\s";
        public ModuleTime()
        {
            
        }

        public string GetTimeAsString(string format = "")
        {
            if (string.IsNullOrEmpty(format))
            {
                format = this.formatTime;
            }
            return DateTime.Now.ToString(format);
        }
        public string GetDateAsString(string format = "")
        {
            if (string.IsNullOrEmpty(format))
            {
                format = this.formatDate;
            }
            return DateTime.Today.ToString(format).Replace(@"\s", "");
        }
        public string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1: case 21: case 31:
                    return "st";
                case 2: case 22:
                    return "nd";
                case 3: case 23:
                    return "rd";
                default:
                    return "th";
            }
        }
        
    }
}
