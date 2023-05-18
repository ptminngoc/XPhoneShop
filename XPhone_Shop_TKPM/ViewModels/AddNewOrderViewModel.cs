using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.Repositories;

namespace XPhone_Shop_TKPM.ViewModels
{
    class AddNewOrderViewModel : BaseViewModel
    {
        private OrderDetailsRepository _repoDetail;
        private NewOrderRepository _repoNew;

        public AddNewOrderViewModel()
        {
            _repoDetail = new OrderDetailsRepository();
            _repoNew = new NewOrderRepository();
        }

        public List<Status> getStatusList()
        {
            return _repoDetail.getOrderStatusList();
        }

        public List<PromotionModel> getPromotionList()
        {
            return _repoDetail.getPromotionListFromDB();
        }

        public void addNewOrder(OrderModel newOrder)
        {
            _repoNew.addNewOrder(newOrder);
        }

        public Boolean addCustomer(CustomerModel newCustomer)
        {
            return _repoNew.addNewCustomer(newCustomer);
        }

        public void updateCustomer(CustomerModel newCustomer)
        {
            _repoNew.updateCustomer(newCustomer);
        }
    }
}
