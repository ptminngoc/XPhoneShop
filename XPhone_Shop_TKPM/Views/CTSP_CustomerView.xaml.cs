using XPhone_Shop_TKPM.ViewModels;
using DocumentFormat.OpenXml.Vml;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Win32;
using XPhone_Shop_TKPM.Converters;
using XPhone_Shop_TKPM.Models;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using XPhone_Shop_TKPM.UserControls;
using XPhone_Shop_TKPM.ViewModels;
using XPhone_Shop_TKPM.Converters;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for CTSP_CustomerView.xaml
    /// </summary>
    public partial class CTSP_CustomerView : Page
    {
        CTSPViewModel _viewModel = null;
        CartViewModel _viewModelCart = null;
        int _currentCategoryCombobox = 0;
        bool _selected = false;
        ProductModel _product = null;

        public CTSP_CustomerView(int? productID)
        {
            InitializeComponent();

            //productID = pID;
            _viewModel = new CTSPViewModel(productID);
            _viewModelCart = new CartViewModel();
            base.DataContext = _viewModel._product;
            _product = _viewModel._product;
            addToCartQuantityTextBox.Text = "1";

            int i = 0;
            foreach (var category in _viewModel._categoryList)
            {
                if (_viewModel._product.CategoryID == category.CategoryID)
                {
                    comboboxCategory.SelectedIndex = i;
                    _currentCategoryCombobox = i;
                }
                comboboxCategory.Items.Add(category.CategoryName);
                i++;

            }
        }

        private void BtnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn xóa loại sản phẩm này không?",
                "Xóa loại sản phẩm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var remove = _viewModel.RemoveProduct(_viewModel._product.ProductID);
                if (remove)
                {
                    string message = "Đã xóa sản phẩm thành công";
                    string title = "Xóa thông tin sản phẩm";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                    DataContext = new MainViewModel();
                }
            }
        }


        private void restoreBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn khôi phục lại thông tin sản phẩm này không?",
            "Khôi phục",
             MessageBoxButton.YesNo,
             MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                editProductName.Text = _viewModel._restoreProduct.ProductName;
                editProductPrice.Text = _viewModel._restoreProduct.ProductPrice.ToString();
                editProductQuantity.Text = _viewModel._restoreProduct.ProductQuantity.ToString();
                for (int i = 0; i < _viewModel._categoryList.Count; i++)
                {
                    if (_viewModel._categoryList[i].CategoryID == _viewModel._restoreProduct.CategoryID)
                    {
                        _currentCategoryCombobox = i;
                        comboboxCategory.SelectedIndex = i;
                        break;
                    }
                }


                AbsoluteConverter absoluteConverter = new AbsoluteConverter();

                // Convert the relative path to an absolute path
                string imagePath = _viewModel._restoreProduct.ProductAvatar;
                // Relative path of the image
                string absolutePath = (string)absoluteConverter.Convert(imagePath, typeof(string), null, null);
                // Set the absolute path as the source of the Image control
                editProductAvatar.Source = new BitmapImage(new Uri(absolutePath));
                _selected = false;
            }
            editProductName.IsReadOnly = true;
            editProductPrice.IsReadOnly = true;
            editProductQuantity.IsReadOnly = true;
            comboboxCategory.IsEnabled = false;
        }

        private void ComboPageIndex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentCategoryCombobox = comboboxCategory.SelectedIndex;
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

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private void Price_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TextBox textBox = sender as TextBox;

            System.Windows.Controls.TextBox textBox = sender as System.Windows.Controls.TextBox;

            // Chuyển định dạng abc,xyz cho giá cả
            if (textBox.Text.Length > 0)
            {
                double value = 0;
                double.TryParse(textBox.Text, out value);
                textBox.Text = value.ToString("N0");
                textBox.CaretIndex = textBox.Text.Length;
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            var select = Dashboard_Customer.menuBTN.Children[0] as MenuButton;
            select?.btn.Focus();
            DataContext = new MainViewModel();
        }

        private bool addProduct()
        {
            if (addToCartQuantityTextBox.Text == null || addToCartQuantityTextBox.Text.Equals(""))
            {
                MessageBox.Show("Hãy nhập số lượng sản phẩm");
            }
            else
            {
                var isAddSuccess = _viewModelCart.addProductToCart(_product, Int16.Parse(addToCartQuantityTextBox.Text));
                if (isAddSuccess)
                {
                    editProductQuantity.Text = (_product.ProductQuantity - Int16.Parse(addToCartQuantityTextBox.Text)).ToString();
                    MessageBox.Show("Đã thêm sản phẩm vào giỏ hàng");
                    return true;
                }
                else
                {
                    MessageBox.Show("Sản phẩm không đủ số lượng");
                }
            }

            return false;
        }

        private void BtnCartProduct_Click(object sender, RoutedEventArgs e)
        {
            addProduct();
        }

        private void BtnBuyProduct_Click(object sender, RoutedEventArgs e)
        {
            if (addProduct())
                screen.Content = new CartDetailsView();
        }
    }
}
