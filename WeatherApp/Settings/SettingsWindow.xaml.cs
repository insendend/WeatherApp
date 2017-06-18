using GismeteoService;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private System.Windows.Forms.DateTimePicker timePicker;

        /// <summary>
        /// Default ctor
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();

            // get timer from WinFormHost
            timePicker = (System.Windows.Forms.DateTimePicker)picker.Child;

            // set default value 01/01/2000/00:20:00
            timePicker.Value = new DateTime(2000, 1, 1, 0, 20, 0);
        }

        /// <summary>
        /// Save settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void but_ok_Click(object sender, RoutedEventArgs e)
        {
            // initialize param object
            Param.Instance.Timer.Stop();
           
            // Init params
            Param.Instance.PressUnit = (PressUnits)cmb_pressunit.SelectedValue;
            Param.Instance.TempUnit = (TempUnits)cmb_tempunit.SelectedValue;
            Param.Instance.SpeedUnit = (SpeedUnits)cmb_speedunit.SelectedValue;
            Param.Instance.Region = (string)cmb_cities.SelectedValue;

            // add/remove autorun key from registry
            SetAutorun(cb_startup.IsChecked);

            // check for incorrect time for update
            if (timePicker.Value.TimeOfDay < TimeSpan.FromSeconds(30))
            {
                if (MessageBox.Show(
                    "Время обновления слишком мало\n" +
                    "Установить наименьший интервал?",
                    "Время обновления",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question) ==
                    MessageBoxResult.Yes)

                    // set to minimal time 30 sec
                    timePicker.Value = new DateTime(2000, 1, 1, 0, 0, 30);
                return;
            }

            Param.Instance.Delay = timePicker.Value.TimeOfDay;

            // serialize params to file 
            DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(Param));

            string settingPath = Path.Combine(
                        Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName,
                        "settings.json");

            using (FileStream fs = new FileStream(settingPath, FileMode.Truncate))
                jsonFormatter.WriteObject(fs, Param.Instance);

            Param.Instance.Timer.Interval = TimeSpan.FromMilliseconds(100);
            Param.Instance.Timer.Start();

            Close();
        }

        /// <summary>
        /// Set/Delete autorun key to/from registry
        /// </summary>
        /// <param name="isChecked"></param>
        private void SetAutorun(bool? isChecked)
        {
            try
            {
                using (var reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\"))

                    if (isChecked == true)
                        // SET
                        reg.SetValue("WeatherApp", System.Reflection.Assembly.GetExecutingAssembly().Location);

                    else
                        // DELETE
                        reg.DeleteValue("WeatherApp");
            }
            catch (Exception) { return; }
        }

        /// <summary>
        /// Exit from settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void but_cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Set to default time value 20 min
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timePicker.Value = new DateTime(2000, 1, 1, 0, 20, 0);
        }
   
        /// <summary>
        /// load cities collection by changing service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmb_service_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Param.Instance.Service = (Services)cmb_service.SelectedValue;
            var DicCities = new Dictionary<string, string>();

            switch (Param.Instance.Service)
            {
                case Services.Gismeteo:
                    DicCities = GetGismeteoCities();
                    break;

                case Services.WUndeground:
                    DicCities = GetWUndergroundCities();
                    break;

                default:
                    break;
            }

            cmb_cities.DataContext = DicCities;
            cmb_cities.SelectedIndex = 0;
        }

        /// <summary>
        /// Init cities from gismteo.ua
        /// </summary>
        private static Dictionary<string, string> GetGismeteoCities()
        {
            var Cities = new Dictionary<string, string>();

            try
            {
                XDocument xdoc = null;
                string exeDir = Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName;
                string fileName = "GismeteoCities.xml";
                string fullPath = Path.Combine(exeDir, fileName);

                try
                {
                    xdoc = XDocument.Load(fullPath);
                }
                catch (Exception)
                {
                    // file does not exist, need to create
                    Gismeteo.GetCities().Save(fullPath);
                    xdoc = XDocument.Load(fullPath);
                }

                foreach (XElement cityElement in xdoc.Element("Cities").Elements("City"))
                {
                    XElement nameElement = cityElement.Element("Name");
                    XElement linkElement = cityElement.Element("LinkId");

                    if (!Cities.ContainsKey(linkElement.Value))
                        Cities.Add(linkElement.Value, nameElement.Value);
                }

                // sorting
                Cities = Cities
                    .OrderBy(x => x.Value)
                    .ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return Cities;
        }

        /// <summary>
        /// Init cities from wunderground.com
        /// </summary>
        private static Dictionary<string, string> GetWUndergroundCities()
        {
            var Cities = new Dictionary<string, string>();

            try
            {
                string key = "/Ukraine/Dnipropetrovs'k";
                string val = "Ukraine, Dnepr";

                if (!Cities.ContainsKey(key))
                    Cities.Add(key, val);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return Cities;
        }
    }
}
