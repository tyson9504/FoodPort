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
    public class AdminDAL
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Souradeep"].ConnectionString);
        public bool CheckMobileNumberDB(string MobileNo)
        {
            con.Open();
            SqlCommand com_count = new SqlCommand("Select Count(*) from Admins where AdminPhoneNumber=@number", con);
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
        public bool CheckMobileNumberDBres(string MobileNo)
        {
            con.Open();
            SqlCommand com_count = new SqlCommand("Select Count(*) from Restaurant where RestaurantPhoneNumber=@number", con);
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
        public bool AddAdmin(AdminModel model)
        {
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            SqlCommand com_add = new SqlCommand("insert Admins values(@name,@number,@email)", con);
            com_add.Parameters.AddWithValue("@name", model.AdminName);
            com_add.Parameters.AddWithValue("@email", model.AdminEmailID);
            com_add.Parameters.AddWithValue("@number", model.AdminPhoneNumber);
            com_add.Transaction = trans;
            com_add.ExecuteNonQuery();
            SqlCommand com_cusid = new SqlCommand("Select @@identity", con);
            com_cusid.Transaction = trans;
            int id = Convert.ToInt32(com_cusid.ExecuteScalar());
            model.AdminID = id;
            String userid = "A" + model.AdminPhoneNumber;
            MembershipCreateStatus status;
            Membership.CreateUser(userid, model.AdminPasswrod, model.AdminEmailID, "Question?", "Answer", true, out status);
            if (status == MembershipCreateStatus.Success)
            {
                Roles.AddUserToRole(model.AdminPhoneNumber, "Admin");
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
        public List<AdminModel> getAdmins()
        {
            con.Open();
            List<AdminModel> list = new List<AdminModel>();
            SqlCommand com_admin = new SqlCommand("Select * from Admins", con);
            SqlDataReader dr = com_admin.ExecuteReader();
            while (dr.Read())
            {
                AdminModel model = new AdminModel();
                model.AdminID = dr.GetInt32(0);
                model.AdminName = dr.GetString(1);
                model.AdminPhoneNumber = dr.GetString(2);
                model.AdminEmailID = dr.GetString(3);
                list.Add(model);
            }
            con.Close();
            return list;
        }

        public bool AddRestaurant(RestaurantModel model)
        {
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            SqlCommand com_add = new SqlCommand("insert Restaurant values(@name,@city,@locality,@state,@phone,@email,@open,@close,@status,@img)", con);
            com_add.Parameters.AddWithValue("@name", model.RestaurantName);
            com_add.Parameters.AddWithValue("@city", model.RestaurantCity);
            com_add.Parameters.AddWithValue("@locality", model.RestaurantLocality);
            com_add.Parameters.AddWithValue("@state", model.RestaurantState);
            com_add.Parameters.AddWithValue("@phone", model.RestaurantPhoneNumber);
            com_add.Parameters.AddWithValue("@email", model.RestaurantEmailID);
            com_add.Parameters.AddWithValue("@open", "");
            com_add.Parameters.AddWithValue("@close", "");
            com_add.Parameters.AddWithValue("@status", "Active");
            com_add.Parameters.AddWithValue("@img", "");
            com_add.Transaction = trans;
            com_add.ExecuteNonQuery();
            SqlCommand com_resid = new SqlCommand("Select @@identity", con);
            com_resid.Transaction = trans;
            int id = Convert.ToInt32(com_resid.ExecuteScalar());
            model.RestaurantID = id;
            String userid = "R" + model.RestaurantPhoneNumber;
            MembershipCreateStatus status;
            Membership.CreateUser(userid, model.RestaurantPassWord, model.RestaurantEmailID, "Question?", "Answer", true, out status);
            if (status == MembershipCreateStatus.Success)
            {
                Roles.AddUserToRole(model.RestaurantPhoneNumber, "Restaurant");
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
        public List<RestaurantModel> getRestaurants()
        {
            con.Open();
            List<RestaurantModel> list = new List<RestaurantModel>();
            SqlCommand com_restaurant = new SqlCommand("Select * from Restaurant", con);
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
        public List<CustomerModel> getCustomers()
        {
            con.Open();
            List<CustomerModel> list = new List<CustomerModel>();
            SqlCommand com_customer = new SqlCommand("Select * from Customer", con);
            SqlDataReader dr = com_customer.ExecuteReader();
            while (dr.Read())
            {
                CustomerModel model = new CustomerModel();
                model.CustomerID = dr.GetInt32(0);
                model.CustomerName = dr.GetString(1);
                model.CustomerEmail = dr.GetString(2);
                model.CustomerPhoneNumber = dr.GetString(3);
                model.CustomerAddress = dr.GetString(4);
                list.Add(model);
            }
            con.Close();
            return list;
        }

        public bool removerestaurant(int id)
        {
            con.Open();
            SqlTransaction trans = con.BeginTransaction();
            SqlCommand com_userid = new SqlCommand("Select RestaurantPhoneNumber from Restaurant where RestaurantID=@id",con);
            com_userid.Parameters.AddWithValue("@id", id);
            com_userid.Transaction = trans;
            string userid="R"+com_userid.ExecuteScalar().ToString();
            SqlCommand com_removemenu = new SqlCommand("delete Menu where RestaurantID=@id", con);
            com_removemenu.Parameters.AddWithValue("@id", id);
            com_removemenu.Transaction = trans;
            com_removemenu.ExecuteNonQuery();
            SqlCommand com_removetab = new SqlCommand("update Restaurant set RestaurantStatus='Inactive' where RestaurantID=@id", con);
            com_removetab.Parameters.AddWithValue("@id", id);
            com_removetab.Transaction = trans;
            com_removetab.ExecuteNonQuery();
            bool status = Membership.DeleteUser(userid, true);
            if(status)
            {
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
    }
}