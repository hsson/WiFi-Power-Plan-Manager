using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WifiPowerPlanSelector
{
    public partial class SettingsDialog : Window
    {
        private bool autostart;

        public bool StartWithWindows
        {
            get
            {
                return autostart;
            }
            set
            {
                autostart = value;
            }
        }

        public SettingsDialog()
        {
            InitializeComponent();

            autostart = Properties.Settings.Default.autostart;
            CheckBoxAutoRun.IsChecked = autostart;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            autostart = (bool)CheckBoxAutoRun.IsChecked;

            Properties.Settings.Default.autostart = autostart;
            Properties.Settings.Default.Save();
            DialogResult = true;
        }

        public bool? ShowDialog(Window owner)
        {
            this.Owner = owner;
            return this.ShowDialog();
        }
    }
}
