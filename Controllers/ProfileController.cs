using MVCSampleProject.common;
using MVCSampleProject.Models;
using System;
using System.Collections.Generic;
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
            currentUser = db.Accounts.Where(x => x.UserID == id).FirstOrDefault();
            if (currentUser != null)
            {
                return View("MyProfile", currentUser);
            }
            else return View("MyProfile");
        }
    }
}