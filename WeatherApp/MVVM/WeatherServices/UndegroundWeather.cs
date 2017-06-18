using HtmlExtension;
using Newtonsoft.Json.Linq;
using System;
using System.Windows;
using WUndergroundService;

namespace WeatherApp
{
    /// <summary>
    /// Weather info from https://www.wunderground.com/
    /// </summary>
    public class UndegroundWeather : WeatherContent
    {
        private JToken jtoken;

        // API key, which sign in page while requesting.
        private const string token = "11a19e8bc6f8a0c6";

        // key for GET-request, which points on type of getting info
        private const string features = "conditions";

        // format of http-response
        private const string format = ".json";

        /// <summary>
        /// default ctor
        /// </summary>
        public UndegroundWeather()
        {
            // text of GET-request which means:
            // 'api.wunderground.com/api' - main adress
            // 'token' - API key
            // 'conditions' - features, which return main information
            //      current temperature, 
            //      weather condition, 
            //      humidity, wind, 
            //      'feels like' temperature, 
            //      barometric pressure
            //      and visibility
            // 'q' - notify about next location for which we want weather information
            sourceLink = $"http://api.wunderground.com/api/{token}/{features}/q";
        }

        /// <summary>
        /// Fill properties and fields
        /// </summary>
        public override void SetWeatherInfo()
        {
            try
            {
                region = Param.Instance.Region;

                weatherBlockInfo = GetWeatherInfo();

                if (string.IsNullOrEmpty(weatherBlockInfo))
                    throw new ArgumentNullException("Weather infoblock has not found");

                var json = JObject.Parse(weatherBlockInfo);
                jtoken = json[WUnderground.Values[JsonValue.WeatherInfo]];

                // init place
                InitPlace();

                // init temperature
                InitTemperature();

                // init visibility
                InitVisibility();

                // init wind
                InitWind();

                // init pressure
                InitPressure();

                // init humudity
                InitHumidity();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitPlace()
        {
            try
            {
                Place = jtoken
                    [WUnderground.Values[JsonValue.Location]]
                    [WUnderground.Values[JsonValue.LocationType]]
                    .ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Place info has not found", ex);
            }
        }

        /// <summary>
        /// Init temperature properties
        /// </summary>
        protected override void InitTemperature()
        {
            try
            {
                Temperature temp = new Temperature();
                switch (Param.Instance.TempUnit)
                {
                    case TempUnits.Celsius:
                        temp.Value = jtoken[WUnderground.Values[JsonValue.Temp_C]].ToString();
                        temp.Units = "°C";
                        break;
                    case TempUnits.Fahrenheit:
                        temp.Value = jtoken[WUnderground.Values[JsonValue.Temp_F]].ToString();
                        temp.Units = "F";
                        break;
                    default:
                        break;
                }
                Temperature = temp;
            }
            catch (Exception ex)
            {
                throw new Exception("Temperature info has not found", ex);
            }
        }

        /// <summary>
        /// Init Visibility properties
        /// </summary>
        protected override void InitVisibility()
        {
            try
            {
                Visibility vis = new Visibility();
                vis.Img = jtoken[WUnderground.Values[JsonValue.ImageIcon]].ToString();
                vis.State = jtoken[WUnderground.Values[JsonValue.State]].ToString();
                Visibility = vis;
            }
            catch (Exception ex)
            {
                throw new Exception("Visibility info has not found", ex);
            }
        }

        /// <summary>
        /// Init Wind property
        /// </summary>
        protected override void InitWind()
        {
            try
            {
                Wind wind = new Wind();

                string speed_km = jtoken[WUnderground.Values[JsonValue.Speed_km]].ToString();

                switch (Param.Instance.SpeedUnit)
                {
                    case SpeedUnits.Meters:
                        wind.Value = WUnderground.GetMeterSpeed(speed_km);
                        wind.Units = "m/s";
                        break;

                    case SpeedUnits.Kilometers:
                        wind.Value = speed_km;
                        wind.Units = "km/h";
                        break;

                    case SpeedUnits.Miles:
                        wind.Value = jtoken[WUnderground.Values[JsonValue.Speed_mp]].ToString();
                        wind.Units = "mh/h";
                        break;

                    default:
                        break;
                }
                wind.Description = jtoken[WUnderground.Values[JsonValue.WindDirection]].ToString();
                wind.Img = "Icons/Up Left-26.png";
                Wind = wind;
            }
            catch (Exception ex)
            {
                throw new Exception("Wind info has not found", ex);
            }
        }

        /// <summary>
        /// Init pressure property
        /// </summary>
        protected override void InitPressure()
        {
            try
            {
                Pressure pres = new Pressure();

                string pres_in = jtoken[WUnderground.Values[JsonValue.Pres_in]].ToString();

                switch (Param.Instance.PressUnit)
                {
                    case PressUnits.MmHg:
                        pres.Value = WUnderground.GetMmHgPressure(pres_in);
                        pres.Units = "mm";
                        break;
                    case PressUnits.gPa:
                        pres.Value = jtoken[WUnderground.Values[JsonValue.Pres_mb]].ToString();
                        pres.Units = "mb";
                        break;
                    case PressUnits.InchHg:
                        pres.Value = pres_in;
                        pres.Units = "in";
                        break;
                    default:
                        break;
                }
                pres.Img = "Icons/Speed-26.png";
                Pressure = pres;
            }
            catch (Exception ex)
            {
                throw new Exception("Pressure info has not found", ex);
            }
        }

        /// <summary>
        /// Init Humidity property
        /// </summary>
        protected override void InitHumidity()
        {
            try
            {
                Humidity = new Humidity
                {
                    Value = jtoken[WUnderground.Values[JsonValue.Humudity]].ToString(),
                    Units = string.Empty,
                    Img = "Icons/Water-26.png",
                    Description = "humudity"
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Humidity info has not found", ex);
            }
        }

        /// <summary>
        /// Get a JSON response after GET-request
        /// </summary>
        /// <returns></returns>
        protected override string GetWeatherInfo()
        {
            // 'region' - place of weather, for exmaple, /CA/San_Francisco
            // 'format' - output format (json or xml)
            return HtmlHelper.GetRequest(sourceLink + region + format);
        }
    }
}
