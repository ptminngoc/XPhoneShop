﻿using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPhone_Shop_TKPM.ViewModels
{
    class CTSPViewModel : BaseViewModel
    {
        public ProductModel _product;
        public ProductModel _restoreProduct;

        public ObservableCollection<CategoryModel> _categoryList;

        private CategoryRepository _categoryRepository = new CategoryRepository();
        private ProductRepository _repository = new ProductRepository();

        public CTSPViewModel(int? pId)
        {
            _product = _repository.getProductWithId(pId);
            _restoreProduct = _repository.getProductWithId(pId);
            _categoryList = _categoryRepository.getAllCategory();
        }

        public bool EditProduct(ProductModel editProduct)
        {
            return _repository.editProduct(editProduct);
        }

        public bool RemoveProduct(int? pId)
        {
            return _repository.removeProduct(pId);
        }
    }
}
