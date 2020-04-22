using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using System.Web.Security;

namespace FinalProject_FoodPort.Models
{
    public class CustomerDAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Souradeep"].ConnectionString);
        public CustomerModel getCustomer(string Phonenumber)
        {
            con.Open();
            SqlCommand com_cus = new SqlCommand("Select * from Customer where CustomerPhoneNumber=@phone", con);
            com_cus.Parameters.AddWithValue("@phone", Phonenumber);
            SqlDataReader dr = com_cus.ExecuteReader();
            CustomerModel model = new CustomerModel();
            if (dr.Read())
            {
                model.CustomerID = dr.GetInt32(0);
                model.CustomerName = dr.GetString(1);
                model.CustomerEmail = dr.GetString(2);
                model.CustomerPhoneNumber = dr.GetString(3);
                model.CustomerAddress = dr.GetString(4);
                model.CustomerImage = dr.GetString(5);
            }
            if (model.CustomerImage == "")
            {
                model.CustomerImage = "/Images/blankimage.jpg";
            }
            con.Close();
            return model;
        }
        public List<RestaurantModel> getRestaurants(string Search)
        {
            con.Open();
            List<RestaurantModel> list = new List<RestaurantModel>();
            SqlCommand com_restaurant = new SqlCommand("Select * from Restaurant where RestaurantCity=@add or RestaurantLocality=@add or RestaurantState=@add", con);
            com_restaurant.Parameters.AddWithValue("@add", Search);
            SqlDataReader dr = com_restaurant.ExecuteReader();
            while (dr.Read())
            {
                RestaurantModel model = new RestaurantModel();
                model.RestaurantID = dr.GetInt32(0);
                model.RestaurantName = dr.GetString(1);
                model.RestaurantCity = dr.GetString(2);
                model.RestaurantLocality = dr.GetString(3);
                model.RestaurantState = dr.GetString(4);
                model.RestaurantPhoneNumber = dr.GetString(5);
                model.RestaurantEmailID = dr.GetString(6);
                model.OpenTime = dr.GetString(7);
                model.CloseTime = dr.GetString(8);
                model.Status = dr.GetString(9);
                model.Image = dr.GetString(10);
                list.Add(model);
            }
            con.Close();
            return list;
        }
        public bool UpadateProfile(CustomerModel model)
        {

            if (model.CustomerName != "")
            {
                con.Open();
                SqlCommand com_update = new SqlCommand("update Customer set CustomerName=@name where CustomerPhoneNumber=@number", con);
                com_update.Parameters.AddWithValue("@name", model.CustomerName);
                com_update.Parameters.AddWithValue("@number", model.CustomerPhoneNumber);
                com_update.ExecuteNonQuery();
                con.Close();
            }
            if (model.CustomerEmail != "")
            {
                con.Open();
                SqlCommand com_update = new SqlCommand("update Customer set CustomerEmail=@email where CustomerPhoneNumber=@number", con);
                com_update.Parameters.AddWithValue("@email", model.CustomerEmail);
                com_update.Parameters.AddWithValue("@number", model.CustomerPhoneNumber);
                com_update.ExecuteNonQuery();
                con.Close();
            }
            if (model.CustomerAddress != "")
            {
                con.Open();
                SqlCommand com_update = new SqlCommand("update Customer set CustomerAddress=@add where CustomerPhoneNumber=@number", con);
                com_update.Parameters.AddWithValue("@add", model.CustomerAddress);
                com_update.Parameters.AddWithValue("@number", model.CustomerPhoneNumber);
                com_update.ExecuteNonQuery();
                con.Close();
            }
            if (model.CustomerImage != "")
            {
                con.Open();
                SqlCommand com_update = new SqlCommand("update Customer set CustomerImage=@img where CustomerPhoneNumber=@number", con);
                com_update.Parameters.AddWithValue("@img", model.CustomerImage);
                com_update.Parameters.AddWithValue("@number", model.CustomerPhoneNumber);
                com_update.ExecuteNonQuery();
                con.Close();
            }
            return true;
        }
        public int cusid(string Phonenumber)
        {
            con.Open();
            SqlCommand com_id = new SqlCommand("Select CustomerID from Customer where CustomerPhoneNumber=@number", con);
            com_id.Parameters.AddWithValue("@number", Phonenumber);
            int id = Convert.ToInt32(com_id.ExecuteScalar());
            con.Close();
            return id;
        }
        public bool AddtoCart(int resid, int itemid,int cusid)
        {
            con.Open();
            SqlCommand com_verify = new SqlCommand("Select Count(*) from Cart where CustomerID=@cusid and RestaurantID=@resid and itemID=@itemid", con);
            com_verify.Parameters.AddWithValue("@cusid", cusid);
            com_verify.Parameters.AddWithValue("@resid", resid);
            com_verify.Parameters.AddWithValue("@itemid", itemid);
            int count = Convert.ToInt32(com_verify.ExecuteScalar());
            if(count>0)
            {
                con.Close();
                return false;
            }
            else
            {
                SqlCommand com_get = new SqlCommand("Select ItemName,ItemPrice from Menu where ItemID=@itemid and RestaurantID=@resid", con);
                com_get.Parameters.AddWithValue("@resid", resid);
                com_get.Parameters.AddWithValue("@itemid", itemid);
                SqlDataReader dr = com_get.ExecuteReader();
                string itemname="";
                decimal itemprice=0;
                
                if(dr.Read())
                {
                    itemname = dr.GetString(0);
                    itemprice = dr.GetDecimal(1);
                }
                con.Close();
                SqlCommand com_insert = new SqlCommand("insert Cart values(@cusid,@resid,@itemid,@itemname,1,@itemprice)", con);
                com_insert.Parameters.AddWithValue("@cusid", cusid);
                com_insert.Parameters.AddWithValue("@resid", resid);
                com_insert.Parameters.AddWithValue("@itemid", itemid);
                com_insert.Parameters.AddWithValue("@itemname", itemname);
                com_insert.Parameters.AddWithValue("@itemprice", itemprice);
                con.Open();
                com_insert.ExecuteNonQuery();
                con.Close();
                return true;
            }
        }
        public List<CartModel> viewCart(int custid)
        {
            con.Open();
            SqlCommand com_cart = new SqlCommand("Select distinct RestaurantID from Cart where CustomerID=@custid", con);
            com_cart.Parameters.AddWithValue("@custid", custid);
            SqlDataReader dr = com_cart.ExecuteReader();
            List<CartModel> list = new List<CartModel>();
            while(dr.Read())
            {
                CartModel cart = new CartModel();
                cart.RestaurantID = dr.GetInt32(0);
                list.Add(cart);
            }
            con.Close();
            return list;
        }
        public List<CartModel> ViewCartDetails(int resid, int custid)
        {
            con.Open();
            SqlCommand com_cart = new SqlCommand("Select * from Cart where CustomerID=@custid and RestaurantID=@resid", con);
            com_cart.Parameters.AddWithValue("@custid", custid);
            com_cart.Parameters.AddWithValue("@resid", resid);
            SqlDataReader dr = com_cart.ExecuteReader();
            List<CartModel> list = new List<CartModel>();
            while (dr.Read())
            {
                CartModel cart = new CartModel();
                cart.CustomerID = dr.GetInt32(0);
                cart.RestaurantID = dr.GetInt32(1);
                cart.itemID = dr.GetInt32(2);
                cart.itemName = dr.GetString(3);
                cart.itemquantity = dr.GetInt32(4);
                cart.itemprice = dr.GetDecimal(5);
                list.Add(cart);
            }
            con.Close();
            return list;
        }
        public string getrestaurantname(int id)
        {
            con.Open();
            SqlCommand com_name = new SqlCommand("Select RestaurantName from Restaurant where RestaurantID=@id", con);
            com_name.Parameters.AddWithValue("@id", id);
            string name = com_name.ExecuteScalar().ToString();
            con.Close();
            return name;
        }

        public void updatecart(CartModel model)
        {
            con.Open();
            SqlCommand com_subtract = new SqlCommand("update Cart set itemQuantity=@quan where CustomerID=@custid and RestaurantID=@resid and itemID=@itemid", con);
            com_subtract.Parameters.AddWithValue("@resid", model.RestaurantID);
            com_subtract.Parameters.AddWithValue("@custid", model.CustomerID);
            com_subtract.Parameters.AddWithValue("@itemid", model.itemID);
            com_subtract.Parameters.AddWithValue("@quan", model.itemquantity);
            com_subtract.ExecuteNonQuery();
            con.Close();
        }
        public void removecart(CartModel model)
        {
            con.Open();
            SqlCommand com_subtract = new SqlCommand("delete Cart where CustomerID=@custid and RestaurantID=@resid and itemID=@itemid", con);
            com_subtract.Parameters.AddWithValue("@resid", model.RestaurantID);
            com_subtract.Parameters.AddWithValue("@custid", model.CustomerID);
            com_subtract.Parameters.AddWithValue("@itemid", model.itemID);
            com_subtract.ExecuteNonQuery();
            con.Close();
        }
        public bool address(string address,int id)
        {
            con.Open();
            SqlCommand com_check = new SqlCommand("Select Count(*) from Address where CustomerID=@id and CustomerAddress=@add", con);
            com_check.Parameters.AddWithValue("@id", id);
            com_check.Parameters.AddWithValue("@add", address);
            int count = Convert.ToInt32(com_check.ExecuteScalar());
            if(count>0)
            {
                con.Close();
                return false;
            }
            else
            {
                SqlCommand com_add = new SqlCommand("insert Address values(@id,@add)", con);
                com_add.Parameters.AddWithValue("@id", id);
                com_add.Parameters.AddWithValue("@add", address);
                com_add.ExecuteNonQuery();
                con.Close();
                return true;
            }
        }

        public Summary viewdetails(int restid,int custid)
        {
            Summary sum = new Summary();
            sum.RestaurantName = getrestaurantname(restid);
            con.Open();
            SqlCommand com_find = new SqlCommand("Select SUM(itemQuantity),SUM(itemQuantity*itemPrice) from Cart where CustomerID=@cusid and RestaurantID=@resid", con);
            com_find.Parameters.AddWithValue("@cusid", custid);
            com_find.Parameters.AddWithValue("@resid", restid);
            SqlDataReader dr = com_find.ExecuteReader();
            if(dr.Read())
            {
                sum.TotalItems = dr.GetInt32(0);
                sum.TotalAmount = dr.GetDecimal(1);
            }
            con.Close();
            return sum;
        }
        public int OrderDetails(OrderDetail ord)
        {
            con.Open();
            SqlTransaction tranc = con.BeginTransaction();
            bool flag = true;
            SqlCommand com_order = new SqlCommand("insert OrderDetails values(@cusid,@resid,@transid,@amt,GetDate(),@add,'Order Placed')", con);
            com_order.Parameters.AddWithValue("@cusid", ord.CustomerID);
            com_order.Parameters.AddWithValue("@resid", ord.RestaurantID);
            com_order.Parameters.AddWithValue("@transid", ord.TransactionID);
            com_order.Parameters.AddWithValue("@amt", ord.OrderAmount);
            com_order.Parameters.AddWithValue("@add", ord.OrderAddress);
            com_order.Transaction = tranc;
            com_order.ExecuteNonQuery();
            SqlCommand com_id = new SqlCommand("Select @@identity", con);
            com_id.Transaction = tranc;
            int ordid = Convert.ToInt32(com_id.ExecuteScalar());
            ord.OrderID = ordid;
            SqlCommand com_itemlist = new SqlCommand("Select * from Cart where CustomerID=@cusid and RestaurantID=@resid", con);
            com_itemlist.Parameters.AddWithValue("@cusid", ord.CustomerID);
            com_itemlist.Parameters.AddWithValue("@resid", ord.RestaurantID);
            com_itemlist.Transaction = tranc;
            SqlDataReader dr = com_itemlist.ExecuteReader();
            List<CartModel> list = new List<CartModel>();
            while (dr.Read())
            {
                CartModel cart = new CartModel();
                cart.itemID = dr.GetInt32(2);
                cart.itemName = dr.GetString(3);
                cart.itemquantity = dr.GetInt32(4);
                cart.itemprice = dr.GetDecimal(5);
                list.Add(cart);
            }
            dr.Close();
            foreach(CartModel item in list)
            {
                SqlCommand com_additem = new SqlCommand("insert OrderItems values(@ordid,@itemid,@itemname,@itemquan,@itemprice)", con);
                com_additem.Parameters.AddWithValue("@ordid", ordid);
                com_additem.Parameters.AddWithValue("@itemid", item.itemID);
                com_additem.Parameters.AddWithValue("@itemname", item.itemName);
                com_additem.Parameters.AddWithValue("@itemquan", item.itemquantity);
                com_additem.Parameters.AddWithValue("@itemprice", item.itemprice);
                com_additem.Transaction = tranc;
                int count = Convert.ToInt32(com_additem.ExecuteNonQuery());
                if (count == 0)
                    flag = false;
            }
            SqlCommand com_emptycart = new SqlCommand("delete Cart where CustomerID=@cusid and RestaurantID=@resid", con);
            com_emptycart.Parameters.AddWithValue("@cusid", ord.CustomerID);
            com_emptycart.Parameters.AddWithValue("@resid", ord.RestaurantID);
            com_emptycart.Transaction = tranc;
            int countrow = Convert.ToInt32(com_emptycart.ExecuteNonQuery());
            if(countrow==1 && flag==true)
            {
                tranc.Commit();
                con.Close();
                return ordid;
            }
            else
            {
                tranc.Rollback();
                con.Close();
                return 0;
            }
        }

        public bool updatestaus(int transid)
        {
            con.Open();
            SqlCommand com_update = new SqlCommand("Update OrderDetails set OrderStatus='Cancelled' where TransactionID=@id", con);
            com_update.Parameters.AddWithValue("@id", transid);
            int count = Convert.ToInt32(com_update.ExecuteNonQuery());
            if(count==1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<SelectListItem> getaddress(int cusid)
        {
            con.Open();
            SqlCommand com_address = new SqlCommand("Select CustomerAddress from Address where CustomerID=@id", con);
            com_address.Parameters.AddWithValue("@id", cusid);
            SqlDataReader dr = com_address.ExecuteReader();
            List<SelectListItem> list = new List<SelectListItem>();
            while(dr.Read())
            {
                list.Add(new SelectListItem { Text = dr.GetString(0).ToString(), Value = dr.GetString(0).ToString() });
            }
            return list;
        }
        public List<OrderDetail> vieworders(int Custid)
        {
            con.Open();
            SqlCommand com_order = new SqlCommand("Select * from OrderDetails where CustomerID=@id", con);
            com_order.Parameters.AddWithValue("@id", Custid);
            SqlDataReader dr = com_order.ExecuteReader();
            List<OrderDetail> list = new List<OrderDetail>();
            while(dr.Read())
            {
                OrderDetail ord = new OrderDetail();
                ord.OrderID = dr.GetInt32(0);
                ord.CustomerID = dr.GetInt32(1);
                ord.RestaurantID = dr.GetInt32(2);
                ord.TransactionID = dr.GetInt32(3);
                ord.OrderAmount = dr.GetDecimal(4);
                ord.OrderDate = dr.GetDateTime(5).ToString();
                ord.OrderAddress = dr.GetString(6);
                ord.OrderStatus = dr.GetString(7);
                list.Add(ord);
            }
            con.Close();
            foreach(OrderDetail item in list)
            {
                item.RestaurantName = getrestaurantname(item.RestaurantID);
            }
            return list;
        }

        public bool changepassword(string userid,string old,string newpass)
        {
            return Membership.Provider.ChangePassword(userid, old, newpass);
        }


    }
}