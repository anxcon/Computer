using System;
using System.Collections;
using System.Collections.Generic;
//using System.IO;
using System.Reflection;

namespace Computer.API
{
    public sealed class Settings
    {
        public static Settings Instance { get; private set; }
        private static bool Changed { get; set; }
        private SortedDictionary<string, string> list;

        public Settings()
        {
            Settings.Instance = this;
            this.list = new SortedDictionary<string, string>();
        }
        public static void AddValue(string name, object obj)
        {
            Settings.Changed = true;
            if (obj is IList)
            {
                Settings.Instance.list[name] = string.Join(",", (IEnumerable<object>)obj);
                return;
            }
            Settings.Instance.list.Add(name, obj.ToString());
        }
        public static object GetValue(string name, object def)
        {
            Type type = def.GetType();
            if (!Settings.Instance.list.ContainsKey(name))
            {
                Settings.AddValue(name, def);
                return def;
            }
            if (type == typeof(string))
            {
                return (object)Settings.Instance.list[name];
            }
            if (def is IList)
            {
                if (type.GenericTypeArguments[0] == typeof(string))
                {
                    return new List<string>(Settings.Instance.list[name].Split(','));
                }
                return def;
            }
            MethodInfo mi = type.GetMethod("Parse", new Type[] { typeof(string) });
            return mi.Invoke(null, new object[] { Settings.Instance.list[name] });
        }
        public static void SetValue(string name, object obj)
        {
            if (!Settings.Instance.list.ContainsKey(name))
            {
                Settings.AddValue(name, obj);
                return;
            }
            string value = (obj is IList) ? string.Join(",", (IEnumerable<object>)obj) : obj.ToString();
            if (Settings.Instance.list[name] == value)
            {
                return;
            }
            Settings.Changed = true;
            Settings.Instance.list[name] = value;
        }
        internal static void Load(string path)
        {
            Logger.Log("Loading settings...");
            Settings settings = new Settings();
            try
            {
                List<string> data = Utilities.GetFileData($"{path}settings.cfg");
                if (data == null)
                {
                    Logger.LogErr("Settings file does not exist and will be created");
                    Settings.Instance = settings;
                    return;
                }
                for (int i = 0; i < data.Count; i++)
                {
                    string line = data[i];
                    int div = line.IndexOf(" = ");
                    if (div == -1)
                    {
                        Logger.LogErr($"Unable to parse: {line}");
                        continue;
                    }
                    string[] kv = new string[] { line.Substring(0, div), line.Substring(div + 3) };
                    settings.list.Add(kv[0].Trim(), kv[1].Trim());
                }
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return;
            }
            Settings.Instance = settings;
            Logger.Log("Loading settings complete");
        }
        internal static void Save(string path)
        {
            if (!Settings.Changed)
            {
                return;
            }
            Logger.Log("Saving settings...");
            try
            {
                int cnt = Settings.Instance.list.Count;
                List<string> lines = new List<string>();
                foreach (KeyValuePair<string, string> item in Settings.Instance.list)
                {
                    lines.Add($"{item.Key} = {item.Value}");
                }
                Utilities.WriteFileData($"{path}settings.cfg", lines);
            }
            catch (Exception exc)
            {
                Logger.Log(exc);
                return;
            }
            Logger.Log("Saving settings complete");
        }
    }
}
