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

    public class QLLOAISPViewModel : BaseViewModel
    {
        public ObservableCollection<CategoryTypeStatistic> _categoryList;
        public CategoryModel _category = null;
        public ObservableCollection<OrderModel> _orderList;
        private CategoryRepository _repository = new CategoryRepository();
        private OrderRepository _repository2 = new OrderRepository();

        public QLLOAISPViewModel()
        {
            _categoryList = _repository.getCategoryWithProduct();
            _category = new CategoryModel();
            _orderList =  _repository2.getAllOrder();
        }

        public ObservableCollection<CategoryTypeStatistic> getCategory()
        {
            _categoryList = _repository.getCategoryWithProduct();
            return _categoryList;
        }

        public bool AddNewCategory(CategoryModel newCategory)
        {
            return _repository.addCategory(newCategory);
        }

        public bool EditCategory(CategoryModel editCategory)
        {
            return _repository.editCategory(editCategory);
        }

        public bool RemoveCategory(int? cId)
        {
            return _repository.removeCategory(cId);
        }

        public void removeOrderDetail(int id)
        {
            _repository2.deleteOrderId(id);
        }
    }
}
