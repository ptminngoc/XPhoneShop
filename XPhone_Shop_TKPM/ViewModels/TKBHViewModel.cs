using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.Repositories;

namespace XPhone_Shop_TKPM.ViewModels
{
    class TKBHViewModel : BaseViewModel
    {
        private ProductRepository _repository = new ProductRepository();

        public TKBHViewModel()
        {
        }

        public ObservableCollection<ProductBestSellModel> getTop10BestSell(DateTime start, DateTime end)
        {
            return _repository.getTop10ProductBestSelling(start, end);
        }
    }
}
