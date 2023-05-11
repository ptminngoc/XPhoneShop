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
    class OrderDetailChooseProductViewModel : BaseViewModel
    {
        public ObservableCollection<ProductModel> _productList = new ObservableCollection<ProductModel>();
        private OrderDetailChooseProductRepository _repo = new OrderDetailChooseProductRepository();
        private int currentOrderID = Global.selectedOrderID;

        public OrderDetailChooseProductViewModel()
        {
            _productList = _repo.getAllProductNotInOrder(currentOrderID);
        }

        public string convertToAbsolute(string relative)
        {
            string folder = AppDomain.CurrentDomain.BaseDirectory;
            string absolute = $"{folder}{relative}";

            return absolute;
        }

        public Boolean addNewProduct(int productID, int userInputQuantity, int currentStockQuantity)
        {
            // check if stock is available
            if (userInputQuantity > currentStockQuantity)
                return false;

            _repo.addProductToOrderDetail(currentOrderID, productID, userInputQuantity);
            _repo.updateProductQuantity(productID, currentStockQuantity - userInputQuantity);

            return true;
        }

        public Boolean addNewProductToCart(int CartID, int productID, int userInputQuantity, int currentStockQuantity)
        {
            // check if stock is available
            if (userInputQuantity > currentStockQuantity)
                return false;

            _repo.addProductToOrderDetail(CartID, productID, userInputQuantity);
            _repo.updateProductQuantity(productID, currentStockQuantity - userInputQuantity);

            return true;
        }
    }
}
