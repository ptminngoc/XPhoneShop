using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPhone_Shop_TKPM.Models;

namespace XPhone_Shop_TKPM.Repositories
{
    class OrderRepository
    {
        // query and get all orders item in DB
        public ObservableCollection<OrderModel> getAllOrder()
        {
            ObservableCollection<OrderModel> result = new ObservableCollection<OrderModel>();
            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();
            if (Global.Connection != null)
            {
                List<string> statusTypeList = getStatusDisplayTextStringFromDB();

                // query to get user's role
                string sql = $"select * from Purchase";

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int? pId;

                    // check null promotion code
                    if (reader["Promotion_ID"] == DBNull.Value)
                        pId = null;
                    else
                        pId = (int)reader["Promotion_ID"];

                    var dtime = (DateTime)reader["Centered_At"];
                    var month = dtime.Month;
                    var day = dtime.Day;
                    var year = dtime.Year;
                    //string.Format("{0}/{1}/{2}", month, day, year);

                    // add orders from DB to collection
                    result.Add(new OrderModel()
                    {
                        PromotionID = pId,
                        OrderID = (int)reader["Purchase_ID"],
                        OrderDate = dtime,
                        OrderStatus = (int)reader["Status"],
                        OrderTotal = (Double)reader["Total"],
                        CustomerPhone = (string)reader["Customer_Phone"],
                        OrderStatusDisplayText = statusTypeList.ElementAt((int)reader["Status"] - 1).ToString()
                    });
                }

                reader.Close();
            }


            return result;
        }

        public bool addOrder(OrderModel order)
        {
            bool result = false;
            int lastId = 0;

            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();

            if (Global.Connection != null)
            {
                // Retrieve the ID of the last Category record
                var getLastIdSql = "SELECT TOP 1 Purchase_ID FROM Purchase ORDER BY Purchase_ID DESC";
                var getLastIdCommand = new SqlCommand(getLastIdSql, Global.Connection);
                var lastIdReader = getLastIdCommand.ExecuteReader();

                if (lastIdReader.Read())
                {
                    lastId = lastIdReader.GetInt32(0);
                }

                lastIdReader.Close();

                // Turn on IDENTITY_INSERT for the Category table
                var setIdInsertSql = "SET IDENTITY_INSERT Purchase ON";
                var setIdInsertCommand = new SqlCommand(setIdInsertSql, Global.Connection);
                setIdInsertCommand.ExecuteNonQuery();

                // Insert the new Category record with a new ID
                var sql = "INSERT INTO Purchase(Purchase_ID, Promotion_ID, Total, Centered_At, Status, Customer_Phone) VALUES(@id, @promotion, @total, @date, @status, @phone)";

                var command = new SqlCommand(sql, Global.Connection);

                command.Parameters.AddWithValue("@id", lastId + 1);
                if(order.PromotionID == 0)
                {
                    command.Parameters.AddWithValue("@promotion", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@promotion", order.PromotionID);
                }
                command.Parameters.AddWithValue("@total", order.OrderTotal);
                command.Parameters.AddWithValue("@date", order.OrderDate);
                command.Parameters.AddWithValue("@status", order.OrderStatus);
                command.Parameters.AddWithValue("@phone", order.CustomerPhone);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }

                // Turn off IDENTITY_INSERT for the Category table
                var unsetIdInsertSql = "SET IDENTITY_INSERT Purchase OFF";
                var unsetIdInsertCommand = new SqlCommand(unsetIdInsertSql, Global.Connection);
                unsetIdInsertCommand.ExecuteNonQuery();
            }

            //Global.Connection?.Close();
            return result;

        }
        public bool editOrderWithPhone(string? phone, string? oldphone)
        {
            bool result = false;

            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();

            if (Global.Connection != null)
            {
                var sql = "UPDATE Purchase SET Customer_Phone = @newPhone WHERE Customer_Phone = @oldPhone";

                var command = new SqlCommand(sql, Global.Connection);

                command.Parameters.AddWithValue("@newPhone", phone);
                command.Parameters.AddWithValue("@oldPhone", oldphone);


                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }

            //Global.Connection?.Close();
            return result;
        }

        public List<int> GetOrderListWithPhone(string? phone)
        {
            List<int> result = new List<int>();
            if (Global.Connection != null)
            {
                string sql = $"select * from Purchase where Customer_Phone = @cPhone";

                var command = new SqlCommand(sql, Global.Connection);
                command.Parameters.AddWithValue("@cPhone", phone);
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int pId = (int)reader["Purchase_ID"];

                    result.Add(pId);
                }
                reader.Close();
            }
            return result;
        }

        public Boolean deleteOrderPhone(string? phone)
        {
            var result = false;
            List<int> orderList = GetOrderListWithPhone(phone);

            foreach (var order in orderList)
            {
                result = deleteOrderId(order);
            }
            return result;
        }

        public Boolean deleteOrderId(int id)
        {
            if (Global.Connection != null)
            {
                // delete order details first to avoid FK conflict
                deleteOrderDetails(id);

                // query to get user's role
                string sql = $"delete from Purchase where Purchase_ID = @ID";

                var command = new SqlCommand(sql, Global.Connection);
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();

                return true;
            }
            return false;
        }

        private void deleteOrderDetails(int id)
        {
            string sql = $"delete from PurchaseDetail where Purchase_ID = @ID";

            var command = new SqlCommand(sql, Global.Connection);
            command.Parameters.AddWithValue("@ID", id);
            command.ExecuteNonQuery();
        }

        // query and get status display text from SQL
        private List<string> getStatusDisplayTextStringFromDB()
        {
            string sql = $"select Display_Text from PurchasesStatusEnum";
            List<string> result = new List<string>();
            var command = new SqlCommand(sql, Global.Connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add((string)reader["Display_Text"]);
            }

            reader.Close();


            return result;
        }
    }
}
