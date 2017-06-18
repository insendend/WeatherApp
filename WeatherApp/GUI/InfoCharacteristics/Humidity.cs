
namespace WeatherApp
{
    /// <summary>
    /// Humidity info
    /// </summary>
    public class Humidity : InfoBlock
    {
        /// <summary>
        /// Advanced info of сharacteristic
        /// </summary>
        public string Description { set; get; }

        /// <summary>
        /// Change compare logic
        /// </summary>
        /// <param name="obj">old value</param>
        /// <returns>compare result</returns>
        public override bool Equals(object obj)
        {
            Humidity hum = obj as Humidity;

            return
                Value == hum?.Value &&
                Img == hum?.Img &&
                Units == hum.Units &&
                Description == hum.Description;
        }

        /// <summary>
        /// Change compare logic
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
