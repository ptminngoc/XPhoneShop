using MaterialDesignThemes.Wpf;
using Microsoft.Data.SqlClient;
using XPhone_Shop_TKPM.Commands;
using XPhone_Shop_TKPM.UserControls;
using XPhone_Shop_TKPM.ViewModels;
using XPhone_Shop_TKPM.Views;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace XPhone_Shop_TKPM
{
    /// <summary>
    /// Interaction logic for Dashboard_Customer.xaml
    /// </summary>
    public partial class Dashboard_Customer : Window
    {
        public static StackPanel menuBTN = null;
        public Dashboard_Customer()
        {
            MainViewModel current = new MainViewModel();
            DataContext = current;
            InitializeComponent();

            menuBTN = menu;
            var select = menu.Children[0] as MenuButton;
            select?.btn.Focus();
            select.btn.Command = current.UpdateViewCommand;
            select?.btn.Command.Execute("HTSP");

            for (int i = 0; i < menuBTN.Children.Count; i++)
            {
                if (menu.Children[i] is MenuButton)
                {
                    var select_ = menu.Children[i] as MenuButton;
                    if (select_.btn.IsFocused == true)
                        select_.isActive = true;
                    else
                        select_.isActive = false;
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        bool IsMaximized = false;
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                if (IsMaximized)
                {
                    this.WindowState = WindowState.Normal;
                    this.Width = 1200;
                    this.Height = 750;

                    IsMaximized = false;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                    IsMaximized = true;

                }
            }
        }

        private void MenuButton_Loaded(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuButton;
            Debug.WriteLine(item.btn.IsFocused);
        }
    }
}
