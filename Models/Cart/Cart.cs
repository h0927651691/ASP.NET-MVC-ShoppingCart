using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCarts.Models.Cart
{
    [Serializable] //可序列化
    public class Cart //購物車類別
    {
        //建構值
        public Cart()
        {
            this.cartItems = new List<CartItem>();
        }
        //儲存所有商品
        public List<CartItem> cartItems;

        //取得商品總價
        public decimal TotalAmout
        {
            get
            {
                decimal totalAmount = 0.0m;
                foreach (var cartItem in this.cartItems)
                {
                    totalAmount = totalAmount + cartItem.Amount;
                }
                return totalAmount;
            }
        }
    }
}