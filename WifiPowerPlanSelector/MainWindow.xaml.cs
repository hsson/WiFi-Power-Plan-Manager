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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WifiPowerPlanSelector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        public MainWindow()
        {
            InitializeComponent();
            List<WiFiRule> rules = new List<WiFiRule>();

            //Dummy data
            WiFiRule dummyRule = new WiFiRule(true, "Some-WiFi", "Balanced");
            rules.Add(dummyRule);
            //End dummy data

            rulesList.ItemsSource = rules;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            //Make sure to save state.
            Close();
        }

        private void ruleEnabledCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            //Un-enable wifi rule
        }

        private void EditRuleMenu_Click(object sender, RoutedEventArgs e)
        {
            WiFiRule item = this.rulesList.SelectedItem as WiFiRule;
            MessageBox.Show("EDIT: " + item.SSID);
        }

        private void DeleteRuleMenu_Click(object sender, RoutedEventArgs e)
        {
            WiFiRule item = this.rulesList.SelectedItem as WiFiRule;
            MessageBox.Show("DELETE: " + item.SSID);
        }
    }
}
