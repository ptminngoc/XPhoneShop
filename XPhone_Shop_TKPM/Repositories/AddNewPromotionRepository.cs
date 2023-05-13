using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPhone_Shop_TKPM.Models;

namespace XPhone_Shop_TKPM.Repositories
{
    class AddNewPromotionRepository
    {
        public void addNewPromo(PromotionModel newPromo)
        {
            var sql = "insert into Promotion (Promotion_Name, Promotion_Percentage) values (@name, @percent)";
            var command = new SqlCommand(sql, Global.Connection);

            command.Parameters.AddWithValue("@name", newPromo._promotionName);
            command.Parameters.AddWithValue("@percent", newPromo._promotionPercentage);

            command.ExecuteNonQuery();
        }
    }
}
