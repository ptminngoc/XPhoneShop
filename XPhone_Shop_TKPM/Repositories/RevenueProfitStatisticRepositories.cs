﻿using Microsoft.Data.SqlClient;
using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XPhone_Shop_TKPM.Repositories
{
    class RevenueProfitStatisticRepositories
    {
        public ObservableCollection<RevenueProfitStatisticModel> getAllRevenueAndProfit(DateTime start, DateTime end)
        {
            ObservableCollection<RevenueProfitStatisticModel> result = new ObservableCollection<RevenueProfitStatisticModel>();
            Global.Connection = new SqlConnection(Global.ConnectionString);
            Global.Connection.Open();
            if (Global.Connection != null)
            {
                string sql = string.Format("SELECT p.Purchase_ID, p.Centered_At, p.Total,SUM(pd.Quantity * pr.Price_Original) as Capital, (p.Total - SUM(pd.Quantity * pr.Price_Original)) as Profit\r\nFROM Purchase p left join PurchaseDetail pd on p.Purchase_ID = pd.Purchase_ID join\r\n\tProduct pr on pr.Product_ID = pd.Product_ID\r\nWHERE p.Centered_At BETWEEN '{0}' AND '{1}'\r\nGROUP BY p.Purchase_ID, p.Centered_At, p.Total ORDER BY p.Centered_At", start.ToString("yyyy-MM-dd"), end.ToString("yyyy-MM-dd"));
                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {

                    int id = (int)reader["Purchase_ID"];
                    DateTime date = (DateTime)reader["Centered_At"];
                    double revenue = (double)reader["Total"];
                    double capital = (double)reader["Capital"];
                    double profit = (double)reader["Profit"];

                    result.Add(new RevenueProfitStatisticModel()
                    {
                        id = id,
                        date = date,
                        profit = profit,
                        capital = capital,
                        revenue = revenue
                    }); 
                }

                reader.Close();
            }
            //Global.Connection?.Close();
            return result;
        }
    }
}
