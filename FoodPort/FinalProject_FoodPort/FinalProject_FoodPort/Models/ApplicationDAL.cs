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
    public class ApplicationDAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Souradeep"].ConnectionString);
        public bool Login(LogInModel model)
        {
            if(model.UserType=="Admin")
            {
                String Userid = "A" + model.UserID;
                return Membership.ValidateUser(Userid, model.Password);
            }
            else if (model.UserType == "Restaurant")
            {
                String Userid = "R" + model.UserID;
                return Membership.ValidateUser(Userid, model.Password);
            }
            else
            {
                return Membership.ValidateUser(model.UserID, model.Password);
            }
        }
        public bool CheckMobileNumberDB(string MobileNo)
        {
            con.Open();
            SqlCommand com_count = new SqlCommand("Select Count(*) from Customer where CustomerPhoneNumber=@number", con);
            com_count.Parameters.AddWithValue("@number", MobileNo);
            int count = Convert.ToInt32(com_count.ExecuteScalar());
            con.Close();
            if (count == 0)
            {
                 return true;
            }
            else
            {
                return false;
            }
        }
        public bool AddCustomer(CustomerModel model,string image)
        {
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            SqlCommand com_add = new SqlCommand("insert Customer values(@name,@email,@number,@addr,@img)", con);
            com_add.Parameters.AddWithValue("@name", model.CustomerName);
            com_add.Parameters.AddWithValue("@email", model.CustomerEmail);
            com_add.Parameters.AddWithValue("@number", model.CustomerPhoneNumber);
            com_add.Parameters.AddWithValue("@addr", model.CustomerAddress);
            com_add.Parameters.AddWithValue("@img", "");
            com_add.Transaction = trans;
            com_add.ExecuteNonQuery();
            SqlCommand com_cusid = new SqlCommand("Select @@identity", con);
            com_cusid.Transaction = trans;
            int id = Convert.ToInt32(com_cusid.ExecuteScalar());
            model.CustomerID = id;
            if (image != "")
            {
                string imgadd = "/Images/" + model.CustomerID.ToString() + ".jpg";
                SqlCommand com_image = new SqlCommand("update Customer set CustomerImage=@imgadd where CustomerID=@id", con);
                com_image.Parameters.AddWithValue("@imgadd", imgadd);
                com_image.Parameters.AddWithValue("@id", model.CustomerID);
                com_image.Transaction = trans;
                com_image.ExecuteNonQuery();
            }
            MembershipCreateStatus status;
            Membership.CreateUser(model.CustomerPhoneNumber, model.CustomerPassword, model.CustomerEmail, model.SecurityQuestion, model.SecurityAnswer, true, out status);
            if (status == MembershipCreateStatus.Success)
            {
                Roles.AddUserToRole(model.CustomerPhoneNumber, "Customer");
                trans.Commit();
                con.Close();
                return true;
            }
            else
            {
                trans.Rollback();
                con.Close();
                return false;
            }
        }
        public List<string> Addresslist(string Address)
        {
            List<string> list = new List<string>();
            con.Open();
            Address = Address + "%";
            SqlCommand com_locality = new SqlCommand("Select distinct RestaurantLocality from Restaurant where RestaurantLocality Like(@add)", con);
            com_locality.Parameters.AddWithValue("@add", Address);
            SqlDataReader locality = com_locality.ExecuteReader();
            while (locality.Read())
            {
                list.Add(locality.GetString(0));
            }
            con.Close();
            con.Open();
            SqlCommand com_city = new SqlCommand("Select distinct RestaurantCity from Restaurant where RestaurantCity Like(@add)", con);
            com_city.Parameters.AddWithValue("@add", Address);
            SqlDataReader city = com_city.ExecuteReader();
            while (city.Read())
            {
                list.Add(city.GetString(0));
            }
            con.Close();
            con.Open();
            SqlCommand com_state = new SqlCommand("Select distinct RestaurantState from Restaurant where RestaurantState Like(@add)", con);
            com_state.Parameters.AddWithValue("@add", Address);
            SqlDataReader state = com_state.ExecuteReader();
            while(state.Read())
            {
                list.Add(state.GetString(0));
            }
            con.Close();
            return list;
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
    }
}