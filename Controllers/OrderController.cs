﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace ShoppingCarts.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Models.OrderModel.Ship postback)
        {
            if( this.ModelState.IsValid)
            {
                //取得目前購物車
                var currentcart = Models.Cart.Operation.GetCurrentCart();

                //取得目前登入使用者Id
                var userId = HttpContext.User.Identity.GetUserId();

                using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
                {
                    //建立Order物件
                    var order = new Models.Order()
                    {
                        UserId = userId,
                        ReceiverName = postback.ReceiverName,
                        ReceiverPhone = postback.ReceiverPhone,
                        ReceiverAddress = postback.ReceiverAddress
                    };
                    //加其入Orders資料表後，儲存變更
                    db.Orders.Add(order);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException deEx)
                    {
                        foreach (var validationError in deEx.EntityValidationErrors)
                        {
                            foreach (var dbValidationError in validationError.ValidationErrors)
                            {
                                throw new Exception( dbValidationError.ErrorMessage);
                            }
                        }
                    }

                    //db.SaveChanges();

                    //取得購物車OrderDetail物件
                    var orderDetails = currentcart.ToOrderDetailList(order.Id);

                    //將其加入OrderDetails資料表後，儲存變更
                    db.OrderDetails.AddRange(orderDetails);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException deEx)
                    {
                        foreach (var validationError in deEx.EntityValidationErrors)
                        {
                            foreach (var dbValidationError in validationError.ValidationErrors)
                            {
                                throw new Exception(dbValidationError.ErrorMessage);
                            }
                        }
                    }

                }
                currentcart.ClearCart();
                return Content("訂購成功");
                    
            }
            return View();
        }

        public ActionResult MyOrder()
        {
            //取得目前使用者Id
            var userId = HttpContext.User.Identity.GetUserId();

            using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
            {
                var result = (from s in db.Orders
                              where s.UserId == userId
                              select s).ToList();
                return View(result);
            }
        }
        public ActionResult MyOrderDetail(int id)
        {
            using (Models.ShoppingCartsEntities db = new Models.ShoppingCartsEntities())
            {
                var result = (from s in db.OrderDetails
                              where s.OrderId == id
                              select s).ToList();
                if (result.Count == 0)
                {
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