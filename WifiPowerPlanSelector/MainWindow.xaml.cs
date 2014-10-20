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
using System.ComponentModel;
using System.Windows.Threading;

namespace WifiPowerPlanSelector
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<WiFiRule> rulesCollection = new ObservableCollection<WiFiRule>();
        private String lastKnownWiFi, currentWiFi;

        // The interval in seconds that the timer will check if any rule can be applied
        private const int UPDATE_INTERVAL = 10;

        public static TextBlock logText;

        private String currentDirectory = Environment.CurrentDirectory;
        private String fullExecutablePath = Environment.CurrentDirectory + "\\" + System.AppDomain.CurrentDomain.FriendlyName;
        private const String programName = "WiFi Power Plan Manager";
        
        /*
         * The contructor loads the previously saved rules and sets
         * various parameters. It also starts the timer that will check
         * wether a new WiFi is connected. 
         */
        public MainWindow()
        {
            InitializeComponent();

            lastKnownWiFi = Properties.Settings.Default.lastKnowWiFi;

            Properties.Settings.Default.pathToProgram = fullExecutablePath;
            Properties.Settings.Default.Save();

            logText = (TextBlock)this.FindName("logTextBlock");

            lastKnownWiFi = WiFi.connectedWiFi();
            currentWiFi = lastKnownWiFi;

            this.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            String[] args = Environment.GetCommandLineArgs();
            
            // If there are rules but they can't be loaded, close the program
            if (LoadState() == false)
            {
                Close();
            }

            // Bind the collection of rules with the graphical list view.
            rulesList.ItemsSource = rulesCollection;
            
            // If --minimized is used as command line argument the program will not show. 
            // This is preferably used when autostarting the program at boot.
            if (args.Length > 1 && args[1] == "--minimized")
            {
                this.Hide();
            }

            applyRule(WiFi.connectedWiFi());

            // Starts the timer that will make sure the rules are followed
            DispatcherTimer wifiTimer = new DispatcherTimer();
            wifiTimer.Tick += new EventHandler(wifiTimer_Tick);
            wifiTimer.Interval = new TimeSpan(0, 0, UPDATE_INTERVAL);
            wifiTimer.Start();
        }

        /**
         * This method is responsible for making sure the rules are being
         * put into action. It currently only applies the rule if switching
         * between two WiFi's.
         */
        private void wifiTimer_Tick(object sender, EventArgs e)
        {
            currentWiFi = WiFi.connectedWiFi();           
            if (currentWiFi != lastKnownWiFi && currentWiFi.Trim() != "")
            {
                applyRule(currentWiFi);
                lastKnownWiFi = currentWiFi;
                Properties.Settings.Default.lastKnowWiFi = lastKnownWiFi;
                Properties.Settings.Default.Save();
            }
        }

        /* 
         * This method enables the posibility to click and drag anywhere
         * on the window to move it. This is essential for moving the window
         * since the borders have been disabled.
         */
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        /*
         * Simply minimizes the window like the default minimize button would do.
         */
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /*
         * Closes the window and hides it from the system tray, but doesn't terminate
         * the process. The program is still running in the background making sure 
         * the rules are being followed. It also saves the rules in case of a reboot.
         */
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if(SaveState() == false) 
            {
                MessageBox.Show("The rules could not be saved. They will not stay after reboot.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            this.Visibility = Visibility.Collapsed;
        }

        /*
         * This method is run when the user right clicks a rule and selects "Edit".
         * The method uses the EditOldRule class to open up a new window for editing. 
         * Editing a rule in this sense is to change powerplan for the selected rule's wifi.
         */
        private void EditRuleMenu_Click(object sender, RoutedEventArgs e)
        {
            WiFiRule item = this.rulesList.SelectedItem as WiFiRule;

            EditOldRule editRuleWindow = new EditOldRule(item.WiFi);

            if (editRuleWindow.ShowDialog(Application.Current.MainWindow) == true)
            {
                foreach (WiFiRule rule in rulesCollection)
                {
                    if (rule.WiFi == item.WiFi)
                    {
                        rule.PowerPlan = editRuleWindow.SelectedPowerPlan;
                        rulesCollection[rulesCollection.IndexOf(rule)] = rule;
                        ICollectionView view = CollectionViewSource.GetDefaultView(rulesCollection);
                        view.Refresh();
                        logText.Text = "Successfully edited rule for: " + rule.WiFi.SSID;
                        break;
                    }
                }
            }
        }

        private void DeleteRuleMenu_Click(object sender, RoutedEventArgs e)
        {
            WiFiRule item = this.rulesList.SelectedItem as WiFiRule;

            MessageBoxResult rsltMessageBox = MessageBox.Show("Are you sure you want to delete this rule?",
                        "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    WiFiRule tmpRule = (WiFiRule)rulesList.SelectedItem;
                    rulesCollection.Remove(tmpRule);
                    item.Dispose();
                    logText.Text = "Removed rule: " + tmpRule.WiFi.SSID;
                    break;

                case MessageBoxResult.No:
                    break;
            }
        }

        private void AddNewRuleButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewRule newRuleWindow = new AddNewRule();
            bool unique = true;

            if (newRuleWindow.ShowDialog(Application.Current.MainWindow) == true)
            {
                foreach (WiFiRule rule in rulesCollection)
                {
                    if (rule.WiFi.SSID == newRuleWindow.SelectedWiFi.SSID)
                    {
                        MessageBox.Show("A rule for the WiFi: '" + newRuleWindow.SelectedWiFi.SSID + "' already exists.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        logText.Text = "Could not add duplicate rule";
                        unique = false;
                    }
                }
                if (unique)
                {
                    WiFiRule newRule = new WiFiRule(true, newRuleWindow.SelectedWiFi, newRuleWindow.SelectedPowerPlan);
                    rulesCollection.Add(newRule);
                }
            }
        }

        private bool? applyRule(String ssid)
        {
            foreach (WiFiRule rule in rulesCollection) {
                if (rule.WiFi.SSID == ssid)
                {
                    ExecuteCommand("Powercfg /S " + rule.PowerPlan.GUID);
                    logText.Text = "Rule applied: " + rule.WiFi.SSID;
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
            logText.Text = "Successfully loaded rules";
            return true;
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsDialog settingsDialog = new SettingsDialog();

            if (settingsDialog.ShowDialog(Application.Current.MainWindow) == true)
            {
                if (settingsDialog.StartWithWindows)
                {
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", programName, fullExecutablePath);
                    Properties.Settings.Default.pathToProgram = fullExecutablePath;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Run", programName, "empty");
                }
            }
        }
    }
}