using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.Remoting;
using Microsoft.Shell;

namespace WifiPowerPlanSelector
{
    public partial class App : Application, ISingleInstanceApp
    {
        public static Boolean minimized = false;

        private const string Unique = "WiFiPowerPlanManager";
        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                var application = new App();
                application.InitializeComponent();
                application.Run();
                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }
        #region ISingleInstanceApp Members
        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            if (minimized)
            {
                Current.MainWindow.Show();
                minimized = false;
            }

            if (Current.MainWindow.Visibility == Visibility.Collapsed)
            {
                Current.MainWindow.Visibility = Visibility.Visible;
                CenterWindowOnScreen();
            }

            if (Current.MainWindow.WindowState == WindowState.Minimized)
            {
                Current.MainWindow.WindowState = WindowState.Normal;
            }            

            Current.MainWindow.Activate();
            Current.MainWindow.BringIntoView();

            return true;
        }
        #endregion

        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double windowWidth = Current.MainWindow.Width;
            double windowHeight = Current.MainWindow.Height;
            Current.MainWindow.Left = (screenWidth / 2) - (windowWidth / 2);
            Current.MainWindow.Top = (screenHeight / 2) - (windowHeight / 2);
        }
    }
}
