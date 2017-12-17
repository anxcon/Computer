using Microsoft.Win32;
//using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Computer.API
{
    public static class Utilities
    {
        //public static string FindAppPath(string name)
        //{
        //    //string path = Directory.GetFiles("D:/", "vlc.exe", SearchOption.AllDirectories)[0];
        //    //RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\App Paths\" + name);
        //    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\App Paths\");

        //    foreach (string subkey in key.GetSubKeyNames())
        //    {
        //        if (!subkey.ToLower().Contains("vlc")) continue;
        //        Core.Log(subkey);
        //    }
        //    string regFilePath = "";


        //    //object objRegisteredValue = key.GetValue("");

        //    //registeredFilePath = value.ToString();
        //    return "";
        //}
        public static List<string> GetFiles(string path, string pattern)
        {
            return new List<string>(Directory.GetFiles(path, pattern, SearchOption.AllDirectories));
        }
        public static List<string> GetFileData(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            return new List<string>(File.ReadAllLines(path));
        }
        public static void WriteFileData(string path, List<string> data)
        {
            File.WriteAllLines(path, data);
        }
        public static void VerifyFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public static string GetHttpData(string url)
        {
            HttpWebRequest req = WebRequest.CreateHttp(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader strm = new StreamReader(resp.GetResponseStream());
            return strm.ReadToEnd();
        }
        //public static XmlHandler GetHttpDataXml(string url)
        //{
        //    return new XmlHandler(Utilities.GetHttpData(url));
        //}
        public static bool StringIsAlphaNumeric(string text)
        {
            return Regex.IsMatch(text, "^[a-zA-Z0-9]+$");
        }
        public static bool StringIsNumeric(string text)
        {
            return Regex.IsMatch(text, "^[0-9]+$");
        }
        public static bool IsRegexMatch(string text, string pattern)
        {
            return Regex.IsMatch(text, pattern);
        }
    }
}
