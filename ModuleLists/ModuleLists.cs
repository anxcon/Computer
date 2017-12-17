using System;
using System.Collections.Generic;

using Computer.API;

namespace Computer.Lists
{
    public class ModuleLists : ModuleBase
    {
        public static ModuleLists Instance { get; private set; }
        private SortedDictionary<string, DataList> library;


        public ModuleLists()
        {
            ModuleLists.Instance = this;
        }
        public void AddItemToList(DataItem item, string listName, bool listCreateIfNotExist = false)
        {
            if (string.IsNullOrEmpty(listName)) throw new ArgumentNullException("No list name passed");
            DataList list = this.library[listName];
            if (list == null)
            {
                if (listCreateIfNotExist == false)
                {
                    Log($"The list '{listName}' does not exist and will not be created");
                    return;
                }
                Log($"Creating list '{listName}'");
                list = new DataList(listName);
                this.library.Add(listName, list);
            }
            list.Add(item);
        }
        public DataList GetList(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("No list name passed");
            return this.library[name];
        }
        public DataList CreateList(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("No list name passed");
            DataList list = this.GetList(name);
            if (list != null)
            {
                Log($"The list '{name}' already exists");
                return null;
            }
            Log($"Creating list '{name}'");
            list = new DataList(name);
            this.library.Add(name, list);
            return list;
        }
    }
}
