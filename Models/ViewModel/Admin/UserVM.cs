using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Đề_tài.Models.ViewModel.Admin
{
    public class UserVM
    {
        [Required(ErrorMessage = "Buộc phải nhập tên đăng nhập!")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Tên đăng nhập phải từ 5 đến 50 ký tự")]
        [RegularExpression(@"^(?=.*[a-zA-Z])[a-zA-Z0-9_.-]+$",
    ErrorMessage = "Tên đăng nhập chỉ được chứa chữ, số và các ký tự _ . - và phải có ít nhất 1 chữ cái")]
        [Display(Name = "Tên đăng nhập")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu!")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Mật khẩu phải từ 4 đến 50 ký tự")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Mật khẩu không được chứa khoảng trắng")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu xác nhận!")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Mật khẩu  xác nhận phải từ 4 đến 50 ký tự")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Mật khẩu xác nhận không được chứa khoảng trắng")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp!")]
        [Display(Name = "Mật khẩu xác nhận")]
        public string ConfirmedPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn quyền")]
        public string UserRole { get; set; } = "A";
    }
}