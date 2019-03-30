using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCarts.Models.Cart
{
    [Serializable] //可序列化
    public class Cart : IEnumerable<CartItem> //購物車類別
    {
        //建構值
        public Cart()
        {
            this.cartItems = new List<CartItem>();
        }
        //儲存所有商品
        private List<CartItem> cartItems;

        public int Count
        {
            get
            {
                return this.cartItems.Count;
            }
        }
        //取得商品總價
        public decimal TotalAmount
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
        //新增一筆Product，使用ProductId
        public bool AddProduct(int ProductId)
        {
            var findItem = this.cartItems
                                .Where(s => s.Id == ProductId)
                                .Select(s => s)
                                .FirstOrDefault();
            //判斷相同Id的CartItem是否已經存在購物車內
            if (findItem == default(Models.Cart.CartItem))
            {
                using (Models.ShoppingCartsEntities db = new ShoppingCartsEntities())
                {
                    var product = (from s in db.Products
                                   where s.Id == ProductId
                                   select s).FirstOrDefault();
                    if (product != default(Models.Product))
                    {
                        this.AddProduct(product);
                    }
                }
            }
            else
            {
                //存在購物車內，則將商品數量累加
                findItem.Quantity += 1;
            }
            return true;
        }
        private bool AddProduct(Product product)
        {
            //將Product轉為CartItem
            var cartItem = new Models.Cart.CartItem()
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1
            };
            //加入CartItem至購物車
            this.cartItems.Add(cartItem);
            return true;
        }
        public bool RemoveProduct(int ProductId)
        {
            var findItem = this.cartItems
                               .Where(s => s.Id == ProductId)
                               .Select(s => s)
                               .FirstOrDefault();
            //判斷相同Id的CartItem是否已經存在購物車內
            if(findItem == default(Models.Cart.CartItem))
            {
                //不存在購物車內，不需做任何動作
            }
            else
            {
                //存在購物車內，將商品移除
                this.cartItems.Remove(findItem);
            }
            return true;
        }
    
    #region IEnumerator
    IEnumerator<CartItem> IEnumerable<CartItem>.GetEnumerator()
    {
        return this.cartItems.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.cartItems.GetEnumerator();
    }

    #endregion
    }
}
