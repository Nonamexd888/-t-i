using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Đề_tài.Models.ViewModel.Home
{
    public class HomeProductVM
    {
        //Tiêu chí để search theo tên, mô tả sản phẩm
        //hoặc là loại sản phẩm
        public string SearchTerm { get; set; }

        //Các thuộc tính hỗ trợ phân trang

        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        //Danh sách các sản phẩm nổi bật
        public List<Product> FeaturedProducts { get; set; }

        //Danh sách các sản phẩm mới đã phân trang
        public PagedList.IPagedList<Product> NewProducts { get; set; }
    }
}