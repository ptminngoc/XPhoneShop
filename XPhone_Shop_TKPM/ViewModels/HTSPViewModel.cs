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
    class HTSPViewModel : BaseViewModel
    {
        public ObservableCollection<ProductModel> _productList;
        public ObservableCollection<CategoryModel> _categoryList;
        public ProductModel _product;
        private ProductRepository _repository = new ProductRepository();
        private CategoryRepository _repository2 = new CategoryRepository();

        public HTSPViewModel()
        {
            // query and get all orders
            _productList = _repository.getAllProduct();
            _categoryList = _repository2.getAllCategory();
            _product = new ProductModel();
        }

        public ObservableCollection<ProductModel> getProductList()
        {
            _productList = _repository.getAllProduct();
            return _productList;
        }

        public bool AddNewProduct(ProductModel product)
        {
            return _repository.addProduct(product);
        }
    }
}
