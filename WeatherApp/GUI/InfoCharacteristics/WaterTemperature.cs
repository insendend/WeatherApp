
namespace WeatherApp
{
    /// <summary>
    /// Info about water temperature
    /// </summary>
    public class WaterTemperature : InfoBlock
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
            WaterTemperature temp = obj as WaterTemperature;

            return
                Value == temp?.Value &&
                Img == temp?.Img &&
                Units == temp.Units &&
                Description == temp.Description;
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
