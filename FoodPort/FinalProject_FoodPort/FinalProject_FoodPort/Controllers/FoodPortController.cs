using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FinalProject_FoodPort.Models;

namespace FinalProject_FoodPort.Controllers
{
    [Authorize]
    public class FoodPortController : Controller
    {
        ApplicationDAL dal = new ApplicationDAL();
        
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult GetAddress(string Address)
        {
            List<string> addresslist = dal.Addresslist(Address);

            return Json(addresslist, JsonRequestBehavior.AllowGet);
        }
        
        [AllowAnonymous]
        public ActionResult LogIn()
        {
            List<SelectListItem> logintypes = new List<SelectListItem>();
            logintypes.Add(new SelectListItem { Text = "Select", Value = "" });
            logintypes.Add(new SelectListItem { Text = "Admin", Value = "Admin" });
            logintypes.Add(new SelectListItem { Text = "Customer", Value = "Customer" });
            logintypes.Add(new SelectListItem { Text = "Restaurant", Value = "Restaurant" });
            ViewBag.logintypes = logintypes;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogIn(LogInModel model)
        {
            List<SelectListItem> logintypes = new List<SelectListItem>();
            logintypes.Add(new SelectListItem { Text = "Select", Value = "" });
            logintypes.Add(new SelectListItem { Text = "Admin", Value = "Admin" });
            logintypes.Add(new SelectListItem { Text = "Customer", Value = "Customer" });
            logintypes.Add(new SelectListItem { Text = "Restaurant", Value = "Restaurant" });
            ViewBag.logintypes = logintypes;
            if (ModelState.IsValid)
            {
                bool status = dal.Login(model);
                if (status)
                {
                    if (model.UserType == "Admin")
                    {
                            FormsAuthentication.SetAuthCookie(model.UserID, model.RememberMe);
                            return RedirectToAction("Index", "Admin");
                    }
                    else if (model.UserType == "Customer")
                    {
                        if (Session["id"] == null)
                        {
                            FormsAuthentication.SetAuthCookie(model.UserID, model.RememberMe);
                            return RedirectToAction("Index", "Customer");
                        }
                        else
                        {
                            FormsAuthentication.SetAuthCookie(model.UserID, model.RememberMe);
                            return RedirectToAction("ShowMenu", "Customer");
                        }
                    }
                    else if (model.UserType == "Restaurant")
                    {
                        FormsAuthentication.SetAuthCookie(model.UserID, model.RememberMe);
                        return RedirectToAction("Index", "Restaurant");
                    }
                    else
                    {
                        ViewBag.error = "Invalid UserID or Password or type Mismatch!";
                        ModelState.Clear();
                        return View();
                    }
                }
                else
                {
                    ViewBag.error = "Invalid UserID or Password or type Mismatch!";
                    ModelState.Clear();
                    return View();
                }
            }
            else 
            {
                ViewBag.error = "Invalid UserID or Password or type Mismatch!";
                ModelState.Clear();
                return View();
            }
        }
        [AllowAnonymous]
        public ActionResult CreateAccount()
        {
            List<SelectListItem> questions = new List<SelectListItem>();
            questions.Add(new SelectListItem { Text = "Select", Value = "" });
            questions.Add(new SelectListItem { Text = "What was the name of your elementary / primary school?", Value = "What was the name of your elementary / primary school?" });
            questions.Add(new SelectListItem { Text = "What time of the day were you born?", Value = "What time of the day were you born?" });
            questions.Add(new SelectListItem { Text = "In what city or town does your nearest sibling live?", Value = "In what city or town does your nearest sibling live?" });
            questions.Add(new SelectListItem { Text = "What is the last name of the teacher who gave you your first failing grade?", Value = "What is the last name of the teacher who gave you your first failing grade?" });
            questions.Add(new SelectListItem { Text = "What is your pet’s name?", Value = "What is your pet’s name?" });
            questions.Add(new SelectListItem { Text = "In what year was your father born?", Value = "In what year was your father born?" });
            ViewBag.questions = questions;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult CreateAccount(CustomerModel model, HttpPostedFileBase customerimagedb)
        {
            if (ModelState.IsValid)
            {
                
                List<SelectListItem> questions = new List<SelectListItem>();                                                                
                questions.Add(new SelectListItem { Text = "Select", Value = "" });
                questions.Add(new SelectListItem { Text = "What was the name of your elementary / primary school?", Value = "What was the name of your elementary / primary school?" });
                questions.Add(new SelectListItem { Text = "What time of the day were you born?", Value = "What time of the day were you born?" });
                questions.Add(new SelectListItem { Text = "In what city or town does your nearest sibling live?", Value = "In what city or town does your nearest sibling live?" });
                questions.Add(new SelectListItem { Text = "What is the last name of the teacher who gave you your first failing grade?", Value = "What is the last name of the teacher who gave you your first failing grade?" });
                questions.Add(new SelectListItem { Text = "What is your pet’s name?", Value = "What is your pet’s name?" });
                questions.Add(new SelectListItem { Text = "In what year was your father born?", Value = "In what year was your father born?" });
                ViewBag.questions = questions;
                bool status;
                if (customerimagedb != null)
                {
                    customerimagedb.SaveAs(Server.MapPath("/Images/" + model.CustomerID + ".jpg"));
                    status = dal.AddCustomer(model,"image");
                }
                else
                {
                    status = dal.AddCustomer(model,"");
                }
                if (status)
                {
                    
                        ViewBag.id = model.CustomerID;
                        ViewBag.userid = model.CustomerPhoneNumber;
                        ViewBag.name = model.CustomerName;
                        ModelState.Clear();
                        return View("Success");

                }
                else
                {
                    ModelState.Clear();
                    ViewBag.error = "<script>alert('Process Failed! Try Again...')</script>";
                    return View();
                }
            }
            else
            {
                ModelState.Clear();
                ViewBag.error = "<script>alert('Process Failed! Try Again...')</script>";
                return View();
            }
        }
        [AllowAnonymous]
        public ActionResult AboutUs()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult CheckMobileNumber(string CustomerPhoneNumber)
        {
            bool status = dal.CheckMobileNumberDB(CustomerPhoneNumber);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
    }
}
