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
    public class RestaurantDAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Souradeep"].ConnectionString);
        public bool AddMenu(MenuModel model,string image)
        {
            con.Open();
            SqlCommand com_add = new SqlCommand("insert Menu values(@resid,@category,@vegnonveg,@name,@price,@details,@taste,@image)", con);
            com_add.Parameters.AddWithValue("@resid", model.RestaurantID);
            com_add.Parameters.AddWithValue("@category", model.ItemCategory);
            com_add.Parameters.AddWithValue("@vegnonveg", model.ItemVegNonVeg);
            com_add.Parameters.AddWithValue("@name", model.ItemName);
            com_add.Parameters.AddWithValue("@price", model.ItemPrice);
            com_add.Parameters.AddWithValue("@details", model.ItemDetails);
            com_add.Parameters.AddWithValue("@taste", model.ItemTaste);
            com_add.Parameters.AddWithValue("@image", "");
            com_add.ExecuteNonQuery();
            SqlCommand com_id = new SqlCommand("Select @@identity",con);
            int id = Convert.ToInt32(com_id.ExecuteScalar());
            model.ItemID = id;
            if (image != "")
            {
                string imgadd = "/Images/MenuImages/" + model.RestaurantID + model.ItemID + ".jpg";
                SqlCommand com_image = new SqlCommand("update Menu set ItemImage=@imgadd where ItemID=@itemid and RestaurantID=@id", con);
                com_image.Parameters.AddWithValue("@imgadd", imgadd);
                com_image.Parameters.AddWithValue("@id", model.RestaurantID);
                com_image.Parameters.AddWithValue("@itemid", model.ItemID);
                com_image.ExecuteNonQuery();
            }
            if(id>0)
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
            }
        }
        public int getresid(string RestaurantPhoneNumber)
        {
            con.Open();
            SqlCommand com_id = new SqlCommand("Select RestaurantID from Restaurant where RestaurantPhoneNumber=@number", con);
            com_id.Parameters.AddWithValue("@number", RestaurantPhoneNumber);
            int id = Convert.ToInt32(com_id.ExecuteScalar());
            con.Close();
            return id;
        }

        public List<MenuModel> getMenu(int id)
        {
            con.Open();
            SqlCommand com_menu = new SqlCommand("Select * from Menu where RestaurantID=@id", con);
            com_menu.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = com_menu.ExecuteReader();
            List<MenuModel> list = new List<MenuModel>();
            while(dr.Read())
            {
                MenuModel model = new MenuModel();
                model.ItemID = dr.GetInt32(0);
                model.RestaurantID = dr.GetInt32(1);
                model.ItemCategory = dr.GetString(2);
                model.ItemVegNonVeg = dr.GetString(3);
                model.ItemName = dr.GetString(4);
                model.ItemPrice = dr.GetDecimal(5);
                model.ItemDetails = dr.GetString(6);
                model.ItemTaste = dr.GetString(7);
                model.ItemImage = dr.GetString(8);
                list.Add(model);
            }
            con.Close();
            return list;
        }
        public bool UpadateProfile(RestaurantModel model)
        {

            if (model.RestaurantEmailID != "")
            {
                con.Open();
                SqlCommand com_update = new SqlCommand("update Restaurant set RestaurantEmail=@name where RestaurantPhoneNumber=@number", con);
                com_update.Parameters.AddWithValue("@name", model.RestaurantEmailID);
                com_update.Parameters.AddWithValue("@number", model.RestaurantPhoneNumber);
                com_update.ExecuteNonQuery();
                con.Close();
            }
            if (model.OpenTime != "")
            {
                con.Open();
                SqlCommand com_update = new SqlCommand("update Restaurant set RestaurantOpenTime=@email where RestaurantPhoneNumber=@number", con);
                com_update.Parameters.AddWithValue("@email", model.OpenTime);
                com_update.Parameters.AddWithValue("@number", model.RestaurantPhoneNumber);
                com_update.ExecuteNonQuery();
                con.Close();
            }
            if (model.CloseTime != "")
            {
                con.Open();
                SqlCommand com_update = new SqlCommand("update Restaurant set RestaurantCloseTime=@add where RestaurantPhoneNumber=@number", con);
                com_update.Parameters.AddWithValue("@add", model.CloseTime);
                com_update.Parameters.AddWithValue("@number", model.RestaurantPhoneNumber);
                com_update.ExecuteNonQuery();
                con.Close();
            }
            if (model.Image != null)
            {
                con.Open();
                SqlCommand com_update = new SqlCommand("update Restaurant set RestaurantImage=@img where RestaurantPhoneNumber=@number", con);
                com_update.Parameters.AddWithValue("@img", model.Image);
                com_update.Parameters.AddWithValue("@number", model.RestaurantPhoneNumber);
                com_update.ExecuteNonQuery();
                con.Close();
            }
            return true;
        }
        public RestaurantModel getRestaurant(string Phonenumber)
        {
            con.Open();
            SqlCommand com_cus = new SqlCommand("Select * from Restaurant where RestaurantPhoneNumber=@phone", con);
            com_cus.Parameters.AddWithValue("@phone", Phonenumber);
            SqlDataReader dr = com_cus.ExecuteReader();
            RestaurantModel model = new RestaurantModel();
            if (dr.Read())
            {
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
            }
            if (model.Image == "")
            {
                model.Image = "/Images/blankimage.jpg";
            }
            con.Close();
            return model;
        }
        public List<SelectListItem> getitem(int id)
        {
            con.Open();
            SqlCommand com_item = new SqlCommand("Select ItemID, ItemName from Menu where RestaurantID=@id", con);
            com_item.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = com_item.ExecuteReader();
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Select", Value = "" });
            while(dr.Read())
            {
                list.Add(new SelectListItem{Text=dr.GetString(1), Value=dr.GetInt32(0).ToString()});
            }
            con.Close();
            return list;
        }
        public bool modifymenu(MenuModel model)
        {
            if (model.ItemName != "")
            {
                con.Open();
                SqlCommand com_update = new SqlCommand("update Menu set ItemName=@name where ItemID=@id and RestaurantID=@number", con);
                com_update.Parameters.AddWithValue("@name", model.ItemName);
                com_update.Parameters.AddWithValue("@number", model.RestaurantID);
                com_update.Parameters.AddWithValue("@id", model.ItemID);
                com_update.ExecuteNonQuery();
                con.Close();
            }
            if (model.ItemPrice != 0)
            {
                con.Open();
                SqlCommand com_update = new SqlCommand("update Menu set ItemPrice=@name where ItemID=@id and RestaurantID=@number", con);
                com_update.Parameters.AddWithValue("@name", model.ItemPrice);
                com_update.Parameters.AddWithValue("@number", model.RestaurantID);
                com_update.Parameters.AddWithValue("@id", model.ItemID);
                com_update.ExecuteNonQuery();
                con.Close();
            }
            return true;
        }
        public bool removemenu(int resid,int itemid)
        {
            con.Open();
            SqlCommand com_removemenu = new SqlCommand("delete Menu where ItemID=@itemid and RestaurantID=@resid", con);
            com_removemenu.Parameters.AddWithValue("@resid", resid);
            com_removemenu.Parameters.AddWithValue("@itemid", itemid);
            int status = Convert.ToInt32(com_removemenu.ExecuteNonQuery());
            if (status==1)
            {
                con.Close();
                return true;
            }
            else
            {
                con.Close();
                return false;
            }
        }

        public string cusname(int id)
        {
            con.Open();
            SqlCommand com_name = new SqlCommand("Select CustomerName from Customer where CustomerID=@id", con);
            com_name.Parameters.AddWithValue("@id", id);
            string name = com_name.ExecuteScalar().ToString();
            con.Close();
            return name;
        }
        public string cusnumber(int id)
        {
            con.Open();
            SqlCommand com_name = new SqlCommand("Select CustomerPhoneNumber from Customer where CustomerID=@id", con);
            com_name.Parameters.AddWithValue("@id", id);
            string name = com_name.ExecuteScalar().ToString();
            con.Close();
            return name;
        }

        public List<OrderDetail> vieworders(int id)
        {
            con.Open();
            SqlCommand com_order = new SqlCommand("Select * from OrderDetails where RestaurantID=@id", con);
            com_order.Parameters.AddWithValue("@id", id);
            SqlDataReader dr = com_order.ExecuteReader();
            List<OrderDetail> list = new List<OrderDetail>();
            while (dr.Read())
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
            foreach (OrderDetail item in list)
            {
                item.CustomerName = cusname(item.CustomerID);
                item.CustomerNumber = cusnumber(item.CustomerID);
            }
            return list;
        }
        public void updatestatus(int orderid,string status)
        {
            con.Open();
            SqlCommand com_status = new SqlCommand("update OrderDetails set OrderStatus=@status where OrderID=@id",con);
            com_status.Parameters.AddWithValue("@status", status);
            com_status.Parameters.AddWithValue("@id", orderid);
            com_status.ExecuteNonQuery();
            con.Close();
        }

        public bool changepassword(string userid, string old, string newpass)
        {
            return Membership.Provider.ChangePassword(userid, old, newpass);
        }
    }
}