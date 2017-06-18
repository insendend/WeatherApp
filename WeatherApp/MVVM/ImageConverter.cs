using System;
using System.Windows.Data;
using System.Windows.Media.Imaging;


// Exception: Must create DependencySource on same Thread as the DependencyObject

// Description:
// BitmapImage is DependencyObject so it does matter on 
// which thread it has been created because you cannot access 
// DependencyProperty of an object created on another thread 

// Solution:
// http://stackoverflow.com/questions/26361020/error-must-create-dependencysource-on-same-thread-as-the-dependencyobject-even
// or making Converter class

namespace WeatherApp
{
    /// <summary>
    /// Converter class
    /// </summary>
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class ImageConverter : IValueConverter
    {
        /// <summary>
        /// Implemented from IValueConverter
        /// Convert from string to BitmapImage
        /// </summary>
        /// <param name="value">string with url to ImageSource</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>BitmapImage for Image control in GUI</returns>
        /// <exception cref="Exception"></exception>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string url = value as string;

            if (string.IsNullOrEmpty(url))
                return null;

            try
            {
                return new BitmapImage(new Uri(url, UriKind.RelativeOrAbsolute));
            }
            catch (Exception)
            {
                throw new Exception($"Error converting from {value.ToString()} to BitmapImage");                       
            }
        }

        /// <summary>
        /// Implemented from IValueConverter
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
