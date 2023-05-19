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

            if (currentCartId == 0)
            {
                _cartViewModel.createCart();
                currentCartId = _cartViewModel.getCartID();
            }
            _viewModel = new OrderDetailsViewModel(currentCartId);

            // get order's customer info
            customer = _viewModel.getCustormerFromDB(currentCartId);

            List<PromotionModel> promoListForCustomer = filterListPromotionforCustomer();
            promotionCombobox.ItemsSource = promoListForCustomer;

            int? promoID = _viewModel.getPromotionID(currentCartId);
            if (promoID == null)
            {
                promotionCombobox.SelectedIndex = 0;
            }
            else
            {
                for (int i = 0; i < promoListForCustomer.Count; ++i)
                    if (promoID == promoListForCustomer[i]._promotionId)
                    {
                        promotionCombobox.SelectedIndex = i;
                        break;
                    }
            }

            currentPromo = promotionCombobox.SelectedItem as PromotionModel;

            updateMoneyTextBlock();

            Global.SaveScreen("Cart");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lst.ItemsSource = _viewModel.productList;
           
        }

        void setPromotion ()
        {
            promotionCombobox.ItemsSource = filterListPromotionforCustomer();
            promotionCombobox.SelectedIndex = 0;
        }

        // filter promotion
        private List<PromotionModel> filterListPromotionforCustomer ()
        {
            List<PromotionModel> promoList = _viewModel.getPromotionList();
            List<PromotionModel> promoListNew = new List<PromotionModel>();
            promoListNew.Add(promoList[0]);
            double total = _viewModel.calculateTotalMoney(0F);
            for (int i = 1; i < promoList.Count; ++i)
            {
                if (total <= 50000000) {
                    if (promoList[i]._promotionPercentage <= 10)
                        promoListNew.Add(promoList[i]);
                } 
                else if (total <= 80000000)
                {
                    if (promoList[i]._promotionPercentage <= 20)
                        promoListNew.Add(promoList[i]);
                }
                else if (total <= 100000000)
                {
                    if (promoList[i]._promotionPercentage <= 100)
                        promoListNew.Add(promoList[i]);
                }

            }
            return promoListNew;
        }

        // delete product from list + DB
        private void deleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var currentItem = (OrderDetailsProductModel)button.DataContext;

            _viewModel.removeProductFromList((int)currentItem.ProductID, currentCartId);
            setPromotion();
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
                MessageBox.Show("Số lượng sản phẩm không đủ");
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

        private void promotionCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentPromo = (PromotionModel)promotionCombobox.SelectedItem;
            if (currentPromo == null)
            {
                promotionCombobox.SelectedIndex = 0;
                currentPromo = (PromotionModel)promotionCombobox.SelectedItem;
            }
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
            
            for (int i = 0; i < listStatus.Count; i++)
            {
                Debug.WriteLine(listStatus[i].displayText);
                if (listStatus[i].displayText.Equals("Mới tạo"))
                {
                    _viewModel.updateStatus(currentCartId, i + 1);
                    MessageBox.Show("Bạn đã đặt đơn hàng thành công");
                    screen.Content = new CartDetailsView();
                }
            }
        }
    }
}
