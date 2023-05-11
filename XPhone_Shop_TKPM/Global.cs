using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPhone_Shop_TKPM
{
    internal class Global
    {
        public static string ConnectionString = "Server = DELL_VWAN;\r\nDatabase = MyShopDB;\r\nTrusted_Connection=yes;\r\nTrustServerCertificate=True";
        public static string role = "";
        public static Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        public static SqlConnection? Connection;
        public static int selectedOrderID = 2;
        public static string usernameCurrent = "";

        public static void SaveScreen(string screenName)
        {
            Global.config.AppSettings.Settings["Screen"].Value = screenName;
            Global.config.Save(ConfigurationSaveMode.Full);
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
    }
}
