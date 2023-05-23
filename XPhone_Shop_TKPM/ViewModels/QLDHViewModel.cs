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
    class QLDHViewModel : BaseViewModel
    {
        public ObservableCollection<OrderModel> _orderList;
        public ObservableCollection<OrderDetailsProductModel> _orderDetailList;
        public OrderModel _order;
        public ObservableCollection<CustomerModel> _customerList;
        public OrderDetailsProductModel _orderDetail;
        public ObservableCollection<ProductModel> _productList;
        private OrderRepository _repository = new OrderRepository();
        private OrderDetailsRepository _repository2 = new OrderDetailsRepository();
        private CustomerRepository _repository3 = new CustomerRepository();
        private ProductRepository _repository4 = new ProductRepository();

        private int _OrderID { get; set; }
        private float _OrderTotal { get; set; }
        private DateTime _OrderDate { get; set; }
        private int _OrderStatus { get; set; }
        private int _CustomerPhone { get; set; }
        private int _PromotionID { get; set; }
        private string? _OrderStatusDisplayText { get; set; }


        public QLDHViewModel()
        {
            // query and get all orders
            _orderList = _repository.getAllOrder();
            _order = new OrderModel();  
            _orderDetail = new OrderDetailsProductModel();
            _orderDetailList = _repository2.getAllPurchaseDetail();
            _customerList = _repository3.getAllCustomer();
            _productList = _repository4.getAllProduct();

        }

        public int PromotionID
        {
            get { return _PromotionID; }
            set
            {
                _PromotionID = value;
                OnPropertyChanged("PromotionID");
            }
        }

        public int OrderID
        {
            get { return _OrderID; }
            set
            {
                _OrderID = value;
                OnPropertyChanged("OrderID");

            }
        }
        public float OrderTotal
        {
            get { return _OrderTotal; }
            set
            {
                _OrderTotal = value;
                OnPropertyChanged("OrderTotal");
            }
        }

        public DateTime OrderDate
        {
            get { return _OrderDate; }
            set
            {
                _OrderDate = value;
                OnPropertyChanged("OrderDate");
            }
        }
        public int OrderStatus
        {
            get { return _OrderStatus; }
            set
            {
                _OrderStatus = value;
                OnPropertyChanged("OrderStatus");
            }
        }
        public int CustomerPhone
        {
            get { return _CustomerPhone; }
            set
            {
                _CustomerPhone = value;
                OnPropertyChanged("CustomerPhone");
            }
        }

        public string OrderStatusDisplayText
        {
            get { return _OrderStatusDisplayText; }
            set
            {
                _OrderStatusDisplayText = value;
                OnPropertyChanged("OrderStatusDisplayText");
            }
        }

        // Function
        // remove order at position i (in the list and in the Database)
        public void removeOrder(int id)
        {
            int i = 0;
            for (; i < _orderList.Count; i++)
            {
                if (_orderList[i].OrderID == id)
                    break;
            }
            _repository.deleteOrderId(id);
            _orderList.RemoveAt(i);
        }

        public bool AddNewOrder(OrderModel _newOrder)
        {
            return _repository.addOrder(_newOrder);
        }

        public bool AddNewOrderDetail(OrderDetailsProductModel _newOrder)
        {
            return _repository2.addOrderDetail(_newOrder);
        }

        public void updateOrderDetail(int orderId, int productId, int quantity)
        {
            _repository2.updateProductQuantityInOrderDetail(orderId, productId, quantity);
        }

        public ObservableCollection<OrderModel> getOrdertList()
        {
            _orderList = _repository.getAllOrder();
            return _orderList;
        }
    }
}
