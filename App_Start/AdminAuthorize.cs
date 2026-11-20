using System.Linq;
using System.Web;
using System.Web.Mvc;
using Đề_tài.Models; // Nhớ đổi thành namespace chứa DB Context của bạn (ChildStoreEntities)

namespace Đề_tài.App_Start // Hoặc namespace Đề_tài.Filters nếu bạn để trong thư mục Filters
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // 1. Kiểm tra xem người dùng đã đăng nhập chưa (Có Cookie hợp lệ không)
            // Nếu chưa đăng nhập, hàm base sẽ tự động trả về false và đá ra trang Login
            if (!base.AuthorizeCore(httpContext))
                return false;

            // 2. Lấy tên đăng nhập từ Cookie của người dùng
            string currentUser = httpContext.User.Identity.Name;

            // 3. Kết nối Database để kiểm tra quyền
            using (var db = new ChildStoreEntities())
            {
                // Tìm user trong DB
                var user = db.Users.FirstOrDefault(u => u.Username == currentUser);

                // Nếu tìm thấy và quyền là "A" (hoặc "Admin") thì cho phép đi tiếp
                if (user != null && (user.UserRole == "A"))
                {
                    return true;
                }
            }

            // Nếu không tìm thấy hoặc không phải Admin, chặn truy cập
            return false;
        }

        // (Tùy chọn) Chuyển hướng về trang Login nếu bị chặn
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Nếu bạn muốn đá về trang Login chung của khách hàng:
            filterContext.Result = new RedirectResult("~/Account/Login");

            // Hoặc nếu bạn muốn đá về trang báo lỗi 403 (Cấm truy cập):
            // filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
        }
    }
}