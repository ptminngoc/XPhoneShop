using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPhone_Shop_TKPM.Models
{
    public class OrderProductDetailModel : INotifyPropertyChanged, ICloneable
    {
        public int? PromotionID { get; set; }
        public int OrderID { get; set; }
        public Double OrderTotal { get; set; }
        public string? OrderStatusDisplayText { get; set; }
        public DateTime? OrderDate { get; set; }
        public int OrderStatus { get; set; }
        public string? CustomerPhone { get; set; }

        public string? IsShipping { get; set; }

        public string? IsNew { get; set; }

        public ObservableCollection<ProductModel>? ProductDetail { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
