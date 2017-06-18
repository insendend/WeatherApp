using HtmlExtension;
using GismeteoService;
using System;
using System.Windows;

namespace WeatherApp
{
    /// <summary>
    /// Weather info from https://www.gismeteo.ua/
    /// </summary>
    public class GismeteoWeather : WeatherContent
    {        
        /// <summary>
        /// string with weather info
        /// which get from web-address
        /// </summary>
        private string weatherBlock;

        /// <summary>
        /// default ctor
        /// </summary>
        public GismeteoWeather()
        {
            // web-address with weather info
            sourceLink = "https://www.gismeteo.ua";
        }

        /// <summary>
        /// Fill properties and fields
        /// </summary>
        public override void SetWeatherInfo()
        {
            try
            {
                region = Param.Instance.Region;

                weatherBlock = GetWeatherInfo();

                if (string.IsNullOrEmpty(weatherBlock))
                    throw new ArgumentNullException("Weather infoblock has not found");

                // region
                InitPlace();

                // temperature
                InitTemperature();

                // main img and visibility
                InitVisibility();

                // wind
                InitWind();

                // pressure
                InitPressure();

                // humidity
                InitHumidity();

                // water temperature
                InitWaterTemperature();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Init place property
        /// </summary>
        protected override void InitPlace()
        {
            try
            {
                string place = string.Empty;

                // get string block with place info
                string placeBlock = Gismeteo.GetInfoBlock(weatherBlock, Gismeteo.Classes[GismeteoClasses.Place]);

                int startValue = 0, endValue = 0;
                const string separator = ", ";

                // init Place value
                while (true)
                {
                    int placePartPos = placeBlock.IndexOf(Gismeteo.Markers[GismeteMarkers.Place], endValue);

                    if (placePartPos == -1)
                        // last part of place has been found
                        break;

                    if (!string.IsNullOrEmpty(place))
                        place += separator;

                    startValue = placeBlock.IndexOf('>', placePartPos) + 1;
                    endValue = placeBlock.IndexOf('<', startValue);
                    place += placeBlock.Substring(startValue, endValue - startValue);
                }

                Place = place;
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
                // get string block with temperature info
                string tempBlock = Gismeteo.GetInfoBlock(weatherBlock, Gismeteo.Classes[GismeteoClasses.Temp]);

                string marker = string.Empty;
                int startValue = 0, endValue = 0;
                Temperature temp = new Temperature();

                // init temperature
                switch (Param.Instance.TempUnit)
                {
                    case TempUnits.Celsius:
                        marker = "c";
                        temp.Units = "°C";
                        break;
                    case TempUnits.Fahrenheit:
                        marker = "f";
                        temp.Units = "F";
                        break;
                    default:
                        break;
                }

                startValue = tempBlock.IndexOf('>', tempBlock.IndexOf(
                    $"{Gismeteo.Markers[GismeteMarkers.Temp]} {marker}")) + 1;
                endValue = tempBlock.IndexOf('<', startValue);

                temp.Value = tempBlock.Substring(startValue, endValue - startValue);

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
                // get string block with visibility info
                string visBlock = Gismeteo.GetInfoBlock(weatherBlock, Gismeteo.Classes[GismeteoClasses.Visibility]);

                // init visibility
                int startValue = visBlock.IndexOf("//");
                int endValue = visBlock.IndexOf(")", startValue);
                string url = visBlock.Substring(startValue, endValue - startValue);

                Visibility = new Visibility
                {
                    // init image
                    Img = "https:" + url,
                    
                    // init state info
                    State = HtmlHelper.GetValueOfSimpleTag(visBlock, Gismeteo.Markers[GismeteMarkers.Visibility])
                };
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
                // get string block with wind info
                string windBlock = Gismeteo.GetInfoBlock(weatherBlock, Gismeteo.Classes[GismeteoClasses.Wind]);

                int startValue = 0, endValue = 0;
                string marker = string.Empty;

                Wind wind = new Wind();

                switch (Param.Instance.SpeedUnit)
                {
                    case SpeedUnits.Meters:
                        marker = "ms";
                        wind.Units = "м/с";
                        break;

                    case SpeedUnits.Kilometers:
                        marker =  "kmh";
                        wind.Units = "км/ч";
                        break;

                    case SpeedUnits.Miles:
                        marker = "mih";
                        wind.Units = "миль/ч";
                        break;

                    default:
                        break;
                }

                startValue = windBlock.IndexOf('>', windBlock.IndexOf(
                    $"{Gismeteo.Markers[GismeteMarkers.Wind]} {marker}")) + 1;
                endValue = windBlock.IndexOf('<', startValue);

                wind.Img = "Icons/Up Left-26.png";
                wind.Description = HtmlHelper.GetValueOfSimpleTag(windBlock, Gismeteo.Markers[GismeteMarkers.WindDirection]);
                wind.Value = windBlock.Substring(startValue, endValue - startValue);

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
                // get string block with pressure info
                string pressBlock = Gismeteo.GetInfoBlock(weatherBlock, Gismeteo.Classes[GismeteoClasses.Pressure]);

                string marker = string.Empty;
                int startValue = 0, endValue = 0;
                Pressure pres = new Pressure();               

                // init pressure
                switch (Param.Instance.PressUnit)
                {
                    case PressUnits.MmHg:
                        marker = "torr";
                        pres.Units = "мм рт. ст.";
                        break;
                    case PressUnits.gPa:
                        marker = "hpa";
                        pres.Units = "гПа";
                        break;
                    case PressUnits.InchHg:
                        marker = "inch";
                        pres.Units = "д. рт. ст.";
                        break;
                    default:
                        break;
                }
                startValue = pressBlock.IndexOf('>', pressBlock.IndexOf(
                    $"{Gismeteo.Markers[GismeteMarkers.Pressure]} {marker}")) + 1;
                endValue = pressBlock.IndexOf('<', startValue);

                pres.Img = "Icons/Speed-26.png";
                pres.Value = pressBlock.Substring(startValue, endValue - startValue);

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
                // get string block with humidity info
                string humBlock = Gismeteo.GetInfoBlock(weatherBlock, Gismeteo.Classes[GismeteoClasses.Humidity]);

                // init humidity
                int startValue = humBlock.IndexOf('>', humBlock.IndexOf(
                    Gismeteo.Markers[GismeteMarkers.Humidity])) + 1;
                int endValue = humBlock.IndexOf('<', startValue);

                Humidity = new Humidity
                {
                    // init value
                    Value = humBlock.Substring(startValue, endValue - startValue),

                    // init  unit
                    Units = "%",

                    // init humidity image
                    Img = "Icons/Water-26.png",

                    // init description
                    Description = "влажн."
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Humidity info has not found", ex);
            }
        }

        /// <summary>
        /// Init WaterTemperature property
        /// </summary>
        protected override void InitWaterTemperature()
        {
            try
            {
                // get string block with water temperature info
                string waterBlock = Gismeteo.GetInfoBlock(weatherBlock, Gismeteo.Classes[GismeteoClasses.WaterTemp]);

                string marker = string.Empty;
                int startValue = 0, endValue = 0;
                WaterTemperature wtemp = new WaterTemperature();

                // init temperature
                switch (Param.Instance.TempUnit)
                {
                    case TempUnits.Celsius:
                        marker = "c";
                        wtemp.Units = "°C";
                        break;
                    case TempUnits.Fahrenheit:
                        marker = "f";
                        wtemp.Units = "F";
                        break;
                    default:
                        break;
                }
                startValue = waterBlock.IndexOf('>', waterBlock.IndexOf(
                    $"{Gismeteo.Markers[GismeteMarkers.Temp]} {marker}")) + 1;
                endValue = waterBlock.IndexOf('<', startValue);

                wtemp.Value = waterBlock.Substring(startValue, endValue - startValue);
                wtemp.Img = "Icons/Fish Food-26.png";
                wtemp.Description = "вода";

                WaterTemperature = wtemp;
            }
            catch (Exception ex)
            {
                throw new Exception("Water temperature info has not found", ex);
            }
        }


        /// <summary>
        /// Get a block of page source from web-address with weather info
        /// </summary>
        protected override string GetWeatherInfo()
        {
            string res = null;

            try
            {
                string page = HtmlHelper.GetRequest(sourceLink + region);

                // find start and end of infoblock with weather
                string s = Gismeteo.Markers[GismeteMarkers.MainInfo];
                int startBlockPos = page.IndexOf(s);
                int endBlockPos = HtmlHelper.GetClosedTagPosition(page, startBlockPos);

                // get a block of string with weather info
                res = page.Substring(startBlockPos, endBlockPos - startBlockPos);
            }
            catch (Exception) { }

            return res;
        }
    }
}
