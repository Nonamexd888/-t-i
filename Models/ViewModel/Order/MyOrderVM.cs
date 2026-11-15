using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList; // Cần using PagedList

namespace Đề_tài.Models.ViewModel.Order
{
    public class MyOrderVM
    {
        public string SearchTerm { get; set; }

        // Các thuộc tính hỗ trợ phân trang
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        // Danh sách đơn hàng đã phân trang
        public IPagedList<Đề_tài.Models.Order> Orders { get; set; }
    }
}