using HtmlExtension;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace GismeteoService
{
    /// <summary>
    /// Gismeteo markers with weather info
    /// </summary>
    public enum GismeteMarkers
    {
        /// <summary>
        /// City/region block
        /// </summary>
        Place,
        
        /// <summary>
        /// Temperature block
        /// </summary>
        Temp,

        /// <summary>
        /// Visibility block
        /// </summary>
        Visibility,

        /// <summary>
        /// Wind block
        /// </summary>
        Wind,

        /// <summary>
        /// Direction of wind block
        /// </summary>
        WindDirection,

        /// <summary>
        /// Pressure block
        /// </summary>
        Pressure,

        /// <summary>
        /// Humudity block
        /// </summary>
        Humidity,

        /// <summary>
        /// Full weather info block
        /// </summary>
        MainInfo,

        /// <summary>
        /// List of cities
        /// </summary>
        Cities
    };

    /// <summary>
    /// Classes from the gismeteo service
    /// </summary>
    public enum GismeteoClasses
    {
        /// <summary>
        /// For region
        /// </summary>
        Place,

        /// <summary>
        /// For temperature
        /// </summary>
        Temp,

        /// <summary>
        /// For temperature of water
        /// </summary>
        WaterTemp,

        /// <summary>
        /// For visibility
        /// </summary>
        Visibility,

        /// <summary>
        /// For wind
        /// </summary>
        Wind,

        /// <summary>
        /// For pressure
        /// </summary>
        Pressure,

        /// <summary>
        /// For humudity
        /// </summary>
        Humidity
    };
   
    /// <summary>
    /// Class for realization gismeteo service
    /// </summary>
    public static class Gismeteo
    {  
        /// <summary>
        /// Collection of html-markers
        /// </summary>
        public static Dictionary<GismeteMarkers, string> Markers { set; get; }

        /// <summary>
        /// Collection of CSS classes
        /// </summary>
        public static Dictionary<GismeteoClasses, KeyValuePair<string, string>> Classes { set; get; }

        /// <summary>
        /// Static ctor
        /// </summary>
        static Gismeteo()
        {
            InitMarkers();
            InitClasses();
        }

        /// <summary>
        /// Init markers collection
        /// </summary>
        private static void InitMarkers()
        {
            Markers = new Dictionary<GismeteMarkers, string>();
            Markers.Add(GismeteMarkers.Place, "catalog");
            Markers.Add(GismeteMarkers.Temp, "value m_temp");
            Markers.Add(GismeteMarkers.Visibility, "td");
            Markers.Add(GismeteMarkers.Wind, "value m_wind");
            Markers.Add(GismeteMarkers.WindDirection, "dt");
            Markers.Add(GismeteMarkers.Pressure, "value m_press");
            Markers.Add(GismeteMarkers.Humidity, "wicon hum");
            Markers.Add(GismeteMarkers.MainInfo, "<div class=\"section higher\">");
            Markers.Add(GismeteMarkers.Cities, "<a href=\"/weather");
        }

        /// <summary>
        /// Init classes collection
        /// </summary>
        private static void InitClasses()
        {
            Classes = new Dictionary<GismeteoClasses, KeyValuePair<string, string>>();
            Classes.Add(GismeteoClasses.Place, new KeyValuePair<string, string>("div", "scity"));
            Classes.Add(GismeteoClasses.Temp, new KeyValuePair<string, string>("div", "temp"));
            Classes.Add(GismeteoClasses.WaterTemp, new KeyValuePair<string, string>("div", "wicon water"));
            Classes.Add(GismeteoClasses.Wind, new KeyValuePair<string, string>("div", "wicon wind"));
            Classes.Add(GismeteoClasses.Humidity, new KeyValuePair<string, string>("div", "wicon hum"));
            Classes.Add(GismeteoClasses.Visibility, new KeyValuePair<string, string>("dl", "cloudness"));
            Classes.Add(GismeteoClasses.Pressure, new KeyValuePair<string, string>("div", "wicon barp"));
        }

        /// <summary>
        /// Get xml-doc with cities
        /// </summary>
        /// <returns></returns>
        public static XDocument GetCities()
        {
            string page = HtmlHelper.GetRequest("https://www.gismeteo.ua/");

            int startPos = 0, endPos = 0;
            string name = string.Empty;
            string link = string.Empty;

            XElement cities = new XElement("Cities");

            try
            {
                while (true)
                {
                    // start of block with cities info
                    int posStart = page.IndexOf(Markers[GismeteMarkers.Cities], endPos);

                    if (posStart == -1)
                        break;

                    // get name of city
                    startPos = page.IndexOf('>', posStart) + 1;
                    endPos = page.IndexOf('<', startPos);
                    name = page.Substring(startPos, endPos - startPos);

                    // get linkID of city
                    startPos = page.IndexOf('"', posStart) + 1;
                    endPos = page.IndexOf('"', startPos);
                    link = page.Substring(startPos, endPos - startPos);

                    // forming xelement
                    XElement city = new XElement("City",
                        new XElement("Name", name),
                        new XElement("LinkId", link));

                    cities.Add(city);
                }
            }
            catch (Exception) { throw; }

            return new XDocument(cities);
        }


        /// <summary>
        /// Get a part (block) of string
        /// </summary>
        /// <param name="text">info block of parsing</param>
        /// <param name="gismeteoClass">type of getting info</param>
        /// <returns></returns>
        public static string GetInfoBlock(string text, KeyValuePair<string, string> gismeteoClass)
        {
            string res = null;

            try
            {
                string specifiedTag = $"<{gismeteoClass.Key} class=\"{gismeteoClass.Value}\"";

                int startBlock = text.IndexOf(specifiedTag);
                int endBlock = HtmlHelper.GetClosedTagPosition(text, startBlock);
                res = text.Substring(startBlock, endBlock - startBlock);
            }
            catch (Exception) { throw; }

            return res;
        }
    }
}
