using System.Windows;
using System.Windows.Forms;

namespace WeatherApp
{
    /// <summary>
    /// Extenstion methods for Window
    /// </summary>
    public static class WindowExtension
    {
        /// <summary>
        /// Minimize to tray
        /// </summary>
        /// <param name="window"></param>
        public static void Collapse(this MainWindow window)
        {
            window.Hide();
            window.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Expand
        /// </summary>
        /// <param name="window"></param>
        public static void Expand(this MainWindow window)
        {
            var desk = SystemParameters.WorkArea;
            window.Left = desk.Right - window.ActualWidth;
            window.Top = desk.Bottom - window.ActualHeight;
            window.Show();
            window.WindowState = WindowState.Normal;
        }

        /// <summary>
        /// Behavior of clicking on a tray icon
        /// </summary>
        /// <param name="window"></param>
        /// <param name="e"></param>
        public static void ClickOnIconBehavior(this MainWindow window, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)

                if (window.WindowState == WindowState.Minimized)
                    // show window
                    window.Expand();
                else
                    // hide
                    window.Collapse();
        }
    }
}
