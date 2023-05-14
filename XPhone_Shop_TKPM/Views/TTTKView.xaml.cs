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
using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.Repositories;
using XPhone_Shop_TKPM.UserControls;

namespace XPhone_Shop_TKPM.Views
{
    /// <summary>
    /// Interaction logic for TTTKView.xaml
    /// </summary>
    public partial class TTTKView : UserControl
    {
        AccountRepository accountCurRepo = new AccountRepository();
        AccountModel accountCur = new AccountModel();
        public TTTKView()
        {
            InitializeComponent();
            accountCur = accountCurRepo.getAccountCurrent();

            username.Text = accountCur.AccountUsername;
            hovaten.Text = accountCur.AccountName;
            email.Text = accountCur.AccountEmail;
            diachi.Text = accountCur.AccountAddress;
            sodienthoai.Text = accountCur.AccountTelephone;

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

            Global.SaveScreen("TTTK");
        }

        private void save_info_click(object sender, RoutedEventArgs e)
        {
            var nameAccount = hovaten.Text;
            var emailAccount = email.Text;
            var addressAccount = diachi.Text;

            if (nameAccount == null || emailAccount == null || addressAccount == null)
            {
                MessageBox.Show("Hãy cập nhật đầy đủ thông tin");
            }
            else
            {
                accountCurRepo.updateAccount(nameAccount, emailAccount, addressAccount);
                MessageBox.Show("Cập nhật thông tin thành công");
            }
        }
    }
}
