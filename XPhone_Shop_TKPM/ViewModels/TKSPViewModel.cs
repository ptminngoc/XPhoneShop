using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.Repositories;
using XPhone_Shop_TKPM.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace XPhone_Shop_TKPM.ViewModels
{
    class TKSPViewModel : BaseViewModel
    {
        public ObservableCollection<CategoryTypeStatistic> _categoryList;

        private CategoryTypeStatisticRepository _repository = new CategoryTypeStatisticRepository();

        public TKSPViewModel ()
        {
            //_categoryList = _repository.getAllCategory();
        }

        public ObservableCollection<CategoryTypeStatistic> getAllCategory (DateTime start, DateTime end)
        {
            return _repository.getAllCategory(start, end);
        }
    }
}
