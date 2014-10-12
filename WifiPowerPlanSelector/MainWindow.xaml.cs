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
using System.Diagnostics;
using System.IO;

namespace WifiPowerPlanSelector
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<WiFiRule> rulesCollection = new ObservableCollection<WiFiRule>();
        private String[] args;
        private String lastKnownWiFi, currentWiFi;
        

        public MainWindow()
        {
            InitializeComponent();

            if (LoadState() == false)
            {
                Close();
            }

            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            lastKnownWiFi = WiFi.connectedWiFi();
            currentWiFi = lastKnownWiFi;

            System.Windows.Threading.DispatcherTimer wifiTimer = new System.Windows.Threading.DispatcherTimer();
            wifiTimer.Tick += new EventHandler(wifiTimer_Tick);
            wifiTimer.Interval = new TimeSpan(0, 0, 10);
            wifiTimer.Start();

            args = Environment.GetCommandLineArgs();

            // If --minimized is used the program will not show. Use this when starting
            // the program at windows boot.
            if (args.Length > 1 && args[1] == "--minimized")
            {
                this.Hide();
            }

            rulesList.ItemsSource = rulesCollection;
        }

        private void wifiTimer_Tick(object sender, EventArgs e)
        {
            currentWiFi = WiFi.connectedWiFi();

            if (currentWiFi != lastKnownWiFi && currentWiFi.Trim() != "")
            {
                applyRule(currentWiFi);
                lastKnownWiFi = currentWiFi;
            }
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
            if(SaveState() == false) 
            {
                MessageBox.Show("The rules could not be saved. They will not stay after reboot.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            this.Visibility = Visibility.Collapsed;
        }

        private void EditRuleMenu_Click(object sender, RoutedEventArgs e)
        {
            WiFiRule item = this.rulesList.SelectedItem as WiFiRule;
            
            //Dummy action
            MessageBox.Show("TODO: Edit: " + item.WiFi.SSID);
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
                WiFiRule newRule = new WiFiRule(true, newRuleWindow.SelectedWiFi, newRuleWindow.SelectedPowerPlan);
                rulesCollection.Add(newRule);
            }
        }

        private bool? applyRule(String ssid)
        {
            foreach (WiFiRule rule in rulesCollection) {
                if (rule.WiFi.SSID == ssid)
                {
                    ExecuteCommand("Powercfg /S " + rule.PowerPlan.GUID);
                }
            }
            return true;
        }

        public static string ExecuteCommand(string command)
        {
            ProcessStartInfo procStartInfo = new ProcessStartInfo("cmd", "/c " + command)
            {
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process proc = new Process())
            {
                proc.StartInfo = procStartInfo;
                proc.Start();

                string output = proc.StandardOutput.ReadToEnd();

                if (string.IsNullOrEmpty(output))
                    output = proc.StandardError.ReadToEnd();

                return output;
            }
        }

        private bool? SaveState()
        {
            try
            {
                using (Stream stream = File.Open("rules.save", FileMode.Create))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    bformatter.Serialize(stream, rulesCollection);
                }
            }
            catch (Exception e)
            {
                MessageBoxResult result = MessageBox.Show("Error: " + e.Message + "\n\nTry again?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result == MessageBoxResult.Yes)
                {
                    return SaveState();
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private bool? LoadState()
        {
            try
            {
                using (Stream stream = File.Open("rules.save", FileMode.Open))
                {
                    var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                    rulesCollection = (ObservableCollection<WiFiRule>)bformatter.Deserialize(stream);
                }
            }
            catch (FileNotFoundException) 
            {
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: The WiFi rules could not be loaded\n\nMessage: " + e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
