using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FinalProject_FoodPort.Models
{
    public class CartModel
    {
        public int CustomerID { get; set; }
        public int RestaurantID { get; set; }
        public String RestaurantName { get; set; }
        public int itemID { get; set; }
        [Display(Name="Item Name")]
        public string itemName { get; set; }
        [Display(Name = "Item Price")]
        public decimal itemprice { get; set; }
        
        [Display(Name = "Item Quantity")]
        public int itemquantity { get; set; }
    }
}