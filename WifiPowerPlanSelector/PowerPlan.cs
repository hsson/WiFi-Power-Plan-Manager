using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WifiPowerPlanSelector
{
    public class PowerPlan
    {
        private String name;

        public String Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        public PowerPlan(String name)
        {
            this.name = name;
        }
    }
}
