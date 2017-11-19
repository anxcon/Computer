using System;

namespace Computer.Weather
{
    public class Weather
    {
        public struct WindData
        {
            public string Direction { get; set; }
            public int Speed { get; set; }
            public int Gusts { get; set; }
        }
        public struct TempData
        {
            public int High { get; set; }
            public int Low { get; set; }
            public int Now { get; set; }
        }
        public DateTime Day { get; set; }
        public string Location { get; set; }
        public string Condition { get; set; }
        public TempData Temp;
        public int Humidity { get; set; }
        public WindData Wind;


    }
}
