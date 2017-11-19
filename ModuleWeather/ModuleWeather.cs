using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;

using Computer.API;

namespace Computer.Weather
{
    public class ModuleWeather : ModuleBase
    {
        [Setting]
        private string key = "";

        public ModuleWeather()
        {
            
        }
        protected override void Load()
        {

        }

        //public override void ParseCommand(CommandMessage command)
        //{
        //    Command foundCommand = Commands.Find(c => c.Triggers.Contains(command.Command));
        //    switch (foundCommand.Name)
        //    {
        //        case "Weather":
        //            getWeather(command);
        //            break;
        //        case "Forecast":
        //            getForecast(command);
        //            break;
        //    }
        //}

        private List<Weather> GetForecast(string loc)
        {
            int days = 6;
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load("http://api.wunderground.com/auto/wui/geo/WXCurrentObXML/index.xml?query=" + loc);
            }
            catch (WebException exc) { }
            if (!doc.HasChildNodes)
            {
                return null;
            }
            string location = doc.SelectSingleNode("current_observation/display_location/city").InnerText;
            if (string.IsNullOrEmpty(location) || location == ", ")
            {
                return null;
            }
            doc = new XmlDocument();
            try
            {
                doc.Load("http://api.wunderground.com/auto/wui/geo/ForecastXML/index.xml?query=" + loc);
            }
            catch (WebException exc) { }
            if (!doc.HasChildNodes)
            {
                return null;
            }
            XmlNodeList nodes = doc.SelectNodes("forecast/simpleforecast/forecastday");
            List<Weather> list = new List<Weather>();
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            for (int i = 0; i < nodes.Count; i++)
            {
                XmlNode node = nodes[i];
                Weather weather = new Weather();
                weather.Day = epoch.AddSeconds(double.Parse(node.SelectSingleNode("date/epoch").InnerText)).ToLocalTime();
                if (weather.Day.Date == DateTime.Today)
                {
                    continue;
                }
                //Log(epoch.AddSeconds(double.Parse(node.SelectSingleNode("date/epoch").InnerText)).ToLocalTime().ToString());
                weather.Condition = node["conditions"].InnerText;
                weather.Temp.High = int.Parse(node.SelectSingleNode("high/fahrenheit").InnerText);
                weather.Temp.Low = int.Parse(node.SelectSingleNode("low/fahrenheit").InnerText);
            }
            return list;
        }

        private Weather GetWeather(string loc)
        {
            XmlDocument doc = new XmlDocument();
            try
            {                
                doc.Load("http://api.wunderground.com/auto/wui/geo/WXCurrentObXML/index.xml?query=" + loc);
            }
            catch (WebException exc) { }

            if (!doc.HasChildNodes)
            {
                return null;
            }
            XmlNode node = doc.SelectSingleNode("/current_observation");
            if (node == null)
            {
                return null;
            }
            Weather weather = new Weather();
            weather.Location = weather.Location = doc.SelectSingleNode("current_observation/display_location/city").InnerText;
            if (string.IsNullOrEmpty(weather.Location) || weather.Location == ", ")
            {
                return null;
            }

            weather.Temp.Now = int.Parse(node["temp_f"].InnerText);
            weather.Condition = node["weather"].InnerText;
            weather.Humidity = int.Parse(node["relative_humidity"].InnerText.Replace("%", ""));
            weather.Wind.Direction = node["wind_dir"].InnerText;
            weather.Wind.Speed = int.Parse(node["wind_mph"].InnerText);
            //weather.Wind.Gusts = int.Parse(node["wind_gust_mph"].InnerText);

            Log(weather.Location);
            return weather;
        }
    }
}
