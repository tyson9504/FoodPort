using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Security;

namespace FinalProject_FoodPort.Models
{
    public class CustomerModel
    {
        [Display(Name="Customer ID:")]
        public int CustomerID { get; set; }


        [Display(Name = "Name:")]
        [Required(ErrorMessage="*")]
        [StringLength(50, ErrorMessage = "Max 50 Chars")]
        public String CustomerName { get; set; }


        [Display(Name = "Email:")]
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Invalid Format")]
        [StringLength(50, ErrorMessage = "Max 50 Chars")]
        public String CustomerEmail { get; set; }


        [Display(Name = "Phone Number:")]
        [Required(ErrorMessage = "*")]
        [StringLength(10,MinimumLength=10, ErrorMessage = "Only 10 Chars")]
        [System.Web.Mvc.Remote("CheckMobileNumber", "FoodPort", ErrorMessage = "Already Exists")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Invalid Format!")]
        public String CustomerPhoneNumber { get; set; }


        [Display(Name = "Address:")]
        [Required(ErrorMessage = "*")]
        public String CustomerAddress { get; set; }


        [Display(Name = "Image:")]
        public String CustomerImage { get; set; }


        [Display(Name = "Password:")]
        [Required(ErrorMessage = "*")]
        [StringLength(50, ErrorMessage = "Max 50 Chars")]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[$@!%*#?&])[A-Za-z\\d@$!%*#?&\\s]{6,}$", ErrorMessage = "Invalid Password Format! (Should be Alphanumeric and contain ateast one special character and minimum length of 6 characters)")] 
        public String CustomerPassword { get; set; }


        [Display(Name = "Security Question:")]
        [Required(ErrorMessage = "*")]
        public String SecurityQuestion { get; set; }


        [Display(Name = "Security Answer:")]
        [Required(ErrorMessage = "*")]
        public String SecurityAnswer { get; set; }
    }
}