using System;
using System.Collections.Generic;

namespace Computer.Lists
{
    public class DataList : List<DataItem>
    {
        public string Name { get; private set; }
        public DateTime LastChanged { get; private set; }

        public DataList(string name)
        {
            this.Name = name;
            this.LastChanged = DateTime.MinValue;
        }
        public void Add(string data)
        {
            this.Add(data, DateTime.Now);
        }
        public void Add(string data, DateTime date)
        {
            base.Add(new DataItem(data, date));
            this.LastChanged = DateTime.Now;
        }
    }
}
