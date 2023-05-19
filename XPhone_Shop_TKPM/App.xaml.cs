using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Maps.MapControl.WPF.Overlays;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using XPhone_Shop_TKPM.Views;

namespace XPhone_Shop_TKPM
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            connectDBWithExistsAccount();
        }

        private void connectDBWithExistsAccount ()
        {
            string _screen = System.Configuration.ConfigurationManager.AppSettings["Screen"]!;
            if (_screen == null || _screen.Equals("Login"))
            {
                new LoginView().Show();
                Debug.WriteLine(_screen);
            }
            else
            {
                try
                {
                    string username = System.Configuration.ConfigurationManager.AppSettings["Username"]!;
                    string password = System.Configuration.ConfigurationManager.AppSettings["Password"]!;

                    //Kiểm tra kết nối tới Database
                    Global.Connection = new SqlConnection(Global.ConnectionString); 
                    Global.Connection.Open();

                    //Nếu như kết nối thất bại
                    if (Global.Connection == null)
                    {
                        MessageBox.Show("Login Unsuccessful");
                    }
                    // Kết nối thành công
                    else
                    {
                        var checkAccount = false;
                        string roleStr = "";

                        //Kiểm tra tài khoản có tồn tại trong bảng Account
                        string sql = $"select Rolename from Account where Username = '{username}'";
                        var command = new SqlCommand(sql, Global.Connection);
                        var reader = command.ExecuteReader();

                        //Nếu tài khoản có tồn tại
                        if (reader.Read() != false)
                        {
                            Debug.WriteLine(_screen);

                            Debug.WriteLine("Hello");
                            Debug.WriteLine(Global.role);
                            //vai trò account
                            roleStr = (string)reader["Rolename"];
                            reader.Close();

                            //Lấy mật khẩu ứng với tài khoản trong db
                            sql = $"select Password from Account where Username = '{username}'";
                            command = new SqlCommand(sql, Global.Connection);
                            reader = command.ExecuteReader();
                            reader.Read();
                            string hashedPassword = (string)reader["Password"];
                            reader.Close();

                            //Kiểm tra mật khẩu vừa nhập với mật khẩu trong database
                            var result = BCrypt.Net.BCrypt.Verify(password, hashedPassword);

                            if (result)
                            {
                                checkAccount = true;
                            }
                            //Nếu sai
                            else
                            {
                                checkAccount = false;
                            }
                        }
                        else
                        {
                            reader.Close();

                            //Kiểm tra tài khoản có tồn tại trong bảng Account User 
                            sql = $"select * from AccountUser where Username = '{username}'";
                            command = new SqlCommand(sql, Global.Connection);
                            reader = command.ExecuteReader();

                            if (reader.Read() != false)
                            {
                                //đọc Username 
                                string usernameCheck = (string)reader["Username"];
                                reader.Close();

                                //Lấy mật khẩu ứng với tài khoản trong db
                                sql = $"select Password from AccountUser where Username = '{username}'";
                                command = new SqlCommand(sql, Global.Connection);
                                reader = command.ExecuteReader();
                                reader.Read();
                                string hashedPassword = (string)reader["Password"];
                                reader.Close();

                                //Kiểm tra mật khẩu vừa nhập với mật khẩu trong database
                                var result = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
                                if (result)
                                {
                                    roleStr = "Customer";
                                    checkAccount = true;
                                }
                                //Nếu sai
                                else
                                {
                                    checkAccount = false;
                                }
                            }
                        }
                        if (checkAccount)
                        {
                            //Username current
                            Global.usernameCurrent = username;
                            Global.role = roleStr;

                            if (roleStr == "Admin" || roleStr == "Sale")
                            {
                                new Dashboard_Admin_Sale().Show();
                            }

                            if (roleStr == "Customer")
                            {
                                new Dashboard_Customer().Show();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Can't connect please login");

                        }
                    }
                }
                catch (Exception ex)
                {
                    new LoginView().Show();
                }
            }
        }
    }
}
