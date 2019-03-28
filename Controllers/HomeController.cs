using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCarts.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
            {
                var result = (from s in db.Products select s).ToList();
                return View(result);
            }
              
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Index2()
        {
            return Content(
                "<hmtl><body><h1>ContentHTML</h1></body></html>");
        }
    }
}