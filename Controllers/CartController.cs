using MVCSampleProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCSampleProject.Controllers
{
    public class CartController : ClientBaseController
    {
        private const string cartSession = "cartSession";
        private ProductsFPTSEntities db = new ProductsFPTSEntities();

        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[cartSession];
            var list = new List<cartitem>();
            if (cart != null)
            {
                list = (List<cartitem>)cart;
            }
            return View(list);
        }

        public ActionResult AddItem(int _productId, int _quantity)
        {
            var product = getProductDetail(_productId);
            var cart = Session[cartSession];
            if (cart != null)
            {
                var list = (List<cartitem>)cart;

                if (list.Exists(x => x.Product.ProductID == _productId))
                {
                    foreach (var item in list)
                    {
                        if (item.Product.ProductID == _productId)
                        {
                            item.Quantity += _quantity;
                        }
                    }
                }
                else
                {
                    //Tao item
                    var item = new cartitem();
                    item.Product = product;
                    item.Quantity = _quantity;
                    list.Add(item);
                }
                // gan vao session
                Session[cartSession] = list;
            }
            else
            {
                //Tao item
                var item = new cartitem();
                item.Product = product;
                item.Quantity = _quantity;
                var list = new List<cartitem>();
                list.Add(item);

                // gan vao session
                Session[cartSession] = list;
            }
            return RedirectToAction("Index");
        }

        public Product getProductDetail(int id)
        {
            Product product = db.Products.Find(id);
            return product;
        }
    }
}