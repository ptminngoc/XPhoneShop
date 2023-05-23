using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.UserControls;
using XPhone_Shop_TKPM.ViewModels;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for QLDHView.xaml
    /// </summary>
    public partial class QLDHView : UserControl
    {
        QLDHViewModel _viewModel = new QLDHViewModel();
        int _currentPage = 1, rowsPerPage = 10;
        int _totalPage, _listSize;
        bool isFiltering = false;
        DateTime fromDate, toDate;

        public QLDHView()
        {
            InitializeComponent();
            base.DataContext = _viewModel;
            for (int i = 0; i < Dashboard_Admin_Sale.menuBTN.Children.Count; i++)
            {
                if (Dashboard_Admin_Sale.menuBTN.Children[i] is MenuButton)
                {
                    var select = Dashboard_Admin_Sale.menuBTN.Children[i] as MenuButton;
                    if (select.btn.IsFocused == true)
                        select.isActive = true;
                    else
                        select.isActive = false;
                }
            }

            for (int i = 0; i < Dashboard_Admin_Sale.subMenuBTN.Children.Count; i++)
            {
                if (Dashboard_Admin_Sale.subMenuBTN.Children[i] is MenuButton)
                {
                    var select_ = Dashboard_Admin_Sale.subMenuBTN.Children[i] as MenuButton;
                    if (select_.btn.IsFocused == true)
                        select_.isActive = true;
                    else
                        select_.isActive = false;
                }
            }

            Global.SaveScreen("QLDH");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // set blackout date from to day --> next day available in the next day
            // according to NOW system date
            fromDatePicker.DisplayDateEnd = DateTime.Now;
            toDatePicker.DisplayDateEnd = DateTime.Now;

            displayRowPerPageTextBox.Text = rowsPerPage.ToString();
            if(_viewModel._orderList.Count > 0)
            {
                updatePage(1);
            }
            updatePage(0);
        }


        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage < _totalPage)
            {
                _currentPage++;
                updatePage(_currentPage);
            }
        }

        // event handler
        private void prevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                updatePage(_currentPage);
            }
        }

        // event handler
        private void deleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            var select = Dashboard_Admin_Sale.menuBTN.Children[2] as MenuButton;
            select?.btn.Focus();

            Button button = (Button)sender;
            var currrentItem = (OrderModel)button.DataContext;

            _viewModel.removeOrder(currrentItem.OrderID);

            lst.ClearValue(ItemsControl.ItemsSourceProperty);

            updatePage(_currentPage);
        }

        // event handler
        private void displayRowPerPageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                rowsPerPage = Int16.Parse(displayRowPerPageTextBox.Text);

                updatePage(_currentPage);
            }
            catch { }
        }


        private void removeFilterButton_Click(object sender, RoutedEventArgs e)
        {
            isFiltering = false;
            updatePage(_currentPage);
        }

        // filter orders by date button
        private void filterDateButton_Click(object sender, RoutedEventArgs e)
        {
            string fromDateString = fromDatePicker.Text;
            string toDateString = toDatePicker.Text;

            // check if string is null
            if (fromDateString == null || fromDateString == "" || toDateString == null || toDateString == "")
            {
                isFiltering = false;
                return;
            }

            fromDate = DateTime.Parse(fromDateString + " 12:00");
            toDate = DateTime.Parse(toDateString + " 12:00");


            //MessageBox.Show($"{fromDate.ToString()}\n{toDate.ToString()}");

            isFiltering = true;

            updatePage(1);
            lst.ItemsSource = _viewModel._orderList.Where(x => x.OrderDate >= fromDate.Date && x.OrderDate <= toDate.Date).Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage);
        }

        private void viewDetails_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var order = (OrderModel)button.DataContext;

            Global.selectedOrderID = order.OrderID;
            screen.Content = new OrderDetailsView();
            //selectedOrderDetails.ShowDialog();

            //_viewModel = new QLDHViewModel();

            //updatePage(_currentPage);
        }

        private void screen_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void importPurcahseButton_Click(object sender, RoutedEventArgs e)
        {
            // Mở hộp thoại mở tập tin
           if(_viewModel._productList.Count == 0) {
                string title = "Import";
                string message = "Danh sách sản phẩm đang trống. Vui lòng import danh sách sản phẩm trước!";
                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
           }
           else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    string filename = openFileDialog.FileName;
                    // Do something with the selected file path

                    //try
                    //{
                        var document = SpreadsheetDocument.Open(filename, false);
                        var wbPart = document.WorkbookPart!;
                        var sheets = wbPart.Workbook.Descendants<Sheet>()!;
                        var sheet = sheets.FirstOrDefault(
                            s => s.Name == "Purchase");
                        if (sheet == null)
                        {
                            {
                                string title = "Import purchase từ Excel";
                                string message = "Import không thành công, File excel không có sheet là Purchase";
                                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                        var wsPart = (WorksheetPart)(wbPart!.GetPartById(sheet.Id!));
                        var cells = wsPart.Worksheet.Descendants<Cell>();

                        int row = 2;
                        Cell idPromotionCell;
                        int countAdd = 0;
                        foreach (var order in _viewModel._orderList)
                        {
                            if (order.PromotionID == null)
                            {
                                order.PromotionID = 0;
                            }
                        }
                        do
                        {
                            idPromotionCell = cells.FirstOrDefault(
                                c => c?.CellReference == $"A{row}"
                            )!;

                            if (idPromotionCell?.InnerText.Length > 0)
                            {
                                string id = idPromotionCell!.InnerText;

                                //Total
                                Cell totalCell = cells.FirstOrDefault(
                                    c => c?.CellReference == $"B{row}"
                                )!;
                                string total = totalCell!.InnerText;

                                // Date
                                Cell dateCell = cells.FirstOrDefault(c => c?.CellReference == $"C{row}")!;
                                string stringDate = dateCell!.InnerText;

                                double excelDateValue = double.Parse(stringDate);
                                DateTime date = DateTime.FromOADate(excelDateValue);
                                Debug.WriteLine(date);

                                //Status
                                Cell statusCell = cells.FirstOrDefault(
                                        c => c?.CellReference == $"D{row}"
                                    )!;
                                string status = statusCell!.InnerText;

                                //Customer phone 
                                Cell phoneCell = cells.FirstOrDefault(
                                    c => c?.CellReference == $"E{row}"
                                )!;
                                string stringPhone = phoneCell!.InnerText;
                                var stringPhoneTable = wbPart
                                .GetPartsOfType<SharedStringTablePart>()
                                     .FirstOrDefault()!;
                                string phone = stringPhoneTable.SharedStringTable
                                     .ElementAt(int.Parse(stringPhone)).
                                InnerText;

                                //Product_id
                                Cell productIdCell = cells.FirstOrDefault(
                                   c => c?.CellReference == $"F{row}"
                               )!;
                                string productId = productIdCell!.InnerText;

                                //Quantity
                                Cell quantityCell = cells.FirstOrDefault(
                                        c => c?.CellReference == $"G{row}"
                                    )!;
                                string quantity = quantityCell!.InnerText;

                                bool orderExists = _viewModel._orderList
                                    .Where(p => p.OrderTotal != null && p.OrderDate != null && p.OrderStatus != null && p.CustomerPhone != null) // Filter products based on criteria
                                    .Any(p => p.PromotionID == int.Parse(id) && p.OrderTotal == int.Parse(total) && p.OrderDate == date && p.CustomerPhone == phone); // Check if desired product exists in filtered array

                                if (!orderExists)
                                {
                                     bool isPhoneExists = _viewModel._customerList.Any(customer => customer.phone == phone);
                                     bool isProductExists = _viewModel._productList.Any(product => product.ProductID == int.Parse(productId));
                                    if (isPhoneExists) 
                                    {  
                                        _viewModel._order.PromotionID = int.Parse(id);
                                        _viewModel._order.OrderTotal = int.Parse(total);
                                        _viewModel._order.OrderDate = date;
                                        _viewModel._order.OrderStatus = int.Parse(status);
                                        _viewModel._order.CustomerPhone = phone;

                                        var add = _viewModel.AddNewOrder(_viewModel._order);
                                        if (add)
                                        {
                                            countAdd++;
                                            _viewModel._orderDetail.productId = int.Parse(productId);
                                            _viewModel._orderDetail.ProductQuantity = int.Parse(quantity);

                                            bool purchaseDetailExists = _viewModel._orderDetailList
                                                .Where(p => p.purchaseDetailId != null && p.productId != null && p.ProductQuantity != null) // Filter products based on criteria
                                                .Any(p => p.purchaseDetailId == int.Parse(id) && p.productId == int.Parse(productId) && p.ProductQuantity == int.Parse(quantity)); // Check if desired product exists in filtered array

                                            if (purchaseDetailExists)
                                            {
                                                _viewModel.updateOrderDetail(int.Parse(id), int.Parse(productId), int.Parse(quantity));
                                            }
                                            else
                                            {
                                                if (isProductExists)
                                                {
                                                    _viewModel.AddNewOrderDetail(_viewModel._orderDetail);
                                                }
                                            }
                                        }
                                    }
                                }
                                row++;
                            }

                        } while (idPromotionCell?.InnerText.Length > 0);

                        Console.ReadLine();
                        lst.ItemsSource = _viewModel.getOrdertList();

                        if (countAdd > 0)
                        {
                            string title = "Import đơn hàng từ Excel" + countAdd;
                            string message = "Đã thêm thành công " + countAdd + " đơn hàng mới";
                            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            string title = "Import đơn hàng từ Excel";
                            string message = "Không có dữ liệu hoặc dữ liệu đã tồn tại trong cơ sở dữ liệu";
                            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    //catch (Exception ex)
                    //{
                    //    string title = "Import đơn hàng từ Excel";
                    //    string message = "Import không thành công";
                    //    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                //}
                if(_viewModel._orderList.Count > 0)
                {
                    updatePage(0);
                }
                else
                {
                    updatePage(1);
                }
            }
        }

        private void addNewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            screen.Content = new AddNewOrderView();
        }


        // update the paging system
        // assign new itemsource for listview
        private void updatePage(int page)
        {
            _currentPage = page;

            if (isFiltering)
            {
                _listSize = _viewModel._orderList.Where(x => x.OrderDate >= fromDate.Date && x.OrderDate <= toDate.Date).ToList().Count;
                lst.ItemsSource = _viewModel._orderList.Where(x => x.OrderDate >= fromDate.Date && x.OrderDate <= toDate.Date).Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage);
            }
            else
            {
                _listSize = _viewModel._orderList.Count;
                lst.ItemsSource = _viewModel._orderList.Skip((_currentPage - 1) * rowsPerPage).Take(rowsPerPage);
            }

            _totalPage = _listSize / rowsPerPage + ((_listSize % rowsPerPage) == 0 ? 0 : 1);
            pageCountLabel.Content = $"{_currentPage}/{_totalPage}";

            if (_currentPage == 1)
            {
                if (_totalPage == _currentPage)
                {
                    prevButton.IsEnabled = false;
                    nextButton.IsEnabled = false;
                }
                else
                {
                    prevButton.IsEnabled = false;
                    nextButton.IsEnabled = true;
                }
            }

            else if (_currentPage == _totalPage)
            {

                prevButton.IsEnabled = true;
                nextButton.IsEnabled = false;

            }
            else
            {
                prevButton.IsEnabled = true;
                nextButton.IsEnabled = true;
            }
        }

        // Make sure the input are all numbers
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
