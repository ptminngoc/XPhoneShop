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
using System.Windows.Navigation;
using System.Windows.Shapes;
using XPhone_Shop_TKPM.ViewModels;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for QLDHView.xaml
    /// </summary>
    public partial class QLDHView : UserControl
    {
        QLDHViewModel _viewModel = new QLDHViewModel();
        int _currentPage = 1, _rowsPerPage = 10;
        int _totalPage, _listSize;
        bool isFiltering = false;
        DateTime fromDate, toDate;

        public QLDHView()
        {
            InitializeComponent();
            base.DataContext = _viewModel;
        }

        private void viewDetails_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteRowButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void prevButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {

        }

        private void displayRowPerPageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void screen_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void filterDateButton_Click(object sender, RoutedEventArgs e)
        {
            string fromDateString = fromDatePicker.Text;
        }

        private void removeFilterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addNewOrderButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
