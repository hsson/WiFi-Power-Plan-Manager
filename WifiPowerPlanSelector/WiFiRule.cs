using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WifiPowerPlanSelector
{
    class WiFiRule        
    {
        private string ssid, powerPlan;
        private Boolean enabled, deleted;
        private static int nbrOfInstances;
        private int id;

        public Boolean Enabled
        {
            get 
            {
                return enabled;
            }
            set
            {
                if (!value)
                {                
                    MessageBoxResult rsltMessageBox = MessageBox.Show("Are you sure you want to disable this rule?",
                        "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    switch (rsltMessageBox)
                    {
                        case MessageBoxResult.Yes:
                            enabled = value;
                            break;

                        case MessageBoxResult.No:
                            break;
                    }
                }
                else
                {
                    enabled = value;
                }
            }
        }
        public string SSID
        {
            get
            {
                return ssid;
            }
            set
            {
                ssid = value;
            }
        }
        public string PowerPlan
        {
            get
            {
                return powerPlan;
            }
            set
            {
                powerPlan = value;
            }
        }
        public int NumberOfInstances
        {
            get
            {
                return nbrOfInstances;
            }
        }
        public int ID
        {
            get 
            {
                return id;
            }
        }

        public WiFiRule(Boolean enabled, string ssid, string powerPlan)
        {
            this.deleted = false;
            this.enabled = enabled;
            this.ssid = ssid;
            this.powerPlan = powerPlan;
            nbrOfInstances++;
            id = nbrOfInstances;
        }

        public void Dispose()
        {
            this.deleted = true;
        }
    }
}
