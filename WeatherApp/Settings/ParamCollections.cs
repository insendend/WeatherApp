using System.Collections.Generic;

namespace WeatherApp
{
    /// <summary>
    /// Weather services
    /// </summary>
    public enum Services
    {
        /// <summary>
        /// gismete.ua
        /// </summary>
        Gismeteo,

        /// <summary>
        /// wundergrounc.com
        /// </summary>
        WUndeground,
    };

    /// <summary>
    /// Units of speed
    /// </summary>
    public enum SpeedUnits
    {
        /// <summary>
        /// meters per second
        /// </summary>
        Meters,

        /// <summary>
        /// kilometers per hour
        /// </summary>
        Kilometers,

        /// <summary>
        /// miles per hour
        /// </summary>
        Miles
    };

    /// <summary>
    /// Units of temperature
    /// </summary>
    public enum TempUnits
    {
        /// <summary>
        /// Celsius
        /// </summary>
        Celsius,

        /// <summary>
        /// Fahrenheit
        /// </summary>
        Fahrenheit
    };

    /// <summary>
    /// Units of pressure
    /// </summary>
    public enum PressUnits
    {
        /// <summary>
        /// Millimeter of mercury
        /// </summary>
        MmHg,

        /// <summary>
        /// Pascal
        /// </summary>
        gPa,

        /// <summary>
        /// Inch of mercury
        /// </summary>
        InchHg
    };

    /// <summary>
    /// Storage of collections for settings combobox
    /// </summary>
    static class ParamCollections
    {
        /// <summary>
        /// Collection of weather services
        /// </summary>
        public static Dictionary<Services, string> DicServ { set; get; }


        /// <summary>
        /// Collection of temperature units
        /// </summary>
        public static Dictionary<TempUnits, string> DicTemp { set; get; }

        /// <summary>
        /// Collection of speed units
        /// </summary>
        public static Dictionary<SpeedUnits, string> DicSpeed { set; get; }

        /// <summary>
        /// Collection of pressure units
        /// </summary>
        public static Dictionary<PressUnits, string> DicPres { set; get; }

        /// <summary>
        /// Default ctor
        /// </summary>
        static ParamCollections()
        {
            DicServ = new Dictionary<Services, string>();
            DicServ.Add(Services.Gismeteo, "gismeteo.ua");
            DicServ.Add(Services.WUndeground, "wunderground.com");

            DicTemp = new Dictionary<TempUnits, string>();
            DicTemp.Add(TempUnits.Celsius, "Цельсий");
            DicTemp.Add(TempUnits.Fahrenheit, "Фаренгейт");

            DicSpeed = new Dictionary<SpeedUnits, string>();
            DicSpeed.Add(SpeedUnits.Meters, "метры в сек");
            DicSpeed.Add(SpeedUnits.Kilometers, "километры в час");
            DicSpeed.Add(SpeedUnits.Miles, "миль в час");

            DicPres = new Dictionary<PressUnits, string>();
            DicPres.Add(PressUnits.MmHg, "мм. рт. ст.");
            DicPres.Add(PressUnits.gPa, "гПа");
            DicPres.Add(PressUnits.InchHg, "д. рт. ст.");
        }
    }
}
