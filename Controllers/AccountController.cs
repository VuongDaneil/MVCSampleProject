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
using Facebook;
using System.Configuration;

namespace MVCSampleProject.Controllers
{
    
    public class AccountController : Controller
    {
        public AppUser currentUser = new AppUser();

        private ProductsFPTSEntities _db = new ProductsFPTSEntities();
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

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
            SelectList a = new SelectList(_db.UserRoles, "RoleID", "RoleName");
            ViewBag.RoleID = new SelectList(_db.UserRoles.Where(x => x.RoleName != "Facebook user" && x.RoleName != "Google user"), "RoleID", "RoleName");
            return View();
        }

        //POST: SignUp
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(Account _user)
        {
            if (ModelState.IsValid)
            {
                var check = _db.Accounts.FirstOrDefault(s => s.email == _user.email && s.UserName == _user.UserName);
                if (check == null)
                {
                    _user.UserPassword = toMD5(_user.UserPassword);
                    _user.isBlock = 0;
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.Accounts.Add(_user);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Username already exists";
                    ViewBag.RoleID = new SelectList(_db.UserRoles.Where(x => x.RoleName != "Facebook user" || x.RoleName != "Google user"), "RoleID", "RoleName");
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
        public ActionResult Login(logininfo user)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid) {
                var f_password = toMD5(user.password);
                var data = _db.Accounts.Where(s => s.UserName.Equals(user.username) && s.UserPassword.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    currentUser.UserName = data.FirstOrDefault().UserName;
                    currentUser.Role = data.FirstOrDefault().UserRole.RoleName;
                    //currentUser.isBlook = data.FirstOrDefault().isBlock;
                    //add session
                    var userSession = new UserLogin();
                    userSession.UserID = data.FirstOrDefault().UserID;
                    userSession.UserName = data.FirstOrDefault().UserName;
                    userSession.Avatar = data.FirstOrDefault().Avatar;
                    userSession.Role = data.FirstOrDefault().UserRole.RoleName;
                    userSession.Email = data.FirstOrDefault().email;
                    userSession.isBlock = data.FirstOrDefault().isBlock;
                    Session.Add(CommonConstants.USER_SESSION, userSession);

                    if (data.FirstOrDefault().RoleID == 1)
                    {
                        return RedirectToAction("Index", "Products", new { area = "Admin", id = 1 });
                    }
                    return RedirectToAction("Index", "Products", new { area = "", id = 1 });
                }
                else
                {
                    ViewBag.error = errors;
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return View();
            }
        }

        public ActionResult FacebookLogin()
        {
            var fb = new FacebookClient();

            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppID"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            }) ;

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();

            dynamic result = fb.Post("oauth/access_token",new
            {
                client_id = ConfigurationManager.AppSettings["FbAppID"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });


            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;

                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email,picture");
                string email = me.email;
                string userName = me.first_name;
                string avatar = me.picture["data"]["url"];
                string fullname = me.first_name + me.last_name;


                var _user = new Account();
                _user.email = email;
                _user.UserName = userName;
                _user.FullName = fullname;
                _user.Avatar = avatar;
                _user.UserPassword = "";
                _user.ShippingAddress = "";
                _user.phone = "";
                _user.isBlock = 0;
                _user.RoleID = 3;

                var check = _db.Accounts.FirstOrDefault(s => s.email == email && s.UserName == userName); //Check

                if (check == null)
                {
                    SignUp(_user);

                    var data = _db.Accounts.Where(s => s.UserName.Equals(_user.UserName) && s.UserPassword.Equals(_user.email)).ToList();

                    //Luu vao session
                    var _userSession = new UserLogin();
                    _userSession.UserID = _user.UserID;
                    _userSession.UserName = _user.UserName;
                    _userSession.Avatar = avatar;
                    _userSession.Role = "Customer";
                    _userSession.Email = _user.email;
                    _userSession.isBlock = _user.isBlock;
                    Session.Add(CommonConstants.USER_SESSION, _userSession);
                    
                }
                else
                {
                    //Luu vao session
                    var userSession = new UserLogin();
                    userSession.UserID = check.UserID;
                    userSession.UserName = check.UserName;
                    userSession.Avatar = check.Avatar;
                    userSession.Role = check.UserRole.RoleName;
                    userSession.Email = check.email;
                    userSession.isBlock = check.isBlock;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }
            }
            return RedirectToAction("Index", "Products", new { area = "", id = 1 });
        }

        public ActionResult GoogleLogin()
        {
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

        public PartialViewResult getUserInfo()
        {
            var userInfo = (UserLogin)Session[CommonConstants.USER_SESSION];
            return PartialView("~/Views/Shared/_userPartialPage1.cshtml", userInfo);
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