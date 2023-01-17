using MVCSampleProject.common;
using MVCSampleProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCSampleProject.Controllers
{
    public class ProfileController : ClientBaseController
    {
        private ProductsFPTSEntities db = new ProductsFPTSEntities();
        // GET: Profile
        public ActionResult Index(int? id)
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            id = session.UserID;

            Account currentUser = new Account();
            if(id == 0)
            {
                currentUser.UserID = 0;
                currentUser.UserName = session.UserName;
                currentUser.email = session.Email;

            }
            else
            {
                currentUser = db.Accounts.Where(x => x.UserID == id).FirstOrDefault();
            }
            
            if (currentUser != null)
            {
                return View("MyProfile", currentUser);
            }
            else return View("MyProfile");
        }

        public ActionResult EditProfile(int id)
        {
            Account currentUser = new Account();

            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName");
            currentUser = db.Accounts.Where(x => x.UserID == id).FirstOrDefault();
            return View(currentUser);
        }

        [HttpPost]
        public ActionResult EditProfile(Account _user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(_user).State = EntityState.Modified;
                db.SaveChanges();

                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                session.UserID = _user.UserID;
                session.UserName = _user.UserName;
                session.FullName = _user.FullName;
                session.Avatar = _user.Avatar;
                session.Email = _user.email;

                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName");
            return View(_user);
        }
    }
}