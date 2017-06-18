using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Threading;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon ni;

        /// <summary>
        /// Default ctor 
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();        
        }

        /// <summary>
        /// Initialize tray icon, timer and minimize app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // create notify icon
            InitTrayIcon();

            // start timer
            InitTimer();

            // minimize to tray
            this.Collapse();
            ni.ShowBalloonTip(5000, "Погода", "Свернуто в трей", ToolTipIcon.Info);

            Param.Instance.NeedToLoad = true;
        }

        /// <summary>
        /// Initialize timer
        /// </summary>
        private void InitTimer()
        {
            Param.Instance.Timer.Tick += new EventHandler(DispatcherTimer_Tick);
            Param.Instance.Timer.Interval = TimeSpan.FromMilliseconds(100);
            Param.Instance.Timer.Start();
        }

        /// <summary>
        /// Ticking method of the timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (Param.Instance.NeedToLoad)
            // load/create settings from file
                LoadOrCreateSettings();

            StartWork();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadOrCreateSettings()
        {
            // filepath to the settings
            string settingPath = Path.Combine(
                Directory.GetParent(System.Reflection.Assembly.GetExecutingAssembly().Location).FullName,
                "settings.json");

            try
            {
                // create the formatter for the type of serializible object
                var jsonFormatter = new DataContractJsonSerializer(typeof(Param));

                // create settings file if not exists
                if (!File.Exists(settingPath))
                    using (var fs = new FileStream(settingPath, FileMode.OpenOrCreate))
                        jsonFormatter.WriteObject(fs, Param.Instance);

                // load settings from the file
                using (var fs = new FileStream(settingPath, FileMode.OpenOrCreate))
                {
                    // deserializing from the file to the param-object
                    var p = (Param)jsonFormatter.ReadObject(fs);

                    // init settings from the file
                    Param.Instance.Delay = p.Delay;
                    Param.Instance.PressUnit = p.PressUnit;
                    Param.Instance.Region = p.Region;
                    Param.Instance.Service = p.Service;
                    Param.Instance.SpeedUnit = p.SpeedUnit;
                    Param.Instance.TempUnit = p.TempUnit;
                }

                Param.Instance.NeedToLoad = false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Initialize a tray icon
        /// </summary>
        private void InitTrayIcon()
        {
            try
            {
                ni = new NotifyIcon();
                ni.Icon = new Icon(Path.Combine(
                        Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName,
                        "tray.ico"));
                ni.Visible = true;
                ni.MouseClick += (s, e) => this.ClickOnIconBehavior(e);
                ni.ContextMenu = new System.Windows.Forms.ContextMenu(new System.Windows.Forms.MenuItem[] {
                    new System.Windows.Forms.MenuItem("Настройки", (s, e) => new SettingsWindow().ShowDialog()),
                    new System.Windows.Forms.MenuItem("О программе", (s, e) => new AboutWindow().ShowDialog()), 
                    new System.Windows.Forms.MenuItem("Выход", (s, e) => Close())});

                Closed += (s, e) => ni.Visible = false;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    ex.Message, 
                    "Error of initialize notificy icon", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }    
        }

        /// <summary>
        /// Set a strategy of getting weather info
        /// </summary>
        /// <param name="content"></param>
        public void SetWeather(WeatherContent content)
        {
            if (((WeatherContent)this.DataContext)?.GetType() != content.GetType())
                // only if contents are different
                this.DataContext = content;
        }

        /// <summary>
        /// Choosing a strategy depending of settings
        /// </summary>
        private void SetWeatherService()
        {
            switch (Param.Instance.Service)
            {
                case Services.Gismeteo:
                    SetWeather(new GismeteoWeather());
                    break;

                case Services.WUndeground:
                    SetWeather(new UndegroundWeather());
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Main working method
        /// </summary>
        private void StartWork()
        {
            if (Param.Instance.Timer.Interval == TimeSpan.FromMilliseconds(100))
            {
                // first tick of timer after initialize 
                Param.Instance.Timer.Stop();
                Param.Instance.Timer.Interval = Param.Instance.Delay;
                Param.Instance.Timer.Start();
            }

            // choosing weather service
            SetWeatherService();

            // init GUI by initialize binding properties
            WeatherContent q = (WeatherContent)this.DataContext;
            q?.SetWeatherInfo();
        }
    }
}