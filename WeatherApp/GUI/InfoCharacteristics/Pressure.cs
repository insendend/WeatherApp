
namespace WeatherApp
{
    /// <summary>
    /// Pressure info
    /// </summary>
    public class Pressure : InfoBlock
    {
        /// <summary>
        /// Change compare logic
        /// </summary>
        /// <param name="obj">old value</param>
        /// <returns>compare result</returns>
        public override bool Equals(object obj)
        {
            Pressure press = obj as Pressure;

            return
                Value == press?.Value &&
                Img == press?.Img &&
                Units == press.Units;
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
