using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPhone_Shop_TKPM.Models
{
    internal class OrderModel
    {

        public int? PromotionID
        {
            get; set;
        }

        public int OrderID
        {
            get; set;
        }

        public DateTime OrderDate
        {
            get; set;
        }

        public String ? OrderStatusDisplayText
        {
            get; set;
        }

        public String? CustomerPhone
        {
            get; set;
        }
       

    }
}
