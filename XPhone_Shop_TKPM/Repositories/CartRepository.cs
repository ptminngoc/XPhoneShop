using Microsoft.Data.SqlClient;
using XPhone_Shop_TKPM.Models;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPhone_Shop_TKPM.Models;

namespace XPhone_Shop_TKPM.Repositories
{
    class CartRepository
    {
        public CustomerModel getCurrentCustomer()
        {
            CustomerModel cus = new CustomerModel();
            if (Global.Connection != null)
            {
                // query to get user's role
                string sql = string.Format("SELECT c.*\r\nFROM Customer c join AccountUser a on c.Tel = a.Tel\r\nWHERE a.Username = '{0}'", Global.usernameCurrent);

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    cus.name = (string?)reader["Customer_Name"];
                    cus.phone = (string?)reader["Tel"];
                    if (reader["Address"] != DBNull.Value)
                    {
                        cus.address = (string?)reader["Address"];
                    }
                    cus.email = (string?)reader["Email"];

                }

                reader.Close();
            }
            return cus;
        }

        public Boolean isCartEmpty()
        {
            CustomerModel currentCustomer = this.getCurrentCustomer();

            if (Global.Connection != null)
            {
                string sql = string.Format("SELECT *\r\nFROM Purchase pur join PurchasesStatusEnum purstatus on pur.Status = purstatus.Value\r\nWHERE purstatus.[Key] = 'Cart' AND pur.Customer_Phone = '{0}'", currentCustomer.phone);
                var command = new SqlCommand(sql, Global.Connection);
                var reader = command.ExecuteReader();
                if (reader.HasRows) {
                    reader.Close();
                    return false;
                }
                reader.Close();
            }
            return true;
        }

        public int getValueByOrderStatus(string key)
        {
            if (Global.Connection != null)
            {
                try {
                    string sql = String.Format("SELECT Value FROM PurchasesStatusEnum WHERE [Key] = '{0}'", key);
                    var command = new SqlCommand(sql, Global.Connection);
                    var reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        try
                        {
                            var temp = (int)reader["Value"];
                            reader.Close();
                            return temp;
                        }
                        catch
                        {
                            reader.Close();
                            return 0;
                        }

                    }
                    reader.Close();
                } catch
                {
                    return 0;
                }
            }
            return 0;
        }

        private int getProductQuantity(int? productId)
        {
            if (productId == null)
                return -1;

            int stockQuantity = 0;
            var sql = $"select Quantity from Product where Product_ID = @id"; // query to get current quantity in stock
            var command = new SqlCommand(sql, Global.Connection);

            command.Parameters.AddWithValue("@id", productId);

            // get quantity from queried
            var reader = command.ExecuteReader();

            reader.Read();
            stockQuantity = (int)reader["Quantity"];

            reader.Close();

            return stockQuantity;
        }

        private void updateStockProductQuantity(int? productId, int newQuantity)
        {
            if (productId == null)
                return;
            if (Global.Connection == null)
            {
                Global.Connection = new SqlConnection(Global.ConnectionString);
            }

            // current in stock suitable for input quantity
            // update stock quantity
            var sqlUpdateStockQuantity = "update Product set Quantity = @stockQuantity where Product_ID = @pId";
            var command = new SqlCommand(sqlUpdateStockQuantity, Global.Connection);

            command.Parameters.AddWithValue("@stockQuantity", newQuantity);
            command.Parameters.AddWithValue("@pId", productId);

            command.ExecuteNonQuery();
        }

        public int isExitsProductInCart(int id_cart, ProductModel p)
        {
            if(Global.Connection != null)
            {
                string sql = string.Format("SELECT Quantity\r\nFROM PurchaseDetail\r\nWHERE Product_ID = {0} AND Purchase_ID = {1}", p.ProductID, id_cart);
                var command = new SqlCommand(sql, Global.Connection);
                var reader = command.ExecuteReader();
                var temp = 0;
                if (reader.Read())
                {

                    temp = (int)reader["Quantity"];
                }
                reader.Close();
                return temp;
            }
            return 0;
        }

        public Double getTotalCart(int id_cart)
        {
            if (Global.Connection != null)
            {
                string sql = string.Format("SELECT Total\r\nFROM Purchase\r\nWHERE Purchase_ID = {0}", id_cart);
                var command = new SqlCommand(sql, Global.Connection);
                var reader = command.ExecuteReader();
                var temp = 0.0;
                if (reader.Read())
                {
                    temp = (Double)reader["Total"];
                }
                reader.Close();
                return temp;
            }
            return 0;
        }

        public int getCartID()
        {
            var curCus = getCurrentCustomer();
            var sql = string.Format("SELECT Purchase_ID\r\nFROM Purchase\r\nWHERE Customer_Phone = '{0}' AND Status = {1}", curCus.phone, getValueByOrderStatus("Cart"));
            var command = new SqlCommand(sql, Global.Connection);
            var reader = command.ExecuteReader();
            var id_cart = 0;
            if (reader.Read())
            {
                id_cart = (int)reader["Purchase_ID"];
            }
            reader.Close();
            return id_cart;
        }

        public void createCart()
        {
            CustomerModel currentCustomer = this.getCurrentCustomer();
            DateTime dt = DateTime.Now;

            string sql_ = "INSERT INTO Purchase VALUES (DEFAULT, @ProductPrice, @Date, @Status, @Phone)";

            // Tạo đối tượng command và truyền tham số vào
            var command_ = new SqlCommand(sql_, Global.Connection);
            command_.Parameters.AddWithValue("@ProductPrice", 0);
            command_.Parameters.AddWithValue("@Date", dt.ToString("yyyy-MM-d"));
            command_.Parameters.AddWithValue("@Status", getValueByOrderStatus("Cart"));
            command_.Parameters.AddWithValue("@Phone", currentCustomer.phone);
            // Thực thi câu lệnh SQL
            command_.ExecuteNonQuery();
        }

        public Boolean addProductToNewOrder(ProductModel p, int quantity)
        {
            if (getProductQuantity(p.ProductID) < quantity || getProductQuantity(p.ProductID) == -1)
            {
                return false;
            }
            else
            {
                updateStockProductQuantity(p.ProductID, getProductQuantity(p.ProductID) - quantity);
            }
            try
            {
                CustomerModel currentCustomer = this.getCurrentCustomer();
                DateTime dt = DateTime.Now;

                // Chen order moi vao
                // Tạo câu lệnh SQL với tham số
                string sql_ = "INSERT INTO Purchase VALUES (DEFAULT, @ProductPrice, @Date, @Status, @Phone)";

                // Tạo đối tượng command và truyền tham số vào
                var command_ = new SqlCommand(sql_, Global.Connection);
                command_.Parameters.AddWithValue("@ProductPrice", p.ProductPrice);
                command_.Parameters.AddWithValue("@Date", dt.ToString("yyyy-MM-d"));
                command_.Parameters.AddWithValue("@Status", getValueByOrderStatus("New"));
                command_.Parameters.AddWithValue("@Phone", currentCustomer.phone);

                // Thực thi câu lệnh SQL
                command_.ExecuteNonQuery();

                // Lay id order
                var sql = string.Format("SELECT MAX(Purchase_ID) AS Purchase_ID\r\nFROM Purchase\r\nWHERE Customer_Phone = '{0}' AND Status = {1}", currentCustomer.phone, getValueByOrderStatus("New"));
                var command = new SqlCommand(sql, Global.Connection);
                var reader = command.ExecuteReader();
                var id_order = 0;
                if (reader.Read())
                {
                    id_order = (int)reader["Purchase_ID"];
                }
                reader.Close();

                // Tạo câu lệnh SQL với tham số
                string sqlDetail = "INSERT INTO PurchaseDetail VALUES (@PurchaseID, @ProductID, @Quantity)";
                string sqlPurchase = "UPDATE Purchase SET Total = @Total WHERE Purchase_ID = @PurchaseID";

                // Tạo đối tượng command và truyền tham số vào
                var commandDetail = new SqlCommand(sqlDetail, Global.Connection);
                commandDetail.Parameters.AddWithValue("@PurchaseID", id_order);
                commandDetail.Parameters.AddWithValue("@ProductID", p.ProductID);
                commandDetail.Parameters.AddWithValue("@Quantity", quantity);

                var commandPurchase = new SqlCommand(sqlPurchase, Global.Connection);
                commandPurchase.Parameters.AddWithValue("@Total", p.ProductPrice * quantity);
                commandPurchase.Parameters.AddWithValue("@PurchaseID", id_order);

                // Thực thi câu lệnh SQL
                commandDetail.ExecuteNonQuery();
                commandPurchase.ExecuteNonQuery(); 
            } catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public Boolean addProductToCart(ProductModel p, int quantity)
        {
            if (getProductQuantity(p.ProductID) < quantity || getProductQuantity(p.ProductID) == -1)
            {
                return false;
            } else
            {
                updateStockProductQuantity(p.ProductID, getProductQuantity(p.ProductID) - quantity);
            }

            CustomerModel currentCustomer = this.getCurrentCustomer();
            DateTime dt = DateTime.Now;
            if (Global.Connection != null)
            {
                if (isCartEmpty())
                {

                    try
                    {
                        // Chen cart moi vao
                        // Tạo câu lệnh SQL với tham số
                        string sql_ = "INSERT INTO Purchase VALUES (DEFAULT, @ProductPrice, @Date, @Status, @Phone)";

                        // Tạo đối tượng command và truyền tham số vào
                        var command_ = new SqlCommand(sql_, Global.Connection);
                        command_.Parameters.AddWithValue("@ProductPrice", p.ProductPrice);
                        command_.Parameters.AddWithValue("@Date", dt.ToString("yyyy-MM-d"));
                        command_.Parameters.AddWithValue("@Status", getValueByOrderStatus("Cart"));
                        command_.Parameters.AddWithValue("@Phone", currentCustomer.phone);

                        // Thực thi câu lệnh SQL
                        command_.ExecuteNonQuery();

                    }
                    catch
                    {
                        return false;
                    }
                }

                // Lay id cart
                var sql = string.Format("SELECT Purchase_ID\r\nFROM Purchase\r\nWHERE Customer_Phone = '{0}' AND Status = {1}", currentCustomer.phone, getValueByOrderStatus("Cart"));
                var command = new SqlCommand(sql, Global.Connection);
                var reader = command.ExecuteReader();
                var id_cart = 0;
                if (reader.Read())
                {
                    id_cart = (int)reader["Purchase_ID"];
                }
                reader.Close();

                // chen tung san pham vao cart
                if (isExitsProductInCart(id_cart, p) > 0)
                {
                    // Tạo câu lệnh SQL với tham số
                    string sqlDetail = "UPDATE PurchaseDetail SET Quantity = @Quantity WHERE Purchase_ID = @PurchaseID AND Product_ID = @ProductID";
                        string sqlPurchase = "UPDATE Purchase SET Total = @Total WHERE Purchase_ID = @PurchaseID";

                        // Tạo đối tượng command và truyền tham số vào
                        var commandDetail = new SqlCommand(sqlDetail, Global.Connection);
                        commandDetail.Parameters.AddWithValue("@Quantity", isExitsProductInCart(id_cart, p) + quantity);
                        commandDetail.Parameters.AddWithValue("@PurchaseID", id_cart);
                        commandDetail.Parameters.AddWithValue("@ProductID", p.ProductID);

                        var commandPurchase = new SqlCommand(sqlPurchase, Global.Connection);
                        commandPurchase.Parameters.AddWithValue("@Total", getTotalCart(id_cart) + p.ProductPrice * quantity);
                        commandPurchase.Parameters.AddWithValue("@PurchaseID", id_cart);

                        // Thực thi câu lệnh SQL
                        commandDetail.ExecuteNonQuery();
                        commandPurchase.ExecuteNonQuery();

                }
                else
                {
                    // Tạo câu lệnh SQL với tham số
                    string sqlDetail = "INSERT INTO PurchaseDetail VALUES (@PurchaseID, @ProductID, @Quantity)";
                    string sqlPurchase = "UPDATE Purchase SET Total = @Total WHERE Purchase_ID = @PurchaseID";

                    // Tạo đối tượng command và truyền tham số vào
                    var commandDetail = new SqlCommand(sqlDetail, Global.Connection);
                    commandDetail.Parameters.AddWithValue("@PurchaseID", id_cart);
                    commandDetail.Parameters.AddWithValue("@ProductID", p.ProductID);
                    commandDetail.Parameters.AddWithValue("@Quantity", quantity);

                    var commandPurchase = new SqlCommand(sqlPurchase, Global.Connection);
                    commandPurchase.Parameters.AddWithValue("@Total", getTotalCart(id_cart) + p.ProductPrice * quantity);
                    commandPurchase.Parameters.AddWithValue("@PurchaseID", id_cart);

                    // Thực thi câu lệnh SQL
                    commandDetail.ExecuteNonQuery();
                    commandPurchase.ExecuteNonQuery();
                }
                return true;
            }
            return false;
        }
    }
}
