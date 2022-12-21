using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCSampleProject.common;
using MVCSampleProject.Models;

namespace MVCSampleProject.Areas.Admin.Controllers
{
    public class UsersController : BaseController
    {
        private ProductsFPTSEntities _db = new ProductsFPTSEntities();
        // GET: Admin/Users
        public ActionResult Index()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            int id = session.UserID;

            List<Account> users = _db.Accounts.ToList();
            var itemToRemove = users.Single(x => x.UserID == id);
            users.Remove(itemToRemove);
            return View(users);
        }

        public ActionResult ChangeRole(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account us = _db.Accounts.Find(id);
            if (us == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(_db.UserRoles, "RoleID", "RoleName");
            return View(us);
        }

        [HttpPost]
        public ActionResult ChangeRole(Account us)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(us).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(_db.UserRoles, "RoleID", "RoleName");
            return View(us);
        }

        public ActionResult BlockUser(int id)
        {
            Account userToBlock = getAccount(id);
            _db.Entry(userToBlock).State = EntityState.Modified;
            userToBlock.isBlock = 1;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult UnblockUser(int id)
        {
            Account userToUnblock = getAccount(id);
            _db.Entry(userToUnblock).State = EntityState.Modified;
            userToUnblock.isBlock = 0;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        //Get user account
        private Account getAccount(int id)
        {
            Account acc = _db.Accounts.Find(id);
            return acc;
        }
    }
}