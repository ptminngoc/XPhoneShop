using Microsoft.IdentityModel.Tokens;
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
    /// Interaction logic for DMKView.xaml
    /// </summary>
    public partial class DMKView : UserControl
    {
        AccountRepository accountCurRepo = new AccountRepository();
        AccountModel accountCur = new AccountModel();
        public DMKView()
        {
            InitializeComponent();
            accountCur = accountCurRepo.getAccountCurrent();

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

            Global.SaveScreen("DMK");
        }

        private void save_info_click(object sender, RoutedEventArgs e)
        {
            var passwordOld = password_old.Password;
            var passwordNew = password_new.Password;
            var passwordNewConfirm = password_new_confirm.Password;

            var hashPassword = accountCur.AccountPassword;
            if (passwordOld.IsNullOrEmpty() || passwordNew.IsNullOrEmpty())
            {
                MessageBox.Show("Vui lòng không để trống ô điền");
            }
            else if (passwordNewConfirm.IsNullOrEmpty())
            {
                MessageBox.Show("Vui lòng không để trống ô điền");
            }
            else if (!BCrypt.Net.BCrypt.Verify(passwordOld, hashPassword))
            {
                MessageBox.Show("Mật khẩu cũ không đúng");
            }
            else if (passwordNew != passwordNewConfirm)
            {
                MessageBox.Show("Xác nhận mật khẩu mới không khớp");
            }
            else
            {
                //Hash newPassword
                string salt = BCrypt.Net.BCrypt.GenerateSalt();
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(passwordNew, salt);

                accountCurRepo.updatePassword(hashedPassword);

                password_old.Password = "";
                password_new.Password = "";
                password_new_confirm.Password = "";

                MessageBox.Show("Đổi mật khẩu thành công");
            }

        }
    }
}
