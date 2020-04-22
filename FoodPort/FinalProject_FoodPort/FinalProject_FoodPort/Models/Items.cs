using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject_FoodPort.Models
{
    public class Items
    {
      public int OrderID{get;set;}
      public int ItemID{get;set;}
      public string ItemName{get;set;}
      public int ItemQuantity{get;set;}
      public decimal ItemPrice { get; set; }
    }
}