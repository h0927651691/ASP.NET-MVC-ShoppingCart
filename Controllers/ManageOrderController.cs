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
        //public ActionResult SearchByUserName(string UserName)
        //{
        //    //儲存查詢出來的UserId
        //    string searchUserId = null;
        //    using (Models.UserEntities db = new Models.UserEntities())
        //    {
        //        searchUserId = (from s in db.AspNetUsers
        //                        where s.UserName == UserName
        //                        select s.Id).FirstOrDefault();
        //    }
        //    //如果有存在UserId
        //    if(!String.IsNullOrEmpty(searchUserId))
        //    {
        //        //則將此UserId的所有訂單找出
        //        using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
        //        {
        //            var result = (from s in db.Orders
        //                          where s.UserId == searchUserId
        //                          select s).ToList();
        //            //回傳 結果 至Index()的View
        //            return View("Index", result);
        //        }
        //    }
        //    else
        //    {
        //        //回傳空結果
        //        return View("Index", new List<Models.Order>());
        //    }
        //}

        public ActionResult SearchByUserName(string SearchType,string SearchString)
        {
            //判斷為查詢使用者
            if(SearchType == "UserId")
            {                //儲存查詢出來的UserId
                string searchUserId = null;
                using (Models.UserEntities db = new Models.UserEntities())
                {
                    searchUserId = (from s in db.AspNetUsers
                                    where s.UserName == SearchString
                                    select s.Id).FirstOrDefault();
                }
                //如果有存在UserId
                if (!String.IsNullOrEmpty(searchUserId))
                {
                    //則將此UserId的所有訂單找出
                    using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
                    {
                        var result = (from s in db.Orders
                                      where s.UserId == searchUserId
                                      select s).ToList();
                        //回傳 結果 至Index()的View
                        return View("Index", result);
                    }
                }
                else
                {
                    //回傳空結果
                    return View("Index", new List<Models.Order>());
                }

            }
            //判斷為查詢收件人電話
            else if (SearchType == "ReceiverPhone")
            {
                using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
                {
                    var result = (from s in db.Orders
                                  where s.ReceiverName == SearchString
                                  select s).ToList();
                    //回傳 結果 至Index()的View
                    return View("Index", result);
                }
            }
            //判斷為查詢收件人地址
            else if (SearchType == "ReceiverAddress")
            {
                using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
                {
                    var result = (from s in db.Orders
                                  where s.ReceiverAddress == SearchString
                                  select s).ToList();
                    //回傳 結果 至Index()的View
                    return View("Index", result);
                }
            }
            //判斷為查詢收件人名稱
            else if (SearchType == "ReceiverName")
            {
                using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
                {
                    var result = (from s in db.Orders
                                  where s.ReceiverPhone == SearchString
                                  select s).ToList();
                    //回傳 結果 至Index()的View
                    return View("Index", result);
                }
            }
            else
            {
                //回傳空結果
                return View("Index", new List<Models.Order>());
            }

        }

    }
}