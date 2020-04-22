using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject_FoodPort.Models
{
    public class Transaction
    {
        public string CustomerAccountNumber { get; set; }
        public string CustomerAccountHolderName { get; set; }
        public decimal CustomerAccountBalance { get; set; }
        public string AccountCardNumber { get; set; }
        public string Validfrom { get; set; }
        public string ValidTo { get; set; }
        public string CVV { get; set; }
        public string RestaurantAccountNumber { get; set; }
        public string RestaurantAccountHolderName { get; set; }
        public decimal RestaurantAccountBalance { get; set; }
        public int TransactionID { get; set; }
        public decimal TransactionAmount { get; set; }
        public string TransactionDate { get; set; }
    }
}