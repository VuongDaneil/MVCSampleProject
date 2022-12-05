using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCSampleProject.Models;
using System.Security.Cryptography;
namespace ProjectMVC5.Controllers
{
    public class AccountController : Controller
    {
        private ProductsFPTSEntities1 _db = new ProductsFPTSEntities1();
        // GET: Account
        public ActionResult Index()
        {
            if (Session["idUser"] != null)
            {
                return RedirectToAction("Index", "Products", new { area = "" });
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        //GET: SignUp

        public ActionResult SignUp()
        {
            return View();
        }

        //POST: SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Account _user)
        {
            if (ModelState.IsValid)
            {
                var check = _db.Accounts.FirstOrDefault(s => s.UserName == _user.UserName);
                if (check == null)
                {
                    _user.UserPassword = toMD5(_user.UserPassword);
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.Accounts.Add(_user);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Username already exists";
                    return View();
                }
                 
 
            }
            return View();
             
            
        }
 
        public ActionResult Login()
        {
            return View();
        }
 
 
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account user)
        {
            if (user.ToString().Length > 0)
            {
                var f_password = toMD5(user.UserPassword);
                var data =_db.Accounts.Where(s => s.UserName.Equals(user.UserName) && s.UserPassword.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["Username"] = data.FirstOrDefault().UserName;
                    Session["UserID"] = data.FirstOrDefault().UserID;
                    return RedirectToAction("Index", "Products", new { area = "" });
                }
                else
                {
                    ViewBag.error = "Login failed";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
         
 
        //Logout
        public ActionResult Logout()
        {
            //reset session
            Session.Clear();
            return RedirectToAction("Login");
        }
 
 
 
        //ma hoa MD5
        public static string toMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;
 
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
                
            }
            return byte2String;
        }
        
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      