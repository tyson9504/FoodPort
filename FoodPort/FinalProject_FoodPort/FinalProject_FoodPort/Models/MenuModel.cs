using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace FinalProject_FoodPort.Models
{
    public class MenuModel
    {
        [Display(Name = "Restaurant ID")]
        public int RestaurantID { get; set; }
        
        [Display(Name = "Item ID")]
        [Required(ErrorMessage="*")]
        public int ItemID { get; set; }

        [Display(Name = "Item Category")]
        [Required(ErrorMessage = "*")]
        public string ItemCategory { get; set; }

        [Display(Name = "Item Type")]
        [Required(ErrorMessage = "*")]
        public string ItemVegNonVeg { get; set; }
        
        [Display(Name = "Item Name")]
        [Required(ErrorMessage = "*")]
        [StringLength(30, ErrorMessage = "Max 30 Chars")]
        public string ItemName { get; set; }

        [Display(Name = "Item Price")]
        [RegularExpression(@"^\d+\.\d{0,2}$", ErrorMessage = "Should be a decimal number with two digits after decimal")]
        [Range(0, 99999999.99, ErrorMessage = "Max 10 digits")]
        public decimal ItemPrice { get; set; }

        [Display(Name = "Item Details")]
        [Required(ErrorMessage = "*")]
        [StringLength(30, ErrorMessage = "Max 30 Chars")]
        public string ItemDetails { get; set; }
        [Display(Name = "Item Taste")]
        [Required(ErrorMessage = "*")]
        public string ItemTaste { get; set; }
        [Display(Name = "Image")]
        public string ItemImage { get; set; }
    }
}