using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCSampleProject.Models;
using System.Security.Cryptography;
using MVCSampleProject.Filters;
using MVCSampleProject.Identity;
using MVCSampleProject.common;

namespace ProjectMVC5.Controllers
{
    
    public class AccountController : Controller
    {
        public AppUser currentUser = new AppUser();

        private ProductsFPTSEntities _db = new ProductsFPTSEntities();
        // GET: Account
        //[MyAuthenFilter]
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
            ViewBag.RoleID = new SelectList(_db.UserRoles, "RoleID", "RoleName");
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
                    ViewBag.RoleID = new SelectList(_db.UserRoles, "RoleID", "RoleName");
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
                    currentUser.UserName = data.FirstOrDefault().UserName;
                    currentUser.Role = data.FirstOrDefault().UserRole.RoleName;
                    //currentUser.isBlook = data.FirstOrDefault().isBlock;
                    //add session
                    var userSession = new UserLogin();
                    userSession.UserID = data.FirstOrDefault().UserID;
                    userSession.UserName = data.FirstOrDefault().UserName;
                    userSession.Role = data.FirstOrDefault().UserRole.RoleName;
                    userSession.isBlock = data.FirstOrDefault().isBlock;
                    Session.Add(CommonConstants.USER_SESSION, userSession);

                    if (data.FirstOrDefault().RoleID == 1)
                    {
                        return RedirectToAction("Index", "Products", new { area = "Admin", id = 1 });
                    }
                    return RedirectToAction("Index", "Products", new { area = "",  id = 1 });
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
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
 
        //Blocked page
        public ActionResult Blocked()
        {
            return View();
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