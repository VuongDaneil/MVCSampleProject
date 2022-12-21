using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCSampleProject.Models;
using MVCSampleProject.Filters;

namespace MVCSampleProject.Controllers
{
    public class ProductsController : ClientBaseController
    {
        private ProductsFPTSEntities db = new ProductsFPTSEntities();

        // GET: Products
        //[MyAuthenFilter]
        public ActionResult Index(int id = 1, int page = 1)
        {
            var products = db.Products.Include(p => p.Category);
            if (id == 1) { products = db.Products.Include(p => p.Category); }
            else { products = db.Products.Where(x => x.CategoryID == id); }


            //Paging
            int NoOfRecordsPerPage = 9;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(products.ToList().Count) / Convert.ToDouble(NoOfRecordsPerPage)));
            int NoOfRecordsToSkip = (page - 1) * NoOfRecordsPerPage;

            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;

            products = products.OrderBy(x => x.ProductID).Skip(NoOfRecordsToSkip).Take(NoOfRecordsPerPage);
            return View(products.ToList());
        }
    }
}