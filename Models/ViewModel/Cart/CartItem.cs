using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

namespace Đề_tài.Models.ViewModel.Cart
{
    public class CartItem //Quản lý thông tin từng sản phẩm trong giỏ
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        //Giá cho một đơn vị sản phẩm
        public decimal UnitPrice { get; set; }
        public string ProductImage { get; set; }
        //Tổng giá cho mỗi sản phẩm
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}