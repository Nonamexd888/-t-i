using Đề_tài.Models.ViewModel.Cart;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Đề_tài.Models.ViewModel.Order
{
    public class CheckoutVM
    {
        public List<CartItem> CartItems { get; set; }

        public int CustomerID { get; set; }
        [Required(ErrorMessage ="Bắt buộc phải có ngày đặt hàng")]
        [Display(Name ="Ngày đặt hàng")]
        public System.DateTime OrderDate { get; set; }

        [Display(Name ="Tổng giá trị   ")]
        public decimal TotalAmount { get; set; }

        [Display(Name ="Trạng thái thanh toán ")]
        
        public string PaymentStatus { get; set; }
        [Display(Name = "Hình thức thanh toán   ")]
        [Required(ErrorMessage = "Bắt buộc phải chọn hình thức thanh toán")]
        public string PaymentMethod { get; set; }
        [Display(Name = "Hình thức giao hàng   ")]

        [Required(ErrorMessage = "Bắt buộc phải có chọn hình thức giao hàng")]
        public string ShippingMethod { get; set; }

        [Required(ErrorMessage = "Bắt buộc phải có địa chỉ giao hàng")]
        [Display(Name = "Địa chỉ giao hàng   ")]
        public string ShippingAddress { get; set; }
        public string Username { get; set; }

        //Các thuộc tính khác của đơn hàng
        public List<OrderDetail> OrderDetails { get; set; }

    }
}