using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WifiPowerPlanSelector
{
    class WiFiRule        
    {
        private string ssid, powerPlan;
        private Boolean enabled;
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
                enabled = value;
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
            this.enabled = enabled;
            this.ssid = ssid;
            this.powerPlan = powerPlan;
            nbrOfInstances++;
            id = nbrOfInstances;
        }
    }
}
