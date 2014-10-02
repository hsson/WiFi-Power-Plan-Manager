using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WifiPowerPlanSelector
{
    class WiFiRule
    {
        public string enabled
        {
            get;
            set;
        }

        public string ssid
        {
            get;
            set;
        }
        public string powerplan
        {
            get;
            set;
        }
    }
}
