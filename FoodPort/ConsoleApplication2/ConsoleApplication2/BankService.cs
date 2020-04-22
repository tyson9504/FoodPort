using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Data.SqlClient;
using System.Configuration;

namespace FoodPort_Payment
{
    class BankService:IBankService
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Souradeep"].ConnectionString);
        [OperationBehavior(TransactionScopeRequired = true)]
        public int MakePayment(Transaction trans)
        {
            con.Open();
            bool flag = false;
            Transaction transservice = new Transaction();
            SqlTransaction tranc = con.BeginTransaction();
            SqlCommand com_verify = new SqlCommand("Select * from AccountsCustomer", con);
            com_verify.Transaction = tranc;
            SqlDataReader dr = com_verify.ExecuteReader();
            if (dr.Read())
            {
                transservice.CustomerAccountNumber = dr.GetString(0);
                transservice.CustomerAccountHolderName = dr.GetString(1);
                transservice.CustomerAccountBalance = dr.GetDecimal(2);
                transservice.AccountCardNumber = dr.GetString(3);
                transservice.Validfrom = dr.GetString(4);
                transservice.ValidTo = dr.GetString(5);
                transservice.CVV = dr.GetString(6);
            }
            con.Close();
            con.Open();
            if (transservice.CustomerAccountBalance > trans.TransactionAmount)
            {
                if (transservice.CustomerAccountNumber==trans.CustomerAccountNumber && transservice.AccountCardNumber == trans.AccountCardNumber && transservice.Validfrom == trans.Validfrom && transservice.ValidTo == trans.ValidTo && transservice.CVV == trans.CVV)
                {
                    SqlCommand com_customerupdate = new SqlCommand("Update AccountsCustomer set AccountBalance-=@balance where AccountNumber=@acc", con);
                    com_customerupdate.Parameters.AddWithValue("@balance", trans.TransactionAmount);
                    com_customerupdate.Parameters.AddWithValue("@acc", trans.CustomerAccountNumber);
                    com_customerupdate.Transaction = tranc;
                    com_customerupdate.ExecuteNonQuery();
                    SqlCommand com_resupdate = new SqlCommand("Update AccountsRestaurant set AccountBalance+=@balance where AccountHolderName=@name", con);
                    com_resupdate.Parameters.AddWithValue("@balance", trans.TransactionAmount);
                    com_resupdate.Parameters.AddWithValue("@name", trans.RestaurantAccountHolderName);
                    com_resupdate.Transaction = tranc;
                    com_resupdate.ExecuteNonQuery();
                    SqlCommand com_resacc = new SqlCommand("Select AccountNumber from AccountsRestaurant where AccountHolderName=@name", con);
                    com_resacc.Parameters.AddWithValue("@name", trans.RestaurantAccountHolderName);
                    com_resacc.Transaction = tranc;
                    trans.RestaurantAccountNumber = com_resacc.ExecuteScalar().ToString();
                    SqlCommand com_trans = new SqlCommand("insert BankTransaction values(@cusno,@resno,'Online Payment',@amt,GetDate())", con);
                    com_trans.Parameters.AddWithValue("@cusno", trans.CustomerAccountNumber);
                    com_trans.Parameters.AddWithValue("@resno", trans.RestaurantAccountNumber);
                    com_trans.Parameters.AddWithValue("@amt", trans.TransactionAmount);
                    com_trans.Transaction = tranc;
                    com_trans.ExecuteNonQuery();
                    SqlCommand com_id = new SqlCommand("Select @@identity", con);
                    int id = Convert.ToInt32(com_id.ExecuteScalar().ToString());
                    if (id > 0)
                    {
                        transservice.TransactionID = id;
                        flag = true;
                    }
                }
            }
            if (flag)
            {
                tranc.Commit();
                con.Close();
                return transservice.TransactionID;
            }
            else
            {
                tranc.Rollback();
                con.Close();
                return 0;
            }
        }
    }
}
