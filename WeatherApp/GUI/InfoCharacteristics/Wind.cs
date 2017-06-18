
namespace WeatherApp
{
    /// <summary>
    /// Wind info
    /// </summary>
    public class Wind : InfoBlock
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
            Wind wind = obj as Wind;

            return
                Value == wind?.Value &&
                Img == wind?.Img &&
                Units == wind.Units &&
                Description == wind.Description;
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
