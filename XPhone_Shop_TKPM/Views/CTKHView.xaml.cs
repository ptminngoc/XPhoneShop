﻿using Microsoft.Win32;
using XPhone_Shop_TKPM.Converters;
using XPhone_Shop_TKPM.UserControls;
using XPhone_Shop_TKPM.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for CTKHView.xaml
    /// </summary>
    public partial class CTKHView : Page
    {
        CTKHViewModel _viewModel;

        public CTKHView(string? tel)
        {
            InitializeComponent();

            _viewModel = new CTKHViewModel(tel);
            base.DataContext = _viewModel._customer;

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
            bool isMatch = Regex.IsMatch(editEmail.Text, pattern);
            if (editName.Text.Length == 0 || editPhone.Text.Length == 0 || editPhone.Text.Length == 0 || editAddress.Text.Length == 0 || editEmail.Text.Length == 0)
            {
                string title = "kiểm tra nhập thông tin";
                string message = "Vui lòng điền đủ thông tin";
                MessageBox.Show(message, title, MessageBoxButton.OK,MessageBoxImage.Warning);
            }
            else if(!isMatch)
            {
                string title = "Kiểm tra";
                string message = "Vui lòng nhập đúng địa chỉ Email theo @gmail.com";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (editPhone.Text.Replace("-", "").Length != 10)
            {
                string title = "Kiểm tra";
                string message = "Vui lòng nhập đúng số điện thoại đủ 10 số";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
               if(!_viewModel.getCustomerPhone(editPhone.Text.Replace("-", ""), _viewModel._customerRestore.phone.Replace("-", "")))
                {
                    if (MessageBox.Show("Bạn muốn hiệu chỉnh lại thông tin khách hàng này không?",
                       "Hiệu chỉnh",
                       MessageBoxButton.YesNo,
                       MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {

                        _viewModel._customer.name = editName.Text;
                        _viewModel._customer.phone = editPhone.Text.Replace("-", "");
                        _viewModel._customer.email = editEmail.Text;
                        _viewModel._customer.address = editAddress.Text;

                        Debug.WriteLine(_viewModel._customer.phone + ",,,," + _viewModel._customerRestore.phone.Replace("-", ""));
                        var edit = _viewModel.EditCustomer(_viewModel._customer, _viewModel._customerRestore.phone.Replace("-", ""));
                        if (edit)
                        {
                            string title = "Hiệu chỉnh";
                            string message = "Đã cập nhật thông tin khách hàng thành công";
                            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                            _viewModel._customerRestore = _viewModel._customer;
                            base.DataContext = new MainViewModel();
                        }
                    }

                    editName.IsReadOnly = true;
                    editEmail.IsReadOnly = true;
                    editPhone.IsReadOnly = true;
                    editAddress.IsReadOnly = true;

                    saveBtn.Visibility = Visibility.Hidden;
                    restoreBtn.Visibility = Visibility.Hidden;
                    editBtn.Visibility = Visibility.Visible;
                }
               else
                {
                    string message = "Số điện thoại đã tồn tại";
                    string title = "Hiệu chỉnh";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                    editPhone.Clear();
                    //base.DataContext = new MainViewModel();
                }
            }
        }


        private void restoreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn khôi phục lại thông tin khách hàng này không?",
            "Khôi phục",
             MessageBoxButton.YesNo,
             MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                editName.Text = _viewModel._customerRestore.name;
                editEmail.Text = _viewModel._customerRestore.email;
                editPhone.Text = _viewModel._customerRestore.phone;
                editAddress.Text = _viewModel._customerRestore.address;
            }
            editName.IsReadOnly = true;
            editEmail.IsReadOnly = true;
            editPhone.IsReadOnly = true;
            editAddress.IsReadOnly = true;

            saveBtn.Visibility = Visibility.Hidden;
            restoreBtn.Visibility = Visibility.Hidden;
            editBtn.Visibility = Visibility.Visible;
        }

        private void editAmount_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {
            editName.IsReadOnly = false;
            editEmail.IsReadOnly = false;
            editPhone.IsReadOnly = false;
            editAddress.IsReadOnly = false;

            saveBtn.Visibility = Visibility.Visible;
            restoreBtn.Visibility = Visibility.Visible;
            editBtn.Visibility = Visibility.Hidden;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            //var select = Dashboard.menuBTN.Children[1] as MenuButton;
            //select?.btn.Focus();
            DataContext = new MainViewModel();
        }

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (!string.IsNullOrEmpty(textBox.Text))
            {
                string digitsOnly = Regex.Replace(textBox.Text, @"\D", "");

                string formattedNumber = Regex.Replace(digitsOnly, @"(\d{3})(\d{3})(\d{4})", "$1-$2-$3");

                // Update the text in the TextBox
                textBox.Text = formattedNumber;
                textBox.SelectionStart = textBox.Text.Length;
            }
        }
    }
}
