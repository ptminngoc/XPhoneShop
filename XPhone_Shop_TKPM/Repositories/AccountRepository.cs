using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPhone_Shop_TKPM.Models;

namespace XPhone_Shop_TKPM.Repositories
{
    public class AccountRepository
    {
        public AccountModel getAccountCurrent()
        {
            AccountModel accountCurrent = new AccountModel();

            if (Global.Connection != null)
            {
                //Lấy thông tin cá nhân
                string sql = $"select * from AccountUser where Username = '{Global.usernameCurrent}'";
                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();
                reader.Read();

                var username = (string)reader["Username"];
                var tel = (string)reader["Tel"];
                var password = (string)reader["Password"];

                reader.Close();

                //Lay thong tin khác
                sql = $"select * from Customer where Tel = '{tel}'";
                command = new SqlCommand(sql, Global.Connection);

                reader = command.ExecuteReader();
                reader.Read();

                var name = (string)reader["Customer_Name"];
                var email = (string)reader["Email"];
                var address = (string)reader["Address"];

                reader.Close();

                accountCurrent = new AccountModel()
                {
                    AccountName = name,
                    AccountTelephone = tel,
                    AccountAddress = address,
                    AccountPassword = password,
                    AccountEmail = email,
                    AccountUsername = username
                };
            }

            return accountCurrent;
        }

        public void updateAccount(string name, string email, string address)
        {
            //Lấy số điện thoại cá nhân
            string sql = $"select a.Tel from AccountUser a where Username = '{Global.usernameCurrent}'";
            var command = new SqlCommand(sql, Global.Connection);

            var reader = command.ExecuteReader();
            reader.Read();
            var tel = (string)reader["Tel"];
            reader.Close();

            //Update lại thông tin cá nhân 
            sql = $"update Customer\r\nset Customer_Name = N'{name}', Address = N'{address}', email = N'{email}'\r\nwhere Tel='{tel}'";
            command = new SqlCommand(sql, Global.Connection);
            command.ExecuteNonQuery();
        }

        public void updatePassword(string passwordNew)
        {
            //Update lại password
            string sql = $"update AccountUser\r\nset Password = '{passwordNew}'\r\n where Username = '{Global.usernameCurrent}'";
            var command = new SqlCommand(sql, Global.Connection);
            command.ExecuteNonQuery();
        }
    }
}
