using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public AboutWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Close window by clicking on it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Open web-site by clicking on hyperlink
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
