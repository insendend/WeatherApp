
namespace WeatherApp
{
    /// <summary>
    /// Temperature info
    /// </summary>
    public class Temperature
    {
        /// <summary>
        /// Value of temperature
        /// </summary>
        public string Value { set; get; }

        /// <summary>
        /// Units of temperature
        /// </summary>
        public string Units { set; get; }

        /// <summary>
        /// Change compare logic
        /// </summary>
        /// <param name="obj">old value</param>
        /// <returns>compare result</returns>
        public override bool Equals(object obj)
        {
            Temperature temp = obj as Temperature;

            return Units == temp?.Units && Value == temp?.Value;
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
