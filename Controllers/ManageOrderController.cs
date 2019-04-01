using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCarts.Controllers
{
    public class ManageOrderController : Controller
    {
        // GET: ManageOrder
        public ActionResult Index()
        {
            using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
            {
                //取得Order中所有資料
                var result = (from s in db.Orders
                              select s).ToList();

                return View(result);
            }
        }
        public ActionResult Details(int id)
        {
            using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
            {
                //取得OrderId為傳入id的所有商品列表
                var result = (from s in db.OrderDetails
                             where s.OrderId == id select s).ToList();

                if( result.Count ==0)
                {
                    //如果商品數目為零，代表該訂單異常(無商品)，則導回商品列表
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(result);
                }
            }
        }

    }
}