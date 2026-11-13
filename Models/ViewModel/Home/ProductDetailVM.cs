using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Đề_tài.Models.ViewModel.Home
{
    public class ProductDetailVM
    {
        public Product Product { get; set; }

        [Required(ErrorMessage ="Buộc phải có số lượng")]
        public int Quantity { get; set; } = 1;
        //Tính giá trị tạm thời
        public decimal EstimatedValue => Quantity * Product.ProductPrice;

        //Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; } = 6;

        //Danh sách sản phẩm cùng danh mục
        public PagedList.IPagedList<Product> RelatedProducts { get; set; }

        //Danh sách sản phẩm bán chạy
        public PagedList.IPagedList<Product> TopProducts { get; set; }
    }
}