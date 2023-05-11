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
using System.Windows.Shapes;
using XPhone_Shop_TKPM.ViewModels;

namespace XPhone_Shop_TKPM
{
    /// <summary>
    /// Interaction logic for Dashboard_Customer.xaml
    /// </summary>
    public partial class Dashboard_Customer : Window
    {
        public Dashboard_Customer()
        {
            MainViewModel current = new MainViewModel();
            DataContext = current;
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void MenuButton_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
