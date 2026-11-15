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
        //danh sách các sản phẩm trong giỏ hàng đã được phân nhóm theo Category
        public List<IGrouping<string, CartItem>> GroupedItems => items.GroupBy(i => i.Category).ToList();

        //Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 5;

        //Danh sách các sản phẩm cùng danh mục với các sản phẩm trong giỏ
        public PagedList.IPagedList<Product> SimilarProducts { get; set; }

        //Thêm sản phẩm vào giỏ
        public void AddItem(int productId, string productImage, string productName, decimal unitPrice,int quantity,int productStock,string category)
        {
            var existingItem = items.FirstOrDefault(item => item.ProductID == productId);
            if (existingItem == null)
            {
                items.Add(new CartItem { ProductID = productId, ProductImage = productImage, ProductName = productName, UnitPrice = unitPrice, Quantity = quantity, ProductStock = productStock, Category = category });
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
        public void UpdateQuantity( int productId,int quantity)
        {
            var item = items.FirstOrDefault(i => i.ProductID == productId);
            if (item!= null)
            {
                item.Quantity = quantity;
            }
        }

    }
}