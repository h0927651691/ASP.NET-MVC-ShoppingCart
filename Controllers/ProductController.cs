using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCarts.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {

            //宣告回傳商品列表陣列result
            List<Models.Product> result = new List<Models.Product>();

            //使用CartsEntities類別，名稱為db
            using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
            {
                //使用LinQ語法抓取目前products資料庫中的資料
                result = (from s in db.Products select s).ToList();

                //將result傳送給檢視
                return View(result);
            }
                
        }
        //建立商品頁面
        public ActionResult Create()
        {
            return View(); 
        }
        //建立商品頁面 - 資料傳回處理
        [HttpPost] //指定只有POST方法才可進入
        public ActionResult Create(Models.Product postback)
        {
            using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
            {
                //將回傳資料postback加入至Products
                db.Products.Add(postback);

                //儲存異動資料
                db.SaveChanges();
            }
            return View();
        }
    }
}