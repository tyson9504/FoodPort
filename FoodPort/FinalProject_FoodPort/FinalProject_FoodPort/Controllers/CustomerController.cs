using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FinalProject_FoodPort.Models;
using System.Transactions;

namespace FinalProject_FoodPort.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        //
        // GET: /Customer/
        CustomerDAL dal = new CustomerDAL();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewProfile()
        {
            CustomerModel model = new CustomerModel();
            string phonenumber = User.Identity.Name;
            model = dal.getCustomer(phonenumber);
            return View(model);
        }
        public ActionResult LogOut()
        {
            Session.Remove("id");
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "FoodPort");
        }
        [HttpPost]
        public ActionResult Index(string Search)
        {
            List<RestaurantModel> list = dal.getRestaurants(Search);
            if (list.Count > 0)
            {
                return View("ViewRestaurant", list);
            }
            else
            {
                ViewBag.msg = "We Curently donot Serve in this Area";
                return View();
            }
        }
        public ActionResult GetAllRestaurants()
        {
            return View();
        }

        public ActionResult ShowMenu(int id)
        {
            RestaurantDAL dal2 = new RestaurantDAL();
            List<MenuModel> list = new List<MenuModel>();
            list = dal2.getMenu(id);
            if (list.Count > 0)
            {
                return PartialView("ShowMenu", list);
            }
            else
            {
                ViewBag.msg = "No Menu Exists!";
                return PartialView("NoMenu");
            }
        }

        public JsonResult AddtoCart(string itemid, string RestID)
        {
            int resid = Convert.ToInt32(RestID);
            int item = Convert.ToInt32(itemid);
            int Custid = dal.cusid(User.Identity.Name.ToString());
            bool status = dal.AddtoCart(resid, item, Custid);
            if (status)
            {
                string s = "Item Added to Cart!";
                return Json(s, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string s = "Item Already in Cart!";
                return Json(s, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult UpadateProfile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpadateProfile(string customerName, string customerEmail, string customerAddress, HttpPostedFileBase customerimagedb)
        {
            CustomerModel model = new CustomerModel();
            model.CustomerPhoneNumber = User.Identity.Name;
            model.CustomerID = dal.cusid(model.CustomerPhoneNumber);
            customerimagedb.SaveAs(Server.MapPath("/Images/" + model.CustomerID + ".jpg"));
            model.CustomerImage = "/Images/" + model.CustomerID + ".jpg";
            model.CustomerName = customerName;
            model.CustomerEmail = customerEmail;
            model.CustomerAddress = customerAddress;
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
        public ActionResult getpartialrestaurant(string Search)
        {
            List<RestaurantModel> list = dal.getRestaurants(Search);
            if (list.Count > 0)
            {
                return PartialView("ViewRestaurant", list);
            }
            else
            {
                ViewBag.msg = "We Curently donot Serve in this Area";
                return PartialView("NoRestuarants");
            }
        }
        public ActionResult ViewCart()
        {
            string s = User.Identity.Name.ToString();
            int id = dal.cusid(s);
            List<CartModel> list = dal.viewCart(id);
            foreach (var item in list)
            {
                item.RestaurantName = dal.getrestaurantname(item.RestaurantID);
            }
            if (list.Count > 0)
            {
                return View(list);
            }
            else
            {
                ViewBag.msg = "No Items Present in Cart";
                return View("NoItems");
            }
        }

        public ActionResult ViewCartDetails(string resid)
        {
            List<SelectListItem> values = new List<SelectListItem>();
            values.Add(new SelectListItem { Text = "1", Value = "1" });
            values.Add(new SelectListItem { Text = "2", Value = "2" });
            values.Add(new SelectListItem { Text = "3", Value = "3" });
            values.Add(new SelectListItem { Text = "4", Value = "4" });
            values.Add(new SelectListItem { Text = "5", Value = "5" });
            values.Add(new SelectListItem { Text = "6", Value = "6" });
            values.Add(new SelectListItem { Text = "7", Value = "7" });
            values.Add(new SelectListItem { Text = "8", Value = "8" });
            values.Add(new SelectListItem { Text = "9", Value = "9" });
            values.Add(new SelectListItem { Text = "10", Value = "10" });
            values.Add(new SelectListItem { Text = "11", Value = "11" });
            values.Add(new SelectListItem { Text = "12", Value = "12" });
            ViewBag.values = values;
            int restid = Convert.ToInt32(resid);
            string s = User.Identity.Name.ToString();
            int id = dal.cusid(s);
            List<CartModel> list = dal.ViewCartDetails(restid, id);
            ViewBag.id = restid;
            if (list.Count > 0)
            {
                return View(list);
            }
            else
            {
                ViewBag.msg = "No Items Present";
                return View("NoItems");
            }
        }
        public ActionResult update(string itemid, string RestID, string quan)
        {
            int resid = Convert.ToInt32(RestID);
            int item = Convert.ToInt32(itemid);
            int Custid = dal.cusid(User.Identity.Name.ToString());
            CartModel model = new CartModel();
            model.itemID = item;
            model.RestaurantID = resid;
            model.CustomerID = Custid;
            model.itemquantity = Convert.ToInt32(quan);
            dal.updatecart(model);
            return RedirectToAction("ViewCartDetails", new { resid = resid });
        }
        public ActionResult remove(string itemid, string RestID)
        {
            int resid = Convert.ToInt32(RestID);
            int item = Convert.ToInt32(itemid);
            int Custid = dal.cusid(User.Identity.Name.ToString());
            CartModel model = new CartModel();
            model.itemID = item;
            model.RestaurantID = resid;
            model.CustomerID = Custid;
            dal.removecart(model);
            return RedirectToAction("ViewCartDetails", new { resid = resid });
        }
        public ActionResult DeliveryAddress()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DeliveryAddress(string address)
        {
            string s = User.Identity.Name.ToString();
            int id = dal.cusid(s);
            bool status = dal.address(address, id);
            if (status)
            {
                ModelState.Clear();
                Response.Write("<script>alert('Address Added!')</script>");
                return View();
            }
            else
            {
                ModelState.Clear();
                Response.Write("<script>alert('Address Exists!')</script>");
                return View();
            }
        }
        public ActionResult proceedtopay(string id)
        {
            string s = User.Identity.Name.ToString();
            int cusid = dal.cusid(s);
            int resid = Convert.ToInt32(id);
            Summary sum = dal.viewdetails(resid, cusid);
            ViewBag.id = resid;
            ViewBag.amt = sum.TotalAmount;
            return View(sum);
        }
        public ActionResult makepayment(string resid, decimal amt)
        {
            ViewBag.id = resid;
            ViewBag.amt = amt;
            int cusid = dal.cusid(User.Identity.Name.ToString());
            ViewBag.address = dal.getaddress(cusid);
            return View();
        }
        public ActionResult transaction(string resid, decimal amt, ServiceReference1.Transaction trans, string Address)
        {
            bool flag = false;
            OrderDetail ord = new OrderDetail();
            int cusid = dal.cusid(User.Identity.Name.ToString());
            int restid = Convert.ToInt32(resid);
            ord.CustomerID = cusid;
            ord.RestaurantID = restid;
            ord.OrderAmount = amt;
            ord.OrderAddress = Address;
            trans.RestaurantAccountHolderName = dal.getrestaurantname(restid);
            trans.TransactionAmount = amt;
            using (TransactionScope trans1 = new TransactionScope())    //Starting a transaction
            {
                try
                {
                    ServiceReference1.BankServiceClient bank = new ServiceReference1.BankServiceClient();


                    int transid = bank.MakePayment(trans);
                    ord.TransactionID = transid;
                    if (transid > 0)
                    {
                        int Orderid = dal.OrderDetails(ord);
                        if (Orderid > 0)
                        {
                            trans1.Complete();
                            flag = true;
                        }
                        else
                        {
                            trans1.Dispose();
                        }
                    }
                    else
                    {
                        trans1.Dispose();
                    }
                }
                catch (Exception exp)
                {
                    trans1.Dispose();
                    ViewBag.error = "Transaction Aborted! Try Again!";
                }
                    if(flag)
                    {
                        ModelState.Clear();
                        ViewBag.transid = ord.TransactionID;
                        ViewBag.Orderid = ord.OrderID;
                        return View("Successful");
                    }
                    else
                    {
                        ModelState.Clear();
                        return View("UnSuccessful");
                    }

            }
        }
        public ActionResult cancelOrder(int transid)
        {
            bool flag = false;
            using (TransactionScope trans1 = new TransactionScope())    //Starting a transaction
            {
                try
                {
                    ServiceReference1.BankServiceClient bank = new ServiceReference1.BankServiceClient();
                    bool status = bank.CancelOrder(transid);
                    if(status)
                    {
                        if(dal.updatestaus(transid))
                        {
                            trans1.Complete();
                            flag = true;
                        }
                        else
                        {
                            trans1.Dispose();
                        }
                    }
                    else
                    {
                        trans1.Dispose();
                    }
                }
                catch(Exception e)
                {
                    trans1.Dispose();
                }
            }
            if(flag)
            {
                ViewBag.msg = "Order Cancelled";
                return RedirectToAction("ViewOrders");
            }
            else
            {
                ViewBag.msg = "Process Failed... Try Again!";
                return RedirectToAction("ViewOrders");
            }
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(string old,string newpass)
        {
            bool status = dal.changepassword(User.Identity.Name.ToString(), old, newpass);
            if(status)
            {
                Response.Write("<script>alert('Password Changed Successfully!')</script>");
            }
            else
            {
                Response.Write("<script>alert('Process Failed... Try Again!')</script>");
            }
            return View();
        }
        public ActionResult ViewOrders()
        {
            string s = User.Identity.Name.ToString();
            int cusid = dal.cusid(s);
            List<OrderDetail> list = dal.vieworders(cusid);
            if (list.Count > 0)
            {
                return View(list);
            }
            else
            {
                ViewBag.orders = "No Orders Placed till now!";
                return View("NoOrders");
            }
        }
    }
}
