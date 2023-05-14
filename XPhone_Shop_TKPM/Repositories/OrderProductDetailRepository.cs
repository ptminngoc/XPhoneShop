using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPhone_Shop_TKPM.Models;

namespace XPhone_Shop_TKPM.Repositories
{
    public class OrderProductDetailRepository
    {
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

        public ObservableCollection<OrderProductDetailModel> getMoiTao()
        {
            ObservableCollection<OrderProductDetailModel> result = new ObservableCollection<OrderProductDetailModel>();

            if (Global.Connection != null)
            {
                List<string> statusTypeList = getStatusDisplayTextStringFromDB();

                //query de lay so dien thoai hien tai
                string sql = $"select Tel from AccountUser where Username = '{Global.usernameCurrent}'";
                Debug.WriteLine(Global.usernameCurrent);
                var command = new SqlCommand(sql, Global.Connection);
                var reader = command.ExecuteReader();
                reader.Read();
                var tel = (string)reader["Tel"];
                reader.Close();

                //Lay list product san pham chi tiet
                var sql_2 = $"select p.*, pur.Purchase_ID, pd.Quantity as QuantityProduct \r\nfrom Purchase pur join PurchaseDetail pd on pur.Purchase_ID = pd.Purchase_ID\r\n\t\tjoin Product p on pd.Product_ID = p.Product_ID";
                var command_2 = new SqlCommand(sql_2, Global.Connection);
                var reader_2 = command_2.ExecuteReader();

                List<int> purchaseIDList = new List<int>();
                ObservableCollection<ProductModel> productList = new ObservableCollection<ProductModel>();
                while (reader_2.Read())
                {

                    int? pId = (int)reader_2["Product_ID"];
                    int? cId = (int)reader_2["Category_ID"];
                    string pName = (string)reader_2["Product_Name"];
                    string pAvatar = (string)reader_2["Avatar"];
                    int pQuantiry = (int)reader_2["QuantityProduct"];
                    double pPrice = (double)reader_2["Price"];
                    double pPriceOriginal = (double)reader_2["Price_Original"];


                    // add products from DB to collection
                    productList.Add(new ProductModel()
                    {
                        ProductID = pId,
                        CategoryID = cId,
                        ProductName = pName,
                        ProductAvatar = pAvatar,
                        ProductQuantity = pQuantiry,
                        ProductPrice = pPrice,
                        ProductPriceOriginal = pPriceOriginal,
                    });

                    purchaseIDList.Add((int)reader_2["Purchase_ID"]);
                }
                reader_2.Close();

                // query de lay don hang
                sql = $"select * from Purchase where Customer_Phone = '{tel}'";
                command = new SqlCommand(sql, Global.Connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int? promotionId;
                    if (reader["Promotion_ID"] == DBNull.Value)
                        promotionId = null;
                    else
                        promotionId = (int)reader["Promotion_ID"];

                    int? orderId = (int)reader["Purchase_ID"];
                    Double? orderTotal = (Double)reader["Total"];
                    var dtime = (DateTime)reader["Centered_At"];
                    var orderStatus = (int)reader["Status"];
                    var isShipping = "";
                    var isNew = "Visible";

                    if (orderStatus != 5 && orderStatus == 1)
                    {

                        //Lay list product san pham chi tiet
                        ObservableCollection<ProductModel> productListOfPur = new ObservableCollection<ProductModel>();
                        for (int i = 0; i < productList.Count; i++)
                        {
                            if (purchaseIDList[i] == orderId)
                            {
                                productListOfPur.Add(productList[i]);
                            }
                        }

                        //theem ket qua vo result
                        result.Add(new OrderProductDetailModel()
                        {
                            PromotionID = promotionId,
                            OrderID = (int)orderId,
                            OrderDate = dtime,
                            OrderStatus = orderStatus,
                            OrderTotal = (Double)orderTotal,
                            CustomerPhone = (string)reader["Customer_Phone"],
                            OrderStatusDisplayText = statusTypeList.ElementAt((int)reader["Status"] - 1).ToString(),
                            ProductDetail = productListOfPur,
                            IsShipping = isShipping,
                            IsNew = isNew
                        });
                    }
                }

                reader.Close();
            }


            return result;
        }

        public ObservableCollection<OrderProductDetailModel> getAll()
        {
            ObservableCollection<OrderProductDetailModel> result = new ObservableCollection<OrderProductDetailModel>();

            if (Global.Connection != null)
            {
                List<string> statusTypeList = getStatusDisplayTextStringFromDB();

                //query de lay so dien thoai hien tai
                string sql = $"select Tel from AccountUser where Username = '{Global.usernameCurrent}'";
                Debug.WriteLine(Global.usernameCurrent);
                var command = new SqlCommand(sql, Global.Connection);
                var reader = command.ExecuteReader();
                reader.Read();
                var tel = (string)reader["Tel"];
                reader.Close();

                //Lay list product san pham chi tiet
                var sql_2 = $"select p.*, pur.Purchase_ID, pd.Quantity as QuantityProduct \r\nfrom Purchase pur join PurchaseDetail pd on pur.Purchase_ID = pd.Purchase_ID\r\n\t\tjoin Product p on pd.Product_ID = p.Product_ID";
                var command_2 = new SqlCommand(sql_2, Global.Connection);
                var reader_2 = command_2.ExecuteReader();

                List<int> purchaseIDList = new List<int>();
                ObservableCollection<ProductModel> productList = new ObservableCollection<ProductModel>();
                while (reader_2.Read())
                {

                    int? pId = (int)reader_2["Product_ID"];
                    int? cId = (int)reader_2["Category_ID"];
                    string pName = (string)reader_2["Product_Name"];
                    string pAvatar = (string)reader_2["Avatar"];
                    int pQuantiry = (int)reader_2["QuantityProduct"];
                    double pPrice = (double)reader_2["Price"];
                    double pPriceOriginal = (double)reader_2["Price_Original"];


                    // add products from DB to collection
                    productList.Add(new ProductModel()
                    {
                        ProductID = pId,
                        CategoryID = cId,
                        ProductName = pName,
                        ProductAvatar = pAvatar,
                        ProductQuantity = pQuantiry,
                        ProductPrice = pPrice,
                        ProductPriceOriginal = pPriceOriginal,
                    });

                    purchaseIDList.Add((int)reader_2["Purchase_ID"]);
                }
                reader_2.Close();

                // query de lay don hang
                sql = $"select * from Purchase where Customer_Phone = '{tel}'";
                command = new SqlCommand(sql, Global.Connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int? promotionId;
                    if (reader["Promotion_ID"] == DBNull.Value)
                        promotionId = null;
                    else
                        promotionId = (int)reader["Promotion_ID"];

                    int? orderId = (int)reader["Purchase_ID"];
                    Double? orderTotal = (Double)reader["Total"];
                    var dtime = (DateTime)reader["Centered_At"];
                    var orderStatus = (int)reader["Status"];
                    var isShipping = "";
                    var isNew = "Collapsed";

                    if (orderStatus != 5)
                    {
                        if (orderStatus == 4)
                        {
                            isShipping = "Giao hàng thành công |";
                        }
                        if (orderStatus == 1)
                        {
                            isNew = "Visible";
                        }

                        //Lay list product san pham chi tiet
                        ObservableCollection<ProductModel> productListOfPur = new ObservableCollection<ProductModel>();
                        for (int i = 0; i < productList.Count; i++)
                        {
                            if (purchaseIDList[i] == orderId)
                            {
                                productListOfPur.Add(productList[i]);
                                Debug.WriteLine("cos nha");
                            }
                        }

                        //theem ket qua vo result
                        result.Add(new OrderProductDetailModel()
                        {
                            PromotionID = promotionId,
                            OrderID = (int)orderId,
                            OrderDate = dtime,
                            OrderStatus = orderStatus,
                            OrderTotal = (Double)orderTotal,
                            CustomerPhone = (string)reader["Customer_Phone"],
                            OrderStatusDisplayText = statusTypeList.ElementAt((int)reader["Status"] - 1).ToString(),
                            ProductDetail = productListOfPur,
                            IsShipping = isShipping,
                            IsNew = isNew
                        });
                    }
                }

                reader.Close();
            }


            return result;
        }

        public ObservableCollection<OrderProductDetailModel> getDangGiao()
        {
            ObservableCollection<OrderProductDetailModel> result = new ObservableCollection<OrderProductDetailModel>();

            if (Global.Connection != null)
            {
                List<string> statusTypeList = getStatusDisplayTextStringFromDB();

                //query de lay so dien thoai hien tai
                string sql = $"select Tel from AccountUser where Username = '{Global.usernameCurrent}'";
                Debug.WriteLine(Global.usernameCurrent);
                var command = new SqlCommand(sql, Global.Connection);
                var reader = command.ExecuteReader();
                reader.Read();
                var tel = (string)reader["Tel"];
                reader.Close();

                //Lay list product san pham chi tiet
                var sql_2 = $"select p.*, pur.Purchase_ID, pd.Quantity as QuantityProduct \r\nfrom Purchase pur join PurchaseDetail pd on pur.Purchase_ID = pd.Purchase_ID\r\n\t\tjoin Product p on pd.Product_ID = p.Product_ID";
                var command_2 = new SqlCommand(sql_2, Global.Connection);
                var reader_2 = command_2.ExecuteReader();

                List<int> purchaseIDList = new List<int>();
                ObservableCollection<ProductModel> productList = new ObservableCollection<ProductModel>();
                while (reader_2.Read())
                {

                    int? pId = (int)reader_2["Product_ID"];
                    int? cId = (int)reader_2["Category_ID"];
                    string pName = (string)reader_2["Product_Name"];
                    string pAvatar = (string)reader_2["Avatar"];
                    int pQuantiry = (int)reader_2["QuantityProduct"];
                    double pPrice = (double)reader_2["Price"];
                    double pPriceOriginal = (double)reader_2["Price_Original"];


                    // add products from DB to collection
                    productList.Add(new ProductModel()
                    {
                        ProductID = pId,
                        CategoryID = cId,
                        ProductName = pName,
                        ProductAvatar = pAvatar,
                        ProductQuantity = pQuantiry,
                        ProductPrice = pPrice,
                        ProductPriceOriginal = pPriceOriginal,
                    });

                    purchaseIDList.Add((int)reader_2["Purchase_ID"]);
                }
                reader_2.Close();

                // query de lay don hang
                sql = $"select * from Purchase where Customer_Phone = '{tel}'";
                command = new SqlCommand(sql, Global.Connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int? promotionId;
                    if (reader["Promotion_ID"] == DBNull.Value)
                        promotionId = null;
                    else
                        promotionId = (int)reader["Promotion_ID"];

                    int? orderId = (int)reader["Purchase_ID"];
                    Double? orderTotal = (Double)reader["Total"];
                    var dtime = (DateTime)reader["Centered_At"];
                    var orderStatus = (int)reader["Status"];
                    var isShipping = "";
                    var isNew = "Collapsed";

                    if (orderStatus != 5 && orderStatus == 2)
                    {
                        //Lay list product san pham chi tiet
                        ObservableCollection<ProductModel> productListOfPur = new ObservableCollection<ProductModel>();
                        for (int i = 0; i < productList.Count; i++)
                        {
                            if (purchaseIDList[i] == orderId)
                            {
                                productListOfPur.Add(productList[i]);
                            }
                        }

                        //theem ket qua vo result
                        result.Add(new OrderProductDetailModel()
                        {
                            PromotionID = promotionId,
                            OrderID = (int)orderId,
                            OrderDate = dtime,
                            OrderStatus = orderStatus,
                            OrderTotal = (Double)orderTotal,
                            CustomerPhone = (string)reader["Customer_Phone"],
                            OrderStatusDisplayText = statusTypeList.ElementAt((int)reader["Status"] - 1).ToString(),
                            ProductDetail = productListOfPur,
                            IsShipping = isShipping,
                            IsNew = isNew
                        });
                    }
                }

                reader.Close();
            }


            return result;
        }

        public ObservableCollection<OrderProductDetailModel> getHoanThanh()
        {
            ObservableCollection<OrderProductDetailModel> result = new ObservableCollection<OrderProductDetailModel>();

            if (Global.Connection != null)
            {
                List<string> statusTypeList = getStatusDisplayTextStringFromDB();

                //query de lay so dien thoai hien tai
                string sql = $"select Tel from AccountUser where Username = '{Global.usernameCurrent}'";
                Debug.WriteLine(Global.usernameCurrent);
                var command = new SqlCommand(sql, Global.Connection);
                var reader = command.ExecuteReader();
                reader.Read();
                var tel = (string)reader["Tel"];
                reader.Close();

                //Lay list product san pham chi tiet
                var sql_2 = $"select p.*, pur.Purchase_ID, pd.Quantity as QuantityProduct \r\nfrom Purchase pur join PurchaseDetail pd on pur.Purchase_ID = pd.Purchase_ID\r\n\t\tjoin Product p on pd.Product_ID = p.Product_ID";
                var command_2 = new SqlCommand(sql_2, Global.Connection);
                var reader_2 = command_2.ExecuteReader();

                List<int> purchaseIDList = new List<int>();
                ObservableCollection<ProductModel> productList = new ObservableCollection<ProductModel>();
                while (reader_2.Read())
                {

                    int? pId = (int)reader_2["Product_ID"];
                    int? cId = (int)reader_2["Category_ID"];
                    string pName = (string)reader_2["Product_Name"];
                    string pAvatar = (string)reader_2["Avatar"];
                    int pQuantiry = (int)reader_2["QuantityProduct"];
                    double pPrice = (double)reader_2["Price"];
                    double pPriceOriginal = (double)reader_2["Price_Original"];


                    // add products from DB to collection
                    productList.Add(new ProductModel()
                    {
                        ProductID = pId,
                        CategoryID = cId,
                        ProductName = pName,
                        ProductAvatar = pAvatar,
                        ProductQuantity = pQuantiry,
                        ProductPrice = pPrice,
                        ProductPriceOriginal = pPriceOriginal,
                    });

                    purchaseIDList.Add((int)reader_2["Purchase_ID"]);
                }
                reader_2.Close();

                // query de lay don hang
                sql = $"select * from Purchase where Customer_Phone = '{tel}'";
                command = new SqlCommand(sql, Global.Connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int? promotionId;
                    if (reader["Promotion_ID"] == DBNull.Value)
                        promotionId = null;
                    else
                        promotionId = (int)reader["Promotion_ID"];

                    int? orderId = (int)reader["Purchase_ID"];
                    Double? orderTotal = (Double)reader["Total"];
                    var dtime = (DateTime)reader["Centered_At"];
                    var orderStatus = (int)reader["Status"];
                    var isShipping = "Giao hàng thành công |";
                    var isNew = "Collapsed";

                    if (orderStatus != 5 && orderStatus == 4)
                    {
                        //Lay list product san pham chi tiet
                        ObservableCollection<ProductModel> productListOfPur = new ObservableCollection<ProductModel>();
                        for (int i = 0; i < productList.Count; i++)
                        {
                            if (purchaseIDList[i] == orderId)
                            {
                                productListOfPur.Add(productList[i]);
                            }
                        }

                        //theem ket qua vo result
                        result.Add(new OrderProductDetailModel()
                        {
                            PromotionID = promotionId,
                            OrderID = (int)orderId,
                            OrderDate = dtime,
                            OrderStatus = orderStatus,
                            OrderTotal = (Double)orderTotal,
                            CustomerPhone = (string)reader["Customer_Phone"],
                            OrderStatusDisplayText = statusTypeList.ElementAt((int)reader["Status"] - 1).ToString(),
                            ProductDetail = productListOfPur,
                            IsShipping = isShipping,
                            IsNew = isNew
                        });
                    }
                }

                reader.Close();
            }


            return result;
        }

        public ObservableCollection<OrderProductDetailModel> getDaHuy()
        {
            ObservableCollection<OrderProductDetailModel> result = new ObservableCollection<OrderProductDetailModel>();

            if (Global.Connection != null)
            {
                List<string> statusTypeList = getStatusDisplayTextStringFromDB();

                //query de lay so dien thoai hien tai
                string sql = $"select Tel from AccountUser where Username = '{Global.usernameCurrent}'";
                Debug.WriteLine(Global.usernameCurrent);
                var command = new SqlCommand(sql, Global.Connection);
                var reader = command.ExecuteReader();
                reader.Read();
                var tel = (string)reader["Tel"];
                reader.Close();

                //Lay list product san pham chi tiet
                var sql_2 = $"select p.*, pur.Purchase_ID, pd.Quantity as QuantityProduct \r\nfrom Purchase pur join PurchaseDetail pd on pur.Purchase_ID = pd.Purchase_ID\r\n\t\tjoin Product p on pd.Product_ID = p.Product_ID";
                var command_2 = new SqlCommand(sql_2, Global.Connection);
                var reader_2 = command_2.ExecuteReader();

                List<int> purchaseIDList = new List<int>();
                ObservableCollection<ProductModel> productList = new ObservableCollection<ProductModel>();
                while (reader_2.Read())
                {

                    int? pId = (int)reader_2["Product_ID"];
                    int? cId = (int)reader_2["Category_ID"];
                    string pName = (string)reader_2["Product_Name"];
                    string pAvatar = (string)reader_2["Avatar"];
                    int pQuantiry = (int)reader_2["QuantityProduct"];
                    double pPrice = (double)reader_2["Price"];
                    double pPriceOriginal = (double)reader_2["Price_Original"];


                    // add products from DB to collection
                    productList.Add(new ProductModel()
                    {
                        ProductID = pId,
                        CategoryID = cId,
                        ProductName = pName,
                        ProductAvatar = pAvatar,
                        ProductQuantity = pQuantiry,
                        ProductPrice = pPrice,
                        ProductPriceOriginal = pPriceOriginal,
                    });

                    purchaseIDList.Add((int)reader_2["Purchase_ID"]);
                }
                reader_2.Close();

                // query de lay don hang
                sql = $"select * from Purchase where Customer_Phone = '{tel}'";
                command = new SqlCommand(sql, Global.Connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int? promotionId;
                    if (reader["Promotion_ID"] == DBNull.Value)
                        promotionId = null;
                    else
                        promotionId = (int)reader["Promotion_ID"];

                    int? orderId = (int)reader["Purchase_ID"];
                    Double? orderTotal = (Double)reader["Total"];
                    var dtime = (DateTime)reader["Centered_At"];
                    var orderStatus = (int)reader["Status"];
                    var isShipping = "";
                    var isNew = "Collapsed";

                    if (orderStatus != 5 && orderStatus == 3)
                    {
                        //Lay list product san pham chi tiet
                        ObservableCollection<ProductModel> productListOfPur = new ObservableCollection<ProductModel>();
                        for (int i = 0; i < productList.Count; i++)
                        {
                            if (purchaseIDList[i] == orderId)
                            {
                                productListOfPur.Add(productList[i]);
                            }
                        }

                        //theem ket qua vo result
                        result.Add(new OrderProductDetailModel()
                        {
                            PromotionID = promotionId,
                            OrderID = (int)orderId,
                            OrderDate = dtime,
                            OrderStatus = orderStatus,
                            OrderTotal = (Double)orderTotal,
                            CustomerPhone = (string)reader["Customer_Phone"],
                            OrderStatusDisplayText = statusTypeList.ElementAt((int)reader["Status"] - 1).ToString(),
                            ProductDetail = productListOfPur,
                            IsShipping = isShipping,
                            IsNew = isNew
                        });
                    }
                }

                reader.Close();
            }


            return result;
        }

        public Boolean cancelOrderId(int id)
        {
            if (Global.Connection != null)
            {
                // query to get user's role
                string sql = $"update Purchase set Status = 3 where Purchase_ID = @ID";
                var command = new SqlCommand(sql, Global.Connection);
                command.Parameters.AddWithValue("@ID", id);
                command.ExecuteNonQuery();

                return true;
            }
            return false;
        }
    }
}
