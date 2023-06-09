﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPhone_Shop_TKPM.Models
{
    public class ProductModel : INotifyPropertyChanged, ICloneable
    {
        public int? ProductID { get; set; }
        public int? CategoryID { get; set; }
        public string ProductName { get; set; }
        public string ProductAvatar { get; set; }
        public int ProductQuantity { get; set; }
        public double ProductPrice { get; set; }
        public double ProductPriceOriginal { get; set; }


        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
