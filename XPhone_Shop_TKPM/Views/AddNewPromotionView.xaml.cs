using System;
using System.Collections.Generic;
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
using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.UserControls;
using XPhone_Shop_TKPM.ViewModels;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for AddNewPromotionView.xaml
    /// </summary>
    public partial class AddNewPromotionView : Page
    {
        AddNewPromotionViewModel _viewModel;
        public AddNewPromotionView()
        {
            InitializeComponent();

            _viewModel = new AddNewPromotionViewModel();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void numberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = promoNameTextBox.Text;
            string percentString = promoPercentageTextBox.Text;

            if (name == null || name == "" ||
                percentString == null || percentString == "")
            {
                MessageBox.Show("Vui lòng điền đầy đủ tất cả thông tin của khuyến mãi");
                return;
            }

            if (double.Parse(percentString) > 100)
            {
                MessageBox.Show("Khuyến mãi không được lớn hơn 100%");
                return;
            }

            PromotionModel newPromo = new PromotionModel()
            {
                _promotionName = name,
                _promotionPercentage = double.Parse(percentString)
            };

            _viewModel.addNewPromo(newPromo);
            DataContext = new MainViewModel();
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            var select = Dashboard_Admin_Sale.menuBTN.Children[5] as MenuButton;
            select?.btn.Focus();
            DataContext = new MainViewModel();
            DataContext = new MainViewModel();
        }
    }
}
