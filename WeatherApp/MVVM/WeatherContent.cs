using System;
using System.ComponentModel;

namespace WeatherApp
{
    /// <summary>
    /// Base class of a weather info
    /// </summary>
    public class WeatherContent : INotifyPropertyChanged
    {
        /// <summary>
        /// web-servive for getting weaher info
        /// </summary>
        protected string sourceLink;

        /// <summary>
        /// place / region
        /// </summary>
        protected string region;

        /// <summary>
        /// main usefull info about weather
        /// </summary>
        protected string weatherBlockInfo;



        private Temperature temperature;

        /// <summary>
        /// Temperature info
        /// </summary>
        public Temperature Temperature
        {
            set
            {
                if (!value.Equals(temperature))
                {
                    temperature = value;
                    OnPropertyChanged("Temperature");
                }
            }
            get
            {
                return temperature;
            }
        }

        private Visibility visibility;

        /// <summary>
        /// Visibility info
        /// </summary>
        public Visibility Visibility
        {
            set
            {
                if (!value.Equals(visibility))
                {
                    visibility = value;
                    OnPropertyChanged("Visibility");
                }
            }
            get
            {
                return visibility;
            }
        }

        private Wind wind;

        /// <summary>
        /// Wind info
        /// </summary>
        public Wind Wind
        {
            set
            {
                if (!value.Equals(wind))
                {
                    wind = value;
                    OnPropertyChanged("Wind");
                }
            }
            get
            {
                return wind;
            }
        }

        private Pressure pressure;

        /// <summary>
        /// Pressure info
        /// </summary>
        public Pressure Pressure
        {
            set
            {
                if (!value.Equals(pressure))
                {
                    pressure = value;
                    OnPropertyChanged("Pressure");
                }
            }
            get
            {
                return pressure;
            }
        }

        private WaterTemperature watertemperature;

        /// <summary>
        /// Info about water temperature
        /// </summary>
        public WaterTemperature WaterTemperature
        {
            set
            {
                if (!value.Equals(watertemperature))
                {
                    watertemperature = value;
                    OnPropertyChanged("WaterTemperature");
                }
            }
            get
            {
                return watertemperature;
            }
        }

        private Humidity humidity;

        /// <summary>
        /// Humidity info
        /// </summary>
        public Humidity Humidity
        {
            set
            {
                if (!value.Equals(humidity))
                {
                    humidity = value;
                    OnPropertyChanged("Humidity");
                }
            }
            get
            {
                return humidity;
            }
        }

        private string place;

        /// <summary>
        /// Place, city, region
        /// </summary>
        public string Place
        {
            set
            {
                if (!value.Equals(place))
                {
                    place = value;
                    OnPropertyChanged("Place");
                }
            }
            get
            {
                return place;
            }
        }

        /// <summary>
        /// Init place/region
        /// </summary>
        protected virtual void InitPlace()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Init temperature properties
        /// </summary>
        protected virtual void InitTemperature()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Init Visibility properties
        /// </summary>
        protected virtual void InitVisibility()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Init Wind property
        /// </summary>
        protected virtual void InitWind()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Init pressure property
        /// </summary>
        protected virtual void InitPressure()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Init Humidity property
        /// </summary>
        protected virtual void InitHumidity()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Init WaterTemperature property
        /// </summary>
        protected virtual void InitWaterTemperature()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implemented event from interface INotifyPropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify about changed property
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Get response from HTTP GET request
        /// </summary>
        /// <returns>GET response</returns>
        protected virtual string GetWeatherInfo()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Initialize binding properties in delivered classes
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void SetWeatherInfo()
        {
            throw new NotImplementedException();
        }
    }
}
