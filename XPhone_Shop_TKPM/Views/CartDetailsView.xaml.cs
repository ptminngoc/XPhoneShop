using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.UserControls;
using XPhone_Shop_TKPM.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;
using XPhone_Shop_TKPM.Models;
using System.Diagnostics;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for CartDetailsView.xaml
    /// </summary>
    public partial class CartDetailsView : UserControl
    {
        int currentCartId = 0;
        OrderDetailsViewModel _viewModel;
        CartViewModel _cartViewModel;
        CustomerModel customer;
        PromotionModel? currentPromo;
        public CartDetailsView()
        {
            InitializeComponent();
            _cartViewModel = new CartViewModel();
            currentCartId = _cartViewModel.getCartID();

            var select = Dashboard_Customer.menuBTN.Children[1] as MenuButton;
            select?.btn.Focus();

            if (currentCartId == 0)
            {
                _cartViewModel.createCart();
                currentCartId = _cartViewModel.getCartID();
            }
            _viewModel = new OrderDetailsViewModel(currentCartId);

            // get order's customer info
            customer = _viewModel.getCustormerFromDB(currentCartId);


            orderStatusComboBox.ItemsSource = _viewModel.orderStatusList();
            orderStatusComboBox.SelectedIndex = _viewModel.getOrderStatusKey(currentCartId) - 1;

            List<PromotionModel> promoList = _viewModel.getPromotionList();
            promotionCombobox.ItemsSource = promoList;

            int? promoID = _viewModel.getPromotionID(currentCartId);
            if (promoID == null)
            {
                promotionCombobox.SelectedIndex = 0;
            }
            else
            {
                for (int i = 0; i < promoList.Count; ++i)
                    if (promoID == promoList[i]._promotionId)
                    {
                        promotionCombobox.SelectedIndex = i;
                        break;
                    }
            }

            currentPromo = promotionCombobox.SelectedItem as PromotionModel;

            updateMoneyTextBlock();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lst.ItemsSource = _viewModel.productList;
        }

        // delete product from list + DB
        private void deleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var currentItem = (OrderDetailsProductModel)button.DataContext;

            _viewModel.removeProductFromList((int)currentItem.ProductID, currentCartId);

            updateMoneyTextBlock();
        }

        // edit quantity of a product in list + DB
        private void editQuantityButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var currentItem = (OrderDetailsProductModel)button.DataContext; // get selected row data
            int id = (int)currentItem.ProductID;

            if (!_viewModel.updateProductQuantity(currentCartId, id, currentItem.orderQuantity, currentItem.ProductQuantity))
            {
                MessageBox.Show("Not enough product in stock");
            }
            else
            {
                updateMoneyTextBlock();
            }
        }

        private async void addNewProductButton_Click(object sender, RoutedEventArgs e)
        {
            screen.Content = new CartDetailChooseProductView();
        }

        private void orderStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _viewModel.updateStatus(currentCartId, orderStatusComboBox.SelectedIndex + 1);
        }

        private void promotionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPromo = (PromotionModel)promotionCombobox.SelectedItem;

            _viewModel.updatePromo(currentCartId, currentPromo._promotionId);

            // update current promo

            updateMoneyTextBlock();
        }

        private void updateMoneyTextBlock()
        {
            double newTotal = _viewModel.calculateTotalMoney(currentPromo!._promotionPercentage);
            totalMoneyTextBlock.Text = toVndCurrency(newTotal);

            _viewModel.updateTotal(currentCartId, newTotal);
        }

        private string toVndCurrency(double total)
        {
            CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");   // try with "en-US"

            return total.ToString("N0", cul) + " VND";
        }

        private void checkoutButton_Click(object sender, RoutedEventArgs e)
        {
            var listStatus = _viewModel.orderStatusList();
            Debug.WriteLine(listStatus.Count);
            for (int i = 0; i < listStatus.Count; i++)
            {
                if (listStatus[i].displayText.Equals("Đã thanh toán"))
                {
                    _viewModel.updateStatus(currentCartId, i);
                    orderStatusComboBox.SelectedIndex = i;
                }
            }
        }
    }
}
