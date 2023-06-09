﻿using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Data.SqlClient;
using XPhone_Shop_TKPM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPhone_Shop_TKPM.Repositories
{
    class CustomerRepository
    {
        public ObservableCollection<CustomerModel> getAllCustomer()
        {
            ObservableCollection<CustomerModel> result = new ObservableCollection<CustomerModel>();
            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = $"select * from Customer";

                var command = new SqlCommand(sql, Global.Connection);

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string? cName = (string)reader["Customer_Name"];
                    string? cPhone = (string)reader["Tel"];
                    string? cAdress = (string)reader["Address"];
                    string? cEmail = (string)reader["Email"];


                    // add products from DB to collection
                    result.Add(new CustomerModel()
                    {
                        name = cName,
                        phone = cPhone,
                        address = cAdress,
                        email = cEmail,
                    });
                }

                reader.Close();
            }

            //Global.Connection?.Close();
            return result;
        }

        //public ObservableCollection<CustomerModel> getAllCustomerWithOrder()
        //{
        //    ObservableCollection<CustomerModel> result = new ObservableCollection<CustomerModel>();
        //    Global.Connection = new SqlConnection(Global.ConnectionString);
        //    Global.Connection.Open();
        //    if (Global.Connection != null)
        //    {

        //        // query to get user's role
        //        string sql = $"select*, (SELECT COUNT(*) FROM Purchase p WHERE c.Tel = p.Customer_Phone) as NumOfOrder from Customer c";

        //        var command = new SqlCommand(sql, Global.Connection);

        //        var reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            string? cName = (string)reader["Customer_Name"];
        //            string? cPhone = (string)reader["Tel"];
        //            string? cAdress = (string)reader["Adress"];
        //            string? cEmail = (string)reader["Email"];

        //            // add products from DB to collection
        //            result.Add(new CustomerModel()
        //            {
        //                name = cName,
        //                phone = cPhone,
        //                address = cAdress,
        //                email = cEmail,
        //            });
        //        }

        //        reader.Close();
        //    }

        //    Global.Connection?.Close();
        //    return result;
        //}

        public CustomerModel getCustomerWithPhone(string? tel)
        {
            CustomerModel result = new CustomerModel();
            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();
            if (Global.Connection != null)
            {

                // query to get user's role
                string sql = $"select *  from Customer where tel = @phone";

                var command = new SqlCommand(sql, Global.Connection);
                command.Parameters.AddWithValue("@phone", tel);
                var reader = command.ExecuteReader();

                reader.Read();

                string? cName = (string)reader["Customer_Name"];
                string? cPhone = (string)reader["Tel"];
                string? cAdress = (string)reader["Address"];
                string? cEmail = (string)reader["Email"];

                // add products from DB to collection
                result = new CustomerModel()
                {
                    name = cName,
                    phone = cPhone,
                    address = cAdress,
                    email = cEmail,
                };

                reader.Close();
            }

            //Global.Connection?.Close();
            return result;
        }

        public bool addCustomer(CustomerModel customer)
        {
            bool result = false;

            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();

            if (Global.Connection != null)
            {
                var sql = "INSERT INTO Customer(Customer_Name, Tel, Address, Email) VALUES(@CustomerName, @CustomerTel, @CustomerAddress, @CustomerEmail)";

                var command = new SqlCommand(sql, Global.Connection);

                command.Parameters.AddWithValue("@CustomerName", customer.name);
                command.Parameters.AddWithValue("@CustomerTel", customer.phone);
                command.Parameters.AddWithValue("@CustomerAddress", customer.address);
                command.Parameters.AddWithValue("@CustomerEmail", customer.email);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }

            //Global.Connection?.Close();
            return result;
        }


        public bool editCustomer(CustomerModel customer, string oldCustomer)
        {
            bool result = false;
            
            if (Global.Connection != null)
            {
                if(customer.phone.Equals(oldCustomer))
                {
                    var sql = "UPDATE Customer SET  Customer_Name = @name, Email = @email, Address = @address WHERE Tel = @phone";

                    var command = new SqlCommand(sql, Global.Connection);

                    command.Parameters.AddWithValue("@phone", customer.phone);
                    command.Parameters.AddWithValue("@name", customer.name);
                    command.Parameters.AddWithValue("@address", customer.address);
                    command.Parameters.AddWithValue("@email", customer.email);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        result = true;
                    }
                } else
                {
                    OrderRepository _repository = new OrderRepository();
                    bool add = addCustomer(customer);
                    _repository.editOrderWithPhone(customer.phone, oldCustomer);
                    bool remove = removeCustomer(oldCustomer);

                    if (add && remove)
                    {
                        result = true;
                    }
                }  
            }
            return result;
        }


        public bool removeCustomer(string? tel)
        {
            bool result = false;


            //Global.Connection = new SqlConnection(Global.ConnectionString);
            //Global.Connection.Open();
            if (Global.Connection != null)
            {
                // delete related records in PurchaseDetail table

                OrderRepository _orderRepository = new OrderRepository();
                _orderRepository.deleteOrderPhone(tel);

                // delete the product
                var sql = "DELETE FROM Customer WHERE Tel = @CustomerPhone";
                var command = new SqlCommand(sql, Global.Connection);
                command.Parameters.AddWithValue("@CustomerPhone", tel);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
