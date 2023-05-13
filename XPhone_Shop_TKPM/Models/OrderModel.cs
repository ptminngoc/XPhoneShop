using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPhone_Shop_TKPM.Models
{
    public class OrderModel
    {
        public int? PromotionID { get; set; }
        public int OrderID { get; set; }
        public Double OrderTotal { get; set; }
        public string? OrderStatusDisplayText { get; set; }
        public DateTime? OrderDate { get; set; }
        public int OrderStatus { get; set; }
        public string? CustomerPhone { get; set; }

    }
}
