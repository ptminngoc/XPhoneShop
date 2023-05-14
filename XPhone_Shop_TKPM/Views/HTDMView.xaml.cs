using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.Repositories;
using XPhone_Shop_TKPM.UserControls;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for HTDMView.xaml
    /// </summary>
    public partial class HTDMView : UserControl
    {
        OrderProductDetailRepository orderProductDetailRepository = new OrderProductDetailRepository();
        ObservableCollection<OrderProductDetailModel> order = new ObservableCollection<OrderProductDetailModel>();
        int index = -1;
        public HTDMView()
        {
            InitializeComponent();

            order = orderProductDetailRepository.getAll();
            DonMuaListView.ItemsSource = order;

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

            Global.SaveScreen("HTDM");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            index = int.Parse(((Button)e.Source).Uid);
            GridCursor.Margin = new System.Windows.Thickness(10 + (150 * index), 0, 0, 0);

            switch (index)
            {
                case 0:
                    order = orderProductDetailRepository.getAll();
                    DonMuaListView.ItemsSource = order;
                    break;
                case 1:
                    order = orderProductDetailRepository.getMoiTao();
                    DonMuaListView.ItemsSource = order;
                    break;
                case 2:
                    order = orderProductDetailRepository.getDangGiao();
                    DonMuaListView.ItemsSource = order;
                    break;
                case 3:
                    order = orderProductDetailRepository.getHoanThanh();
                    DonMuaListView.ItemsSource = order;
                    break;
                case 4:
                    order = orderProductDetailRepository.getDaHuy();
                    DonMuaListView.ItemsSource = order;
                    break;
            }
        }

        private void cancel_click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var currrentItem = (OrderProductDetailModel)button.DataContext;
            Debug.WriteLine(button.Content.ToString());

            int i = 0;
            for (; i < order.Count; i++)
            {
                if (order[i].OrderID == currrentItem.OrderID)
                    break;
            }

            orderProductDetailRepository.cancelOrderId(currrentItem.OrderID);

            order[i].OrderStatus = 3;
            order[i].OrderStatusDisplayText = "Đã hủy";

            if (index == 1)
            {
                order.RemoveAt(i);
            }
        }
    }
}
