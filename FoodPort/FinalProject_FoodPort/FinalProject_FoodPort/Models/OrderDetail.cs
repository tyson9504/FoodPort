using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject_FoodPort.Models
{
    public class OrderDetail
    {
      public int OrderID{get;set;}
      public int CustomerID{get;set;}
      public int RestaurantID{get;set;}
      public string RestaurantName { get; set; }
      public string CustomerName { get; set; }
      public string CustomerNumber { get; set; }
      public int TransactionID{get;set;}
      public decimal OrderAmount{get;set;}
      public string OrderDate{get;set;}
      public string OrderAddress{get;set;}
      public string OrderStatus { get; set; }
    }
}