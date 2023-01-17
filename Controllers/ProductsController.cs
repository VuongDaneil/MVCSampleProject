
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCSampleProject.common;
using MVCSampleProject.Models;
using MVCSampleProject.Filters;

namespace MVCSampleProject.Controllers
{
    public class ProductsController : ClientBaseController
    {
        private ProductsFPTSEntities db = new ProductsFPTSEntities();

        // GET: Products
        //[MyAuthenFilter]
        public ActionResult Index(int id = 1, int page = 1, string search = "")
        {
            var products = db.Products.Include(p => p.Category);
            if (id == 1) { products = db.Products.Include(p => p.Category).Where(p => p.ProductName.Contains(search)); }
            else {
                products = db.Products.Where(x => x.CategoryID == id && x.ProductName.Contains(search));
            }

            //UserInfomation
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            ViewBag.id = session.UserID;
            ViewBag.Username = session.UserName;
            ViewBag.Role = session.Role;
            //Paging
            int NoOfRecordsPerPage = 8;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(products.ToList().Count) / Convert.ToDouble(NoOfRecordsPerPage)));
            int NoOfRecordsToSkip = (page - 1) * NoOfRecordsPerPage;

            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;

            ViewBag.SearchWords = search;
            products = products.OrderBy(x => x.ProductID).Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage);
            return View(products.ToList());
        }

        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Filter(int category, string searchvalue)
        {
            int a = category;
            string b = searchvalue;
            if (category != 1)
            {
                return RedirectToAction("Index", new { id = category, page = 1, search = searchvalue });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}