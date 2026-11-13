using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Đề_tài.Models.ViewModel.Cart
{
    public class Cart //Quản lý danh sách các sản phẩm trong giỏ
    {
        private List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items => items;

        //Thêm sản phẩm vào giỏ
        public void AddItem(int productId, string productImage, string productName, decimal unitPrice,int quantity,string category)
        {
            var existingItem = items.FirstOrDefault(item => item.ProductID == productId);
            if (existingItem == null)
            {
                items.Add(new CartItem { ProductID = productId, ProductImage = productImage, ProductName = productName, UnitPrice = unitPrice, Quantity = quantity });
            }
            else
            {
                existingItem.Quantity += quantity;
            }
        }

        //Xóa sản phẩm cụ thể khỏi giỏ
        public void RemoveItem(int productId)
        {
            items.RemoveAll(p => p.ProductID == productId);
        }

        public decimal TotalValue()
        {
            return items.Sum(i => i.TotalPrice);
        }

        //Làm trống giỏ hàng
        public void Clear()
        {
            items.Clear();
        }

        //Cập nhật giỏ hàng
        public void UpdateQuantity(int quantity, int productId)
        {
            var item = items.FirstOrDefault(i => i.ProductID == productId);
            if (item!= null)
            {
                item.Quantity = quantity;
            }
        }

    }
}