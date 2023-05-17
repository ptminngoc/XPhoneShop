using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPhone_Shop_TKPM.ViewModels
{
    class TKCTSPViewModel : BaseViewModel
    {
        public ObservableCollection<ProductTypeStatisticModel> _productList;

        private ProductTypeStatisticRepository _repository = new ProductTypeStatisticRepository();

        public TKCTSPViewModel()
        {
            //_categoryList = _repository.getAllCategory();
        }

        public ObservableCollection<ProductTypeStatisticModel> getAllProduct(DateTime start, DateTime end, int id)
        {
            return _repository.getAllProduct(start, end, id);
        }
    }
}
