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
    public partial class EditOldRule : Window
    {
        private List<PowerPlan> powerPlans;

        private WiFi wifi;
        private PowerPlan selectedPowerPlan;

        public WiFi WiFi
        {
            get
            {
                return this.wifi;
            }            
        }

        public PowerPlan SelectedPowerPlan
        {
            get
            {
                return this.selectedPowerPlan;
            }
        }

        public EditOldRule(WiFi wifi)
        {
            InitializeComponent();
            this.wifi = wifi;
            ruleNameText.Text = wifi.SSID;

            powerPlans = new List<PowerPlan>();
            powerPlans = PowerPlan.GetAllPowerPlans();

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

        private void SaveEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (powerPlanComboBox.SelectedIndex > -1)
            {
                selectedPowerPlan = (PowerPlan)powerPlanComboBox.SelectedItem;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("No power plan selected!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public bool? ShowDialog(Window owner)
        {
            this.Owner = owner;
            return this.ShowDialog();
        }
    }
}
