using DocumentFormat.OpenXml.Math;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();

            Global.SaveScreen("Login");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (System.Configuration.ConfigurationManager.AppSettings["Password"]!.Length != 0)
            {
                //Gán vô TextBox tài khoản, mật khẩu đã lưu
                userNameTextBox.Text = System.Configuration.ConfigurationManager.AppSettings["Username"]!;
                passwordTextBox.Password = System.Configuration.ConfigurationManager.AppSettings["Password"]!;
            }
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            //Lấy tài khoản và mật khẩu ở ô nhập
            string username = userNameTextBox.Text;
            string password = passwordTextBox.Password;

            //Hiển thị progress bar
            progressBar.Visibility = Visibility.Visible;
            loadCanvas.Visibility = Visibility.Visible;
            progressBar.IsIndeterminate = true;

            //Kiểm tra kết nối tới Database
            Global.Connection = await Task.Run(() =>
            {
                var connection = new SqlConnection(Global.ConnectionString);
                try
                {
                    connection.Open();
                    System.Threading.Thread.Sleep(2000);

                }
                catch
                {
                    return null;
                }

                return connection;
            });

            progressBar.IsIndeterminate = false;
            loadCanvas.Visibility = Visibility.Hidden;

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

                    // save username, password to App.config
                    if (rememberCheckBox.IsChecked == true)
                    {
                        AppKeyEnum.saveToConfig(password, username);
                    }

                    if (roleStr == "Admin" || roleStr == "Sale")
                    {
                        Window sc = new Dashboard_Admin_Sale();
                        sc.Show();
                        this.Close();
                    }

                    if (roleStr == "Customer")
                    {
                        Window sc = new Dashboard_Customer();
                        sc.Show();
                        this.Close();
                    }
                    MessageBox.Show("Login Successful");
                }
                else
                {
                    MessageBox.Show("Failed!!! Wrong Username or Password");

                }
            }

            progressBar.Visibility = Visibility.Hidden;
        }

        private void close_btn_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void register_btn_click(object sender, RoutedEventArgs e)
        {
            Window sc = new RegisterView();
            sc.Show();
            this.Close();
        }
    }
}
