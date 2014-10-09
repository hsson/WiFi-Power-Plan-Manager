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
using System.Collections.ObjectModel;

namespace WifiPowerPlanSelector
{
    public partial class MainWindow : Window
    {
        ObservableCollection<WiFiRule> rulesCollection = new ObservableCollection<WiFiRule>();

        public MainWindow()
        {
            InitializeComponent();            

            //Dummy data
            WiFiRule dummyRule = new WiFiRule(true, "Dummy-WiFi", "Balanced");
            rulesCollection.Add(dummyRule);
            //End dummy data

            rulesList.ItemsSource = rulesCollection;
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
            //TODO: Make sure to save state.
            Close();
        }

        private void EditRuleMenu_Click(object sender, RoutedEventArgs e)
        {
            WiFiRule item = this.rulesList.SelectedItem as WiFiRule;
            
            //Dummy action
            MessageBox.Show("TODO: Edit: " + item.SSID);
        }

        private void DeleteRuleMenu_Click(object sender, RoutedEventArgs e)
        {
            WiFiRule item = this.rulesList.SelectedItem as WiFiRule;

            MessageBoxResult rsltMessageBox = MessageBox.Show("Are you sure you want to delete this rule?",
                        "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    rulesCollection.Remove((WiFiRule)rulesList.SelectedItem);
                    item.Dispose();
                    break;

                case MessageBoxResult.No:
                    break;
            }
        }

        private void AddNewRuleButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewRule newRuleWindow = new AddNewRule();

            if (newRuleWindow.ShowDialog(Application.Current.MainWindow) == true)
            {
                WiFiRule newRule = new WiFiRule(true, newRuleWindow.SelectedWiFi.SSID, newRuleWindow.SelectedPowerPlan.Name);
                rulesCollection.Add(newRule);
                //MessageBox.Show("TODO: Add rule");
            }
        }
    }
}
