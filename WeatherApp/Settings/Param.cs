using System;
using System.Runtime.Serialization;
using System.Windows.Threading;

namespace WeatherApp
{
    /// <summary>
    /// Settings of application
    /// </summary>
    [DataContract]
    public class Param
    {
        private static Param instance;

        /// <summary>
        /// instance of current param object
        /// </summary>
        public static Param Instance
        {
            get { return instance ?? (instance = new Param()); }
        }

        /// <summary>
        /// Flag for loading cities from the file
        /// </summary>
        public bool NeedToLoad { set; get; }

        /// <summary>
        /// Time delay of update weather info
        /// </summary>
        [DataMember]
        public TimeSpan Delay { set; get; }

        /// <summary>
        /// Main timer of working and getting info
        /// </summary>
        public DispatcherTimer Timer { set; get; }

        /// <summary>
        /// Weather service
        /// </summary>
        [DataMember]
        public Services Service { set; get; }

        /// <summary>
        /// City, region
        /// </summary>
        [DataMember]
        public string Region { set; get; }

        /// <summary>
        /// Unit of wind speed
        /// </summary>
        [DataMember]
        public SpeedUnits SpeedUnit { set; get; }

        /// <summary>
        /// Unit of temperature
        /// </summary>
        [DataMember]
        public TempUnits TempUnit { set; get; }

        /// <summary>
        /// Unit of pressure
        /// </summary>
        [DataMember]
        public PressUnits PressUnit { set; get; }

        /// <summary>
        /// private ctor
        /// </summary>
        private Param()
        {
            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(100);

            Delay = TimeSpan.FromMinutes(20);
        }
    }
}
