using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FinalProject_FoodPort.Models;

namespace FinalProject_FoodPort.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        AdminDAL dal = new AdminDAL(); 
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult CheckMobileNumber(string AdminPhoneNumber)
        {
            bool status = dal.CheckMobileNumberDB(AdminPhoneNumber);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public ActionResult CheckMobileNumberres(string RestaurantPhoneNumber)
        {
            bool status = dal.CheckMobileNumberDBres(RestaurantPhoneNumber);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddAdmin(AdminModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = dal.AddAdmin(model);
                ViewBag.id = model.AdminID;
                ViewBag.userid = model.AdminPhoneNumber;
                ViewBag.name = model.AdminName;
                if (status)
                {
                    ModelState.Clear();
                    return View("Success");
                }
                else
                {
                    ModelState.Clear();
                    ViewBag.error = "Process Failed! Try Again...";
                    return View();
                }
            }
            else
            {
                ModelState.Clear();
                ViewBag.error = "Process Failed! Try Again...";
                return View();
            }
        }
        public ActionResult ViewAdmin()
        {
            List<AdminModel> list = dal.getAdmins();
            if(list.Count>0)
            {
                return View(list);
            }
            else
            {
                ViewBag.msg = "No Records Found";
                return View("NoRecords");
            }
        }
        public ActionResult AddRestaurant()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRestaurant(RestaurantModel model)
        {
            if (ModelState.IsValid)
            {
                bool status = dal.AddRestaurant(model);
                ViewBag.id = model.RestaurantID;
                ViewBag.userid = model.RestaurantPhoneNumber;
                ViewBag.name = model.RestaurantName;
                if (status)
                {
                    ModelState.Clear();
                    return View("Success");
                }
                else
                {
                    ModelState.Clear();
                    ViewBag.error = "Process Failed! Try Again...";
                    return View();
                }
            }
            else
            {
                ModelState.Clear();
                ViewBag.error = "Process Failed! Try Again...";
                return View();
            }
        }
        public ActionResult ViewRestaurant()
        {
            List<RestaurantModel> list = dal.getRestaurants();
            if (list.Count > 0)
            {
                return View(list);
            }
            else
            {
                ViewBag.msg = "No Records Found";
                return View("NoRecords");
            }
        }
        public ActionResult ViewCustomer()
        {
            List<CustomerModel> list = dal.getCustomers();
            if (list.Count > 0)
            {
                return View(list);
            }
            else
            {
                ViewBag.msg = "No Records Found";
                return View("NoRecords");
            }
        }
        public ActionResult ShowMenu(string restaurantid)
        {
            RestaurantDAL dal2 = new RestaurantDAL();
            List<MenuModel> list = new List<MenuModel>();
            int id = Convert.ToInt32(restaurantid);
            list = dal2.getMenu(id);
            if (list.Count > 0)
            {
                return View(list);
            }
            else
            {
                ViewBag.msg = "No Menu Exists!";
                return View("NoMenu");
            }
        }
        public ActionResult RemoveRes(int restaurantid)
        {
            int id = Convert.ToInt32(restaurantid);
            bool status = dal.removerestaurant(id);
            if(status)
            {
                Response.Write("<script>alert('Restaurant removed successfully!')</script>");
                return RedirectToAction("ViewRestaurant");
            }
            else
            {
                Response.Write("<script>alert('Process failed!')</script>");
                return RedirectToAction("ViewRestaurant");
            }
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "FoodPort");
        }

    }
}
