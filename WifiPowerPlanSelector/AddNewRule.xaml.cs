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
using NativeWifi;
using WifiPowerPlanSelector;

namespace WifiPowerPlanSelector
{
    public partial class AddNewRule : Window
    {
        private List<WiFi> wifis;
        private List<String> powerPlans;

        private WiFi chosenWiFi;
        private String selctedPowerplan;

        public WiFi SelectedWiFi
        {
            get
            {
                return chosenWiFi;
            }
            set
            {
                chosenWiFi = value;
            }
        }

        public String SelectedPowerPlan
        {
            get
            {
                return selctedPowerplan;
            }
            set
            {
                selctedPowerplan = value;
            }
        }

        public AddNewRule()
        {
            InitializeComponent();
            WiFi.refreshAllWiFi();
            wifis = WiFi.getAllWiFis();
            powerPlans = new List<string>();
            powerPlans.Add("Dummy plan");
            powerPlans.Add("Dumber dummy plan");

            wifiComboBox.ItemsSource = wifis;
            powerPlanComboBox.ItemsSource = powerPlans;
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

        private void SaveRuleButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Make sure everything is filled in.
            SelectedWiFi = (WiFi)wifiComboBox.SelectedItem;
            SelectedPowerPlan = (String)powerPlanComboBox.SelectedItem;
            DialogResult = true;
        }

        public bool? ShowDialog(Window owner)
        {
            this.Owner = owner;
            return this.ShowDialog();
        }
    }
}
