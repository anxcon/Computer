using System;

namespace Computer.Lists
{
    public class DataItem
    {
        public DateTime Date { get; private set; }
        public string Data { get; private set; }
        public DataItem(string data, DateTime date)
        {
            this.Data = data;
            this.Date = date;
        }
    }
}
