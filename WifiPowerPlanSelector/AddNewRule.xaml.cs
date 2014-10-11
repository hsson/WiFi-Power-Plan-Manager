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
        private List<PowerPlan> powerPlans;

        private WiFi chosenWiFi;
        private PowerPlan selctedPowerplan;

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

        public PowerPlan SelectedPowerPlan
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
            powerPlans = new List<PowerPlan>();
            powerPlans = PowerPlan.GetAllPowerPlans();

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
            if (wifiComboBox.SelectedIndex > -1 || powerPlanComboBox.SelectedIndex > -1)
            {
                SelectedWiFi = (WiFi)wifiComboBox.SelectedItem;
                SelectedPowerPlan = (PowerPlan)powerPlanComboBox.SelectedItem;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Everything is not filled in.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        public bool? ShowDialog(Window owner)
        {
            this.Owner = owner;
            return this.ShowDialog();
        }
    }
}
