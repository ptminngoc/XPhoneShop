﻿using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPhone_Shop_TKPM.Models;
using XPhone_Shop_TKPM.Repositories;

namespace XPhone_Shop_TKPM.ViewModels
{
    class CartViewModel : BaseViewModel
    {
        private CartRepository _repository = new CartRepository();
        public bool isAddSuccess = false;

        public CartViewModel()
        {
            isAddSuccess = false;
        }

        public int getCartID () { 
            return _repository.getCartID();
        }

        public Boolean addProductToCart (ProductModel p, int quantity)
        {
            return _repository.addProductToCart(p, quantity);
        }

        public void createCart ()
        {
            _repository.createCart();
        }

        public Boolean addProductToNewOrder(ProductModel p, int quantity)
        {
            return _repository.addProductToNewOrder(p, quantity);
        }
    }
}
