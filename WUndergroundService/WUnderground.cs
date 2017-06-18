using System;
using System.Collections.Generic;
using System.Globalization;

namespace WUndergroundService
{
    /// <summary>
    /// Markers of json response
    /// </summary>
    public enum JsonValue
    {
        /// <summary>
        /// Full info
        /// </summary>
        WeatherInfo,

        /// <summary>
        /// City/region
        /// </summary>
        Location,

        /// <summary>
        /// Type of ouput location
        /// </summary>
        LocationType,

        /// <summary>
        /// Temperature in Celsius
        /// </summary>
        Temp_C,

        /// <summary>
        /// Temperature in Fahrenheit
        /// </summary>
        Temp_F,

        /// <summary>
        /// Picture of state of the weather
        /// </summary>
        ImageIcon,

        /// <summary>
        /// Description of weather state
        /// </summary>
        State,

        /// <summary>
        /// Speed in km/h
        /// </summary>
        Speed_km,

        /// <summary>
        /// Speed in mp/h
        /// </summary>
        Speed_mp,

        /// <summary>
        /// Direction of wind
        /// </summary>
        WindDirection,

        /// <summary>
        /// Pressure in mm
        /// </summary>
        Pres_mb,

        /// <summary>
        /// Presure in inch
        /// </summary>
        Pres_in,

        /// <summary>
        /// Humudity value
        /// </summary>
        Humudity
    };

    /// <summary>
    /// Class for realization wunderground service
    /// </summary>
    public static class WUnderground
    {
        /// <summary>
        /// Collection of json key-values
        /// </summary>
        public static Dictionary<JsonValue, string> Values { set; get; }

        /// <summary>
        /// Initialization of collection
        /// </summary>
        static WUnderground()
        {
            Values = new Dictionary<JsonValue, string>();
            Values.Add(JsonValue.WeatherInfo, "current_observation");
            Values.Add(JsonValue.Location, "display_location");
            Values.Add(JsonValue.LocationType, "full");
            Values.Add(JsonValue.Temp_C, "temp_c");
            Values.Add(JsonValue.Temp_F, "temp_f");
            Values.Add(JsonValue.ImageIcon, "icon_url");
            Values.Add(JsonValue.State, "weather");
            Values.Add(JsonValue.Speed_km, "wind_kph");
            Values.Add(JsonValue.Speed_mp, "wind_mph");
            Values.Add(JsonValue.WindDirection, "wind_dir");
            Values.Add(JsonValue.Pres_mb, "pressure_mb");
            Values.Add(JsonValue.Pres_in, "pressure_in");
            Values.Add(JsonValue.Humudity, "relative_humidity");
        }

        /// <summary>
        /// Convert speed from km/h to m/s
        /// </summary>
        /// <param name="speed_km">speed in km/h</param>
        /// <returns></returns>
        public static string GetMeterSpeed(string speed_km)
        {
            string res = "??";
            const double koef = 3.6;

            try
            {
                var kmh = double.Parse(speed_km, new CultureInfo("en-US"));
                var ms = Math.Round(kmh / koef, 1);

                res = ms.ToString();
            }
            catch (Exception) { }

            return res;
        }

        /// <summary>
        /// Convert pressure from inch to mm
        /// </summary>
        /// <param name="press_in">pressure in inch</param>
        /// <returns></returns>
        public static string GetMmHgPressure(string press_in)
        {
            string res = "??";
            const double inch = 25.4;

            try
            {
                var pres_inch = double.Parse(press_in, new CultureInfo("en-US"));
                var mmhg = Math.Round(pres_inch * inch);

                res = mmhg.ToString();
            }
            catch (Exception) { }

            return res;
        }
    }
}
