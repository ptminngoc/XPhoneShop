using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPhone_Shop_TKPM.Models
{
    class OrderDetailsProductModel : ProductModel
    {
        public int purchaseDetailId { get; set; }
        public int purchaseId { get; set; }

        public int productId { get; set; }
        public int orderQuantity { get; set; }
    }
}
