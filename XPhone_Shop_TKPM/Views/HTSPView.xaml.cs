using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32;
using XPhone_Shop_TKPM.Commands;
using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.UserControls;
using XPhone_Shop_TKPM.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for HTSPView.xaml
    /// </summary>
    public partial class HTSPView : UserControl
    {
        HTSPViewModel _viewModel = new HTSPViewModel();
        int _currentPage = 1, rowsPerPage = 12;
        int _currentCategoryCombobox = 0;

        int _totalPage = 0;
        int _listSize;
        int _productCount = 0;
        bool isFiltering = false;

        public HTSPView()
        {
            InitializeComponent();
            //base.DataContext = _viewModel;

            for (int i = 0; i < Dashboard_Customer.menuBTN.Children.Count; i++)
            {
                if (Dashboard_Customer.menuBTN.Children[i] is MenuButton)
                {
                    var select = Dashboard_Customer.menuBTN.Children[i] as MenuButton;
                    if (select.btn.IsFocused == true)
                        select.isActive = true;
                    else
                        select.isActive = false;
                }
            }

            Global.SaveScreen("HTSP");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            comboboxCategory.Items.Add("Tất cả");
            displayRowPerPageTextBox.Text = rowsPerPage.ToString();

            foreach (var category in _viewModel._categoryList)
            {
                comboboxCategory.Items.Add(category.CategoryName);
            }
            comboboxCategory.Items.Add("Sắp hết hàng");

            updateTotalPage();
            updatePage(_currentCategoryCombobox, 1);

            comboboxCategory.SelectedIndex = 0;
            currentPageComboBox.SelectedIndex = _currentPage - 1;
        }

        private void updateTotalPage()
        {
            _totalPage = _listSize / rowsPerPage + ((_listSize % rowsPerPage) == 0 ? 0 : 1);
            var lines = new List<Tuple<int, int>>();
            for (int i = 1; i <= _totalPage; i++)
            {
                lines.Add(new Tuple<int, int>(i, _totalPage));
            }
            currentPageComboBox.ItemsSource = lines;

        }

        private void updatePage(int category, int page, string keyword = "")
        {
            _currentPage = page;

            if (_currentPage == 1)
            {
                previousButton.IsEnabled = false;
                nextButton.IsEnabled = true;
            }

            else if (_currentPage == _totalPage)
            {
                previousButton.IsEnabled = true;
                nextButton.IsEnabled = false;
            }

            else
            {
                previousButton.IsEnabled = true;
                nextButton.IsEnabled = true;
            }

            List<ProductModel> _productListCategory = new List<ProductModel>();
            List<ProductModel> _newProductList = new List<ProductModel>();
            List<ProductModel> _newProductItem = new List<ProductModel>();

            if (category == 0)
            {
                _productListCategory = _viewModel._productList.ToList();
            }
            else if (category == comboboxCategory.Items.Count - 1)
            {
                _productListCategory = _viewModel._productList.Where(x => x.ProductQuantity < 5).ToList();
            }
            else
            {
                _productListCategory = _viewModel._productList.Where(x => x.CategoryID == category).ToList();

            }
            if (keyword.Length > 0)
            {
                _newProductList = _productListCategory.Where(x => x.ProductName.ToLower().Contains(keyword.ToLower())).ToList();
                if (_newProductList.Count > 0)
                {
                    _newProductItem = _newProductList.Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    _newProductList = _productListCategory.ToList();
                    _newProductItem = _newProductList.Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();
                }

            }
            else
            {
                if (isFiltering)
                {
                    _newProductList = _productListCategory.Where(x => x.ProductPrice >= double.Parse(fromPrice.Text.Replace(",", "")) && x.ProductPrice <= double.Parse(toPrice.Text.Replace(",", ""))).ToList();
                    _newProductItem = _newProductList.Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();

                }
                else
                {
                    _newProductList = _productListCategory.ToList();
                    _newProductItem = _newProductList.Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage).ToList();

                }
            }


            ProductListView.ItemsSource = _newProductItem;
            _listSize = _newProductList.Count;
            _productCount = _newProductItem.Count;
            infoTextBlock.Text = $"Đang hiển thị {_productCount} / {_listSize} sản phẩm";
        }

        private void searchProductButton_Click(object sender, RoutedEventArgs e)
        {
            string keyword = searchProductInput.Text;

            updatePage(_currentCategoryCombobox, 1, keyword);
            updateTotalPage();
            searchProductInput.Clear();

            currentPageComboBox.SelectedIndex = _currentPage - 1;
        }

        private void previosButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                currentPageComboBox.SelectedIndex = _currentPage - 1;
                updatePage(_currentCategoryCombobox, _currentPage);
            }
        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPage)
            {
                _currentPage++;
                currentPageComboBox.SelectedIndex = _currentPage - 1;
                updatePage(_currentCategoryCombobox, _currentPage);
            }
        }

        private void currentPageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (currentPageComboBox.SelectedIndex >= 0)
            {
                _currentPage = currentPageComboBox.SelectedIndex + 1;

                updatePage(_currentCategoryCombobox, _currentPage);
            }
        }

        private void filterPriceButton_Click(object sender, RoutedEventArgs e)
        {
            string fromPriceString = fromPrice.Text;
            string toPriceString = toPrice.Text;

            // check if string is null
            if (fromPriceString == null || fromPriceString == "" || toPriceString == null || toPriceString == "")
            {
                isFiltering = false;
                return;
            }


            isFiltering = true;
            updatePage(_currentCategoryCombobox, 1);
            updateTotalPage();
            currentPageComboBox.SelectedIndex = _currentPage - 1;
            ProductListView.ItemsSource = _viewModel._productList.Where(x => x.ProductPrice >= double.Parse(fromPriceString) && x.ProductPrice <= double.Parse(toPriceString)).Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage);
        }

        private void removeFilterButton_Click(object sender, RoutedEventArgs e)
        {
            isFiltering = false;
            fromPrice.Clear();
            toPrice.Clear();
            updatePage(_currentCategoryCombobox, _currentPage);
            updateTotalPage();
            currentPageComboBox.SelectedIndex = _currentPage - 1;

        }

        private void ComboPageIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentCategoryCombobox = comboboxCategory.SelectedIndex;
            updatePage(_currentCategoryCombobox, _currentPage);
            updateTotalPage();
            currentPageComboBox.SelectedIndex = _currentPage - 1;
        }

        /// Hiệu ứng khi chọn
        private void ComboProductTypes_DropDownOpened(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.Background = Brushes.LightGray;
        }

        /// Hiệu ứng khi bỏ chọn     
        private void ComboProductTypes_DropDownClosed(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.Background = Brushes.Transparent;
        }

        private void ProductListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int index = -1;
            if (_currentPage > 0)
            {
                index = (_currentPage - 1) * rowsPerPage + ProductListView.SelectedIndex;
            }
            else
            {
                index = ProductListView.SelectedIndex;

            }

            //Trace.WriteLine("san pham thu " + index + ", " + _viewModel._productList[index].ProductID);

            nextPage.Content = new CTSP_CustomerView(_viewModel._productList[index].ProductID);
        }

        private void nextPage_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void displayRowPerPageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //if (Int16.Parse(displayRowPerPageTextBox.Text) > rowsPerPage)
                //{
                //    displayRowPerPageTextBox.Text = rowsPerPage.ToString();
                //}
                //else
                //{
                rowsPerPage = Int16.Parse(displayRowPerPageTextBox.Text);
                //}
                updatePage(_currentCategoryCombobox, _currentPage);
                updateTotalPage();
                currentPageComboBox.SelectedIndex = _currentPage - 1;

            }
            catch { }

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Chuyển định dạng abc,xyz cho giá cả
            if (textBox.Text.Length > 0)
            {
                double value = 0;
                double.TryParse(textBox.Text, out value);
                textBox.Text = value.ToString("N0");
                textBox.CaretIndex = textBox.Text.Length;
            }
        }
    }
}
