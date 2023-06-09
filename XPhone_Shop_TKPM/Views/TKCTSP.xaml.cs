﻿using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.UserControls;
using XPhone_Shop_TKPM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for TKCTSP.xaml
    /// </summary>
    public partial class TKCTSP : Page
    {
        TKCTSPViewModel _viewModel = new TKCTSPViewModel();
        DateTime start;
        DateTime end;
        int id;
        string nameCategory;

        private void refresh(DateTime start, DateTime end)
        {
            ObservableCollection<ProductTypeStatisticModel> viewModels = new ObservableCollection<ProductTypeStatisticModel>();
            viewModels = _viewModel.getAllProduct(start, end, id);

            productChart.ItemsSource = viewModels;
            productPieChart.ItemsSource = viewModels;

            productTurnoverChart.ItemsSource = viewModels;
            productTurnoverPieChart.ItemsSource = viewModels;
        }

        public TKCTSP(DateTime _start, DateTime _end, int _id, string _nameCategory)
        {
            start = _start;
            end = _end;
            id = _id;
            nameCategory = _nameCategory;

            InitializeComponent();
            updateDuration(start, end);
            refresh(start, end);
        }

        private void updateDuration(DateTime start, DateTime end)
        {
            string s = start.ToString("dd/MM/yyyy");
            string e = end.ToString("dd/MM/yyyy");

            txtDuration.Content = "Từ ngày " + s + " đến " + e;
            txtTitle.Content = "THỐNG KÊ SẢN PHẨM THUỘC LOẠI " + nameCategory;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            var select = Dashboard_Admin_Sale.subMenuBTN.Children[1] as MenuButton;
            select?.btn.Focus();
            DataContext = new MainViewModel();
        }
    }
}
