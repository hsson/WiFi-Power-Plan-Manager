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
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private const string Unique = "My_Unique_Application_String";
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
            if (Current.MainWindow.WindowState == WindowState.Minimized)
            {
                Current.MainWindow.WindowState = WindowState.Normal;
            }
            Current.MainWindow.Activate();
            Current.MainWindow.BringIntoView();

            return true;
        }
        #endregion
    }
}
