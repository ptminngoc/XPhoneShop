using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
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
using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.ViewModels;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for ConfirmOTPView.xaml
    /// </summary>
    public partial class ConfirmOTPView : Window
    {
        string otp;
        string name;
        string username;
        string address;
        string hashedPassword;
        string phone;
        string email;
        public ConfirmOTPView(AccountModel acc, string OTP)
        {
            InitializeComponent();
            otp = OTP;
            username = acc.AccountUsername;
            name = acc.AccountName;
            address = acc.AccountAddress;
            hashedPassword = acc.AccountPassword;
            phone = acc.AccountTelephone;
            email = acc.AccountEmail;
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            string otpEnter = otpTextBox.Text;
            if(otp == otpEnter)
            {
                //Kiểm tra đã là Customer trước đó chưa 
                var checkExist = false;
                var sql = $"select * from Customer where Tel = '{phone}'";
                var command = new SqlCommand(sql, Global.Connection);
                var reader = command.ExecuteReader();

                if (reader.Read() != false)
                {
                    checkExist = true;
                }
                reader.Close();

                if(checkExist)
                {
                    if (MessageBox.Show("Bạn đã là khách hàng trước đó! Bạn có muốn tạo tài khoản với thông tin mới này?",
                        "Hiệu chỉnh",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        sql = $"INSERT INTO AccountUser VALUES(N'{username}', '{hashedPassword}', 'Customer', '{phone}')";
                        command = new SqlCommand(sql, Global.Connection);
                        command.ExecuteNonQuery();

                        sql = "UPDATE Customer SET  Customer_Name = @name, Email = @email, Address = @address WHERE Tel = @phone";

                        command = new SqlCommand(sql, Global.Connection);

                        command.Parameters.AddWithValue("@phone", phone);
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@address", address);
                        command.Parameters.AddWithValue("@email", email);

                        command.ExecuteNonQuery();
                        Window sc = new LoginView();
                        sc.Show();
                        this.Close();
                    }
                }
                else
                {
                    sql = $"INSERT INTO Customer VALUES(N'{name}', '{phone}', '{address}', '{email}')";
                    command = new SqlCommand(sql, Global.Connection);
                    command.ExecuteNonQuery();

                    sql = $"INSERT INTO AccountUser VALUES(N'{username}', '{hashedPassword}', 'Customer', '{phone}')";
                    command = new SqlCommand(sql, Global.Connection);
                    command.ExecuteNonQuery();

                    MessageBox.Show("Mã OTP hợp lệ. Tạo tài khoản thành công!");
                    Window sc = new LoginView();
                    sc.Show();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Lỗi. Mã OTP không hợp lệ!");
            }
        }

        private void close_btn_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Window sc = new RegisterView();
            sc.Show();
            this.Show();
        }
    }
}
