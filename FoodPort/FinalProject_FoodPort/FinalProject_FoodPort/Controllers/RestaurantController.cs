using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FinalProject_FoodPort.Models;

namespace FinalProject_FoodPort.Controllers
{
    [Authorize(Roles = "Restaurant")]
    public class RestaurantController : Controller
    {
        //
        // GET: /Restaurant/
        RestaurantDAL dal = new RestaurantDAL();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddMenu()
        {
            List<SelectListItem> itemcategories = new List<SelectListItem>();
            itemcategories.Add(new SelectListItem { Text = "Select", Value = "" });
            itemcategories.Add(new SelectListItem { Text = "Indian", Value = "Indian" });
            itemcategories.Add(new SelectListItem { Text = "Chinese", Value = "Chinese" });
            itemcategories.Add(new SelectListItem { Text = "Continental", Value = "Continental" });
            ViewBag.itemcategories = itemcategories;
            List<SelectListItem> itemtype = new List<SelectListItem>();
            itemtype.Add(new SelectListItem { Text = "Select", Value = "" });
            itemtype.Add(new SelectListItem { Text = "Veg", Value = "Veg" });
            itemtype.Add(new SelectListItem { Text = "Non-Veg", Value = "Non-Veg" });
            ViewBag.itemtype = itemtype;
            return View();
        }
        [HttpPost]
        public ActionResult AddMenu(MenuModel model,HttpPostedFileBase itemimagedb)
        {
            List<SelectListItem> itemcategories = new List<SelectListItem>();
            itemcategories.Add(new SelectListItem { Text = "Select", Value = "" });
            itemcategories.Add(new SelectListItem { Text = "Indian", Value = "Indian" });
            itemcategories.Add(new SelectListItem { Text = "Chinese", Value = "Chinese" });
            itemcategories.Add(new SelectListItem { Text = "Continental", Value = "Continental" });
            ViewBag.itemcategories = itemcategories;
            List<SelectListItem> itemtype = new List<SelectListItem>();
            itemtype.Add(new SelectListItem { Text = "Select", Value = "" });
            itemtype.Add(new SelectListItem { Text = "Veg", Value = "Veg" });
            itemtype.Add(new SelectListItem { Text = "Non-Veg", Value = "Non-Veg" });
            ViewBag.itemtype = itemtype;
            if (ModelState.IsValid)
            {
                bool status;
                model.RestaurantID = dal.getresid(User.Identity.Name);
                if (itemimagedb != null)
                {
                    itemimagedb.SaveAs(Server.MapPath("/Images/MenuImages/" + model.RestaurantID + model.ItemID + ".jpg"));
                    status = dal.AddMenu(model,"image");
                }
                else
                {
                    status = dal.AddMenu(model, "");
                }
                
                if (status)
                {
                    
                    ModelState.Clear();
                    Response.Write("<script>alert('Item Added! Item ID: " + model.ItemID + "')</script>");
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    Response.Write("<script>alert('Process Failed! Try Again!')</script>");
                    ModelState.Clear();
                    return View();
                }
            }
            else
            {
                Response.Write("<script>alert('Process Failed! Try Again!')</script>");
                ModelState.Clear();
                return View();
            }
        }
        public ActionResult ViewMenu()
        {
            List<MenuModel> list = new List<MenuModel>();
            int id = dal.getresid(User.Identity.Name);
            list = dal.getMenu(id);
            if(list.Count>0)
            {
                return View(list);
            }
            else
            {
                ViewBag.msg = "No Menu Exists!";
                return View();
            }
            
        }
        public ActionResult ViewProfile()
        {
            RestaurantModel model = new RestaurantModel();
            string phonenumber = User.Identity.Name;
            model = dal.getRestaurant(phonenumber);
            return View(model);
        }
        public ActionResult UpdateProfile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateProfile(string email, string opentime, string closetime, HttpPostedFileBase restaurantimagedb)
        {
            RestaurantModel model = new RestaurantModel();
            model.RestaurantPhoneNumber = User.Identity.Name;
            model.RestaurantID = dal.getresid(model.RestaurantPhoneNumber);
            if (restaurantimagedb != null)
            {
                restaurantimagedb.SaveAs(Server.MapPath("/Images/Restaurant/" + model.RestaurantID + ".jpg"));
                model.Image = "/Images/Restaurant/" + model.RestaurantID + ".jpg";
            }
            model.RestaurantEmailID = email;
            model.OpenTime = opentime;
            model.CloseTime = closetime;
            bool status = dal.UpadateProfile(model);
            if (status)
            {
                Response.Write("<script>alert('Profile Updated Successfully!')</script>");
                ModelState.Clear();
                return View();
            }
            else
            {
                Response.Write("<script>alert('Profile Not Updated!')</script>");
                ModelState.Clear();
                return View();
            }
        }
        public ActionResult ModifyMenu()
        {
            string Number = User.Identity.Name;
            int id = dal.getresid(Number);
            List<SelectListItem> items = dal.getitem(id);
            ViewBag.Items = items;
            return View();
        }
        [HttpPost]
        public ActionResult ModifyMenu(string ItemID, string name, string itemPrice)
        {
            if (ModelState.IsValid)
            {
                MenuModel model = new MenuModel();
                if (itemPrice != "")
                {
                    decimal p = Convert.ToDecimal(itemPrice);
                    model.ItemPrice = p;
                }
                string Number = User.Identity.Name;
                int id = dal.getresid(Number);
                List<SelectListItem> items = dal.getitem(id);
                ViewBag.Items = items;

                int id2 = Convert.ToInt32(ItemID);
                model.ItemID = id2;
                model.ItemName = name;
                model.RestaurantID = id;

                bool status = dal.modifymenu(model);
                if (status)
                {
                    Response.Write("<script>alert('Profile Updated Successfully!')</script>");
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    Response.Write("<script>alert('Profile Not Updated!')</script>");
                    ModelState.Clear();
                    return View();
                }
            }
            else
            {
                Response.Write("<script>alert('Profile Not Updated!')</script>");
                ModelState.Clear();
                return View();
            }
        }
        public ActionResult removeMenu(string resid,string itemid)
        {
            int Rid = Convert.ToInt32(resid);
            int Iit = Convert.ToInt32(itemid);
            bool status = dal.removemenu(Rid, Iit);
            if(status)
            {
                Response.Write("<script>alert('Item deleted!')</script>");
                return RedirectToAction("ViewMenu");
            }
            else
            {
                Response.Write("<script>alert('Process Failed!')</script>");
                return RedirectToAction("ViewMenu");
            }
        }

        public ActionResult ViewOrders()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Order Placed", Value = "Order Placed" });
            list.Add(new SelectListItem { Text = "Order Confirmed", Value = "Order Confirmed" });
            list.Add(new SelectListItem { Text = "Out for Delivery", Value = "Out for Delivery" });
            list.Add(new SelectListItem { Text = "Order Delivered", Value = "Order Delivered" });
            string Number = User.Identity.Name.ToString();
            int id = dal.getresid(Number);
            ViewBag.values = list;
            List<OrderDetail> list1 = dal.vieworders(id);
            return View(list1);
        }
        [HttpPost]
        public ActionResult ViewOrders(int OrderId, string status)
        {
            int order = Convert.ToInt32(OrderId);
            dal.updatestatus(order,status);
            List<SelectListItem> list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Order Placed", Value = "Order Placed" });
            list.Add(new SelectListItem { Text = "Order Confirmed", Value = "Order Confirmed" });
            list.Add(new SelectListItem { Text = "Out for Delivery", Value = "Out for Delivery" });
            list.Add(new SelectListItem { Text = "Order Delivered", Value = "Order Delivered" });
            string Number = User.Identity.Name.ToString();
            int id = dal.getresid(Number);
            ViewBag.values = list;
            List<OrderDetail> list1 = dal.vieworders(id);
            return View(list1);
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string old, string newpass)
        {
            string userid = "R"+User.Identity.Name.ToString();
            bool status = dal.changepassword(userid, old, newpass);
            if (status)
            {
                Response.Write("<script>alert('Password Changed Successfully!')</script>");
            }
            else
            {
                Response.Write("<script>alert('Process Failed... Try Again!')</script>");
            }
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index","FoodPort");
        }
    }
}
