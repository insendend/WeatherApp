
namespace WeatherApp
{
    /// <summary>
    /// Block of some сharacteristic
    /// </summary>
    public abstract class InfoBlock
    {            
        /// <summary>
        /// Image for an info block
        /// </summary>
        public string Img { set; get; }
        
        /// <summary>
        /// Value of a сharacteristic
        /// </summary>
        public string Value { set; get; }

        /// <summary>
        /// Units of a сharacteristic
        /// </summary>
        public string Units { set; get; }
    }
}
