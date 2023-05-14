using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace XPhone_Shop_TKPM
{
    class AppKeyEnum
    {
        public static string Username = "Username";
        public static string Password = "Password";

        public static string getValue(string key)
        {
            return ConfigurationManager.AppSettings[key]!;
        }

        public static void saveToConfig(string password, string username)
        {


            // save username, password to config
            Global.config.AppSettings.Settings["Username"].Value = username;
            Global.config.AppSettings.Settings["Password"].Value = password;

            Global.config.Save(ConfigurationSaveMode.Full);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
