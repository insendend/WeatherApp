
namespace WeatherApp
{
    /// <summary>
    /// Visibility info
    /// </summary>
    public class Visibility
    {
        /// <summary>
        /// State of weather
        /// </summary>
        public string State { set; get; }

        /// <summary>
        /// Main image of a weather state
        /// </summary>
        public string Img { set; get; }

        /// <summary>
        /// Change compare logic
        /// </summary>
        /// <param name="obj">old value</param>
        /// <returns>compare result</returns>
        public override bool Equals(object obj)
        {
            Visibility vis = obj as Visibility;

            return State == vis?.State && Img == vis?.Img;
        }

        /// <summary>
        /// Change compare logic
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return State.GetHashCode();
        }
    }
}
