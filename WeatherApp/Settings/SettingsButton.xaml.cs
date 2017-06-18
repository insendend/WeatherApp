using System.Windows;
using System.Windows.Controls;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for SettingsButton.xaml
    /// </summary>
    public partial class SettingsButton : UserControl
    {
        /// <summary>
        /// Default ctor
        /// </summary>
        public SettingsButton()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settings = new SettingsWindow();
            settings.ShowDialog();
        }
    }
}
