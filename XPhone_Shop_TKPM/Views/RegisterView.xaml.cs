using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for RegisterView.xaml
    /// </summary>
    public partial class RegisterView : Window
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void close_btn_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void loginview_btn_click(object sender, RoutedEventArgs e)
        {
            Window sc = new LoginView();
            sc.Show();
            this.Close();
        }

        private async void register_btn_click(object sender, RoutedEventArgs e)
        {
            var check = true;

            var name = nameTextBox.Text;
            var username = userNameTextBox.Text;
            var password = passwordTextBox.Password;
            var passwordConfirm = passwordConfirmTextBox.Password;
            var phone = phoneTextBox.Text;
            var email = emailTextBox.Text;
            var address = addressTextBox.Text;

            if (name == "" || username == "" || password == "" || passwordConfirm == "" || phone == "" || email == "" || address == "")
            {
                check = false;
                MessageBox.Show("Hãy điền đầy đủ thông tin");
            }
            else if (password != passwordConfirm)
            {
                check = false;
                MessageBox.Show("Mật khẩu và mật khẩu nhập lại không trùng khớp");
            }

            if (check == true)
            {
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

                //Kiểm tra tài khoản có tồn tại trong bảng Account
                var checkExist = false;

                string sql = $"select * from Account where Username = '{username}'";
                var command = new SqlCommand(sql, Global.Connection);
                var reader = command.ExecuteReader();

                //Nếu tài khoản có tồn tại
                if (reader.Read() != false)
                {
                    checkExist = true;
                }
                reader.Close();

                //Kiểm tra tài khoản có tồn tại trong bảng Account User 
                sql = $"select * from AccountUser where Username = '{username}'";
                command = new SqlCommand(sql, Global.Connection);
                reader = command.ExecuteReader();

                if (reader.Read() != false)
                {
                    checkExist = true;
                }
                reader.Close();


                if (checkExist == true)
                {
                    MessageBox.Show("Tài khoản đã tồn tại");
                }
                else
                {
                    // encrypt password
                    string salt = BCrypt.Net.BCrypt.GenerateSalt();
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

                    sql = $"INSERT INTO Customer VALUES(N'{name}', '{phone}', N'{address}', '{email}')";
                    command = new SqlCommand(sql, Global.Connection);
                    command.ExecuteNonQuery();

                    sql = $"INSERT INTO AccountUser VALUES(N'{username}', '{hashedPassword}', 'Customer', '{phone}')";
                    command = new SqlCommand(sql, Global.Connection);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Tạo tài khoản thành công");
                    
                    Window sc = new LoginView();
                    sc.Show();
                    this.Close();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
