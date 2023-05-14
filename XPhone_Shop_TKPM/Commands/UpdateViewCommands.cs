using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using XPhone_Shop_TKPM.ViewModels;
using System.Diagnostics;
using XPhone_Shop_TKPM.Views;

namespace XPhone_Shop_TKPM.Commands
{
    public class UpdateViewCommand : ICommand
    {
        MainViewModel viewModel;
        Window creatingForm;
        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public Window setCreatingForm
        {
            get { return creatingForm; }
            set { creatingForm = value; }
        }

        public void Execute(object parameter)
        {
            //Customer
            if (parameter.ToString() == "HTSP")
            {
                viewModel.SelectedViewModel = new HTSPViewModel();
            }
            else if (parameter.ToString() == "HTDM")
            {
                viewModel.SelectedViewModel = new HTDMViewModel();
            }
            else if (parameter.ToString() == "TTTK")
            {
                viewModel.SelectedViewModel = new TTTKViewModel();
            }
            else if (parameter.ToString() == "DMK")
            {
                viewModel.SelectedViewModel = new DMKViewModel();
            }
            //Admin
            else if (parameter.ToString() == "QLSP")
            {
                viewModel.SelectedViewModel = new QLSPViewModel();
            }
            else if (parameter.ToString() == "QLLOAISP")
            {
                viewModel.SelectedViewModel = new QLLOAISPViewModel();
            }
            else if (parameter.ToString() == "QLKH")
            {
                viewModel.SelectedViewModel = new QLKHViewModel();
            }
            else if (parameter.ToString() == "Cart")
            {
                viewModel.SelectedViewModel = new OrderDetailsViewModel();
            }
            else if (parameter.ToString() == "dang_xuat")
            {
                LoginView screen = new LoginView();
                screen.Show();
                Window myWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.Name == "dashboard");

                // Kiểm tra nếu cửa sổ tồn tại và đang được hiển thị
                if (myWindow != null && myWindow.IsVisible)
                {
                    // Tắt cửa sổ
                    myWindow.Close();
                }
            }
            else if (parameter.ToString() == "QLDH")
            {
                viewModel.SelectedViewModel = new QLDHViewModel();
            }
            else if (parameter.ToString() == "QLKM")
            {
                viewModel.SelectedViewModel = new QLKMViewModel();
            }

        }
    }
}
