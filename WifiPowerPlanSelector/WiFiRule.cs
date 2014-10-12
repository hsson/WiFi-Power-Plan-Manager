using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WifiPowerPlanSelector
{
    [Serializable]
    class WiFiRule        
    {
        private WiFi wifi;
        private PowerPlan powerPlan;
        private Boolean enabled, deleted;
        private static int nbrOfInstances;

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
        public WiFi WiFi
        {
            get
            {
                return this.wifi;
            }
            set
            {
                this.wifi = value;
            }
        }
        public PowerPlan PowerPlan
        {
            get
            {
                return this.powerPlan;
            }
            set
            {
                this.powerPlan = value;
            }
        }
        public int NumberOfInstances
        {
            get
            {
                return nbrOfInstances;
            }
        }

        public WiFiRule()
        {

        }

        public WiFiRule(Boolean enabled, WiFi wifi, PowerPlan powerPlan)
        {
            this.deleted = false;
            this.enabled = enabled;
            this.wifi = wifi;
            this.powerPlan = powerPlan;
            nbrOfInstances++;
        }

        public void Dispose()
        {
            this.deleted = true;
        }
    }
}
