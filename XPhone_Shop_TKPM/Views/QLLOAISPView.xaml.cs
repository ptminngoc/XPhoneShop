using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Win32;
using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.UserControls;
using XPhone_Shop_TKPM.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for QLLOAISPView.xaml
    /// </summary>
    public partial class QLLOAISPView : UserControl
    {
        QLLOAISPViewModel _viewModel = new QLLOAISPViewModel();
        int index = -1;

        public QLLOAISPView()
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

            Global.SaveScreen("QLLOAISP");
        }

        private void lst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lst.ItemsSource = _viewModel._categoryList;
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var category = (CategoryTypeStatistic)button.DataContext;

            index = category.id;

            _viewModel._category.CategoryID = category.id;

            addCategoryName.Text = category.name;
            saveCategoryButton.Visibility = Visibility.Visible;
            addCategoryButton.Visibility = Visibility.Hidden;
        }

        private void deleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var category = (CategoryTypeStatistic)button.DataContext;

            index = category.id;
            if (MessageBox.Show("Bạn muốn xóa loại sản phẩm này không?",
                "Xóa",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (index >= 0)
                {
                
                    if (category.numOfProduct > 0)
                    {
                        if (MessageBox.Show("Tồn tại sản phẩm thuộc loại sản phẩm này" + "\nXóa tất cả sản phẩm đó?",
                            "Tồn tại sản phẩm",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            var remove = _viewModel.RemoveCategory(category.id);
                            if (remove)
                            {
                                string title = "Xóa";
                                string message = "Xóa loại sản phẩm thành công";
                                MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                        }
                    }

                    else
                    {
                        var remove = _viewModel.RemoveCategory(category.id);
                        if (remove)
                        {
                            string title = "Xóa";
                            string message = "Xóa loại sản phẩm thành công";
                            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }

            addCategoryName.Clear();
            lst.ItemsSource = _viewModel.getCategory();
        }

        private void addCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thêm mới một loại sản phẩm ?",
                "Thêm mới",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (addCategoryName.Text.Length == 0)
                {
                    string message = "Vui lòng điền tên loại sản phẩm!";
                    string title = "kiểm tra nhập thông tin";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    _viewModel._category.CategoryName = addCategoryName.Text;
                    var add = _viewModel.AddNewCategory(_viewModel._category);
                    if (add)
                    {
                        string message = "Thêm";
                        string title = "Thêm loại sản phẩm thành công";
                        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            addCategoryName.Clear();
            lst.ItemsSource = _viewModel.getCategory();
        }

        private void saveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn hiệu chỉnh lại loại sản phẩm này?",
                "Hiệu chỉnh",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                if (addCategoryName.Text.Length == 0)
                {
                    string title = "Chưa điền tên";
                    string message = "Vui lòng điền tên loại sản phẩm";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    if(index >= 0)
                    {
                    
                        _viewModel._category.CategoryName = addCategoryName.Text;

                        var edit = _viewModel.EditCategory(_viewModel._category);
                        if (edit)
                        {
                            string title = "Hiệu chỉnh";
                            string message = "Hiệu chỉnh loại sản phẩm thành công";
                            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);                  
                        }
                    }
                }
            }
            addCategoryName.Clear();
            saveCategoryButton.Visibility = Visibility.Hidden;
            addCategoryButton.Visibility = Visibility.Visible;
            lst.ItemsSource = _viewModel.getCategory();
        }

        private void importProductButton_Click(object sender, RoutedEventArgs e)
        {
            // Mở hộp thoại mở tập tin
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All Files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileName;
                // Do something with the selected file path

                try
                {
                    var document = SpreadsheetDocument.Open(filename, false);
                    var wbPart = document.WorkbookPart!;
                    var sheets = wbPart.Workbook.Descendants<Sheet>()!;
                    var sheet = sheets.FirstOrDefault(
                        s => s.Name == "Category");
                    if(sheet == null )
                    {
                        {
                            string title = "Import category từ Excel";
                            string message = "Import không thành công, File excel không có sheet là Category";
                            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                    }
                    var wsPart = (WorksheetPart)(wbPart!.GetPartById(sheet.Id!));
                    var cells = wsPart.Worksheet.Descendants<Cell>();

                    int row = 2;
                    Cell nameCell;

                    int countAdd = 0;
                    foreach (var category in _viewModel._categoryList)
                    {
                        _viewModel.RemoveCategory(category.id);
                    }
                    do
                    {
                        nameCell = cells.FirstOrDefault(
                            c => c?.CellReference == $"A{row}"
                        )!;

                        if (nameCell?.InnerText.Length > 0)
                        {
                            //string id = idCell.InnerText;
                            //Cell nameCell = cells.FirstOrDefault(
                            //    c => c?.CellReference == $"A{row}"
                            //)!;
                            string stringId = nameCell!.InnerText;
                            var stringTable = wbPart
                            .GetPartsOfType<SharedStringTablePart>()
                                 .FirstOrDefault()!;
                            string name = stringTable.SharedStringTable
                                 .ElementAt(int.Parse(stringId)).
                            InnerText;

                            bool isExist = false;

                             _viewModel._category.CategoryName = name;
                             var add = _viewModel.AddNewCategory(_viewModel._category);
                            if (add)
                            {
                                 countAdd++;
                            }
                            Trace.WriteLine($"{name}");
                        }
                        row++;

                    } while (nameCell?.InnerText.Length > 0);
                    lst.ItemsSource = _viewModel.getCategory();
                    Console.ReadLine();

                    if (countAdd > 0)
                    {
                        string title = "Import category từ Excel" + countAdd;
                        string message = "Đã thêm thành công " + countAdd + " loại sản phẩm mới";
                        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        string title = "Import category từ Excel";
                        string message = "Không có dữ liệu hoặc dữ liệu đã tồn tại trong cơ sở dữ liệu";
                        MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    string title = "Import category từ Excel";
                    string message = "Import không thành công";
                    MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
