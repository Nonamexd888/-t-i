using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList.Mvc;

namespace Đề_tài.Models.ViewModel
{
    //Tạo ViewModel để lưu trữ các tiêu chí và kết quả tìm kiếm
    public class SearchProductVM
    {
        public string SearchTerm { set; get; }

        //Các tiêu chí search theo giá
        public decimal? MinPrice { set; get; }
        public decimal? MaxPrice { set; get; }
        //Các tiêu chí search theo tuổi
        public int? MinAge { set; get; }
        public int? MaxAge { set; get; }

        //Thứ tự giảm dần theo giá hoặc theo tên
        public string SortOrder { get; set; }
        //Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }//Trang hiện tại 
        public int PageSize { get; set; }//Số lượng sản phẩm trên 1 trang
        //Danh sách sản phẩm đã phân trang
        public PagedList.IPagedList<Product> Products { get; set; }

        //public List<Product> Products { get; set; }
    }
}