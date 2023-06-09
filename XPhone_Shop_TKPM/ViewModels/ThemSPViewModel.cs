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
    class ThemSPViewModel : BaseViewModel
    {
        private ProductRepository _repository = new ProductRepository();
        public ProductModel _product = null;
        public ObservableCollection<CategoryModel> _categoryList;

        private CategoryRepository _categoryRepository = new CategoryRepository();
        public ThemSPViewModel()
        {
            _categoryList = _categoryRepository.getAllCategory();
            _product = new ProductModel();
        }

        public bool AddNewProduct(ProductModel product)
        {
            return _repository.addProduct(product);
        }
    }
}

