using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Đề_tài.Models.ViewModel.Admin
{
    public class OrderSearch
    {
        // Các thuộc tính dùng để lưu trữ từ khóa tìm kiếm
        public string SearchName { get; set; }
        public string SearchStatus { get; set; }
        public DateTime? SearchDate { get; set; }

        // Danh sách đơn hàng đã được phân trang
        public IPagedList<Đề_tài.Models.Order> Orders { get; set; }
    }
}