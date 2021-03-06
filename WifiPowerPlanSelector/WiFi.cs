﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeWifi;

namespace WifiPowerPlanSelector
{
    [Serializable]
    public class WiFi
    {
        private string ssid = "";
        private static List<WiFi> wifis = new List<WiFi>();
        private static WlanClient client = new WlanClient();

        #region properties
        public string SSID
        {
            get
            {
                return this.ssid;
            }
        }

        public WiFi(string ssid)
        {
            this.ssid = ssid;
        }
        #endregion

        public static void refreshAllWiFi()
        {
            wifis.RemoveRange(0, wifis.Count);

            WlanClient client = new WlanClient();
            foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
            {
                foreach (Wlan.WlanProfileInfo profileInfo in wlanIface.GetProfiles())
                {
                    if (!profileInfo.profileName.Equals(""))
                    {
                        wifis.Add(new WiFi(profileInfo.profileName));
                    }               
                }
            }
        }

        public static String connectedWiFi()
        {
            try
            {
                foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
                {
                    Wlan.WlanConnectionAttributes currentConnection = wlanIface.CurrentConnection;
                    return currentConnection.profileName;                    
                }                
            }
            finally
            {

            }

            return "";
        }

        public static List<WiFi> getAllWiFis()
        {
            return wifis;
        }
    }
}
