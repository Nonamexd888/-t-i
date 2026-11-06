using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Đề_tài.Models.ViewModel.Account
{
    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu cũ!")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Mật khẩu phải từ 4 đến 50 ký tự")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Mật khẩu không được chứa khoảng trắng")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới!")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Mật khẩu phải từ 4 đến 50 ký tự")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Mật khẩu không được chứa khoảng trắng")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập mật khẩu xác nhận!")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Mật khẩu  xác nhận phải từ 4 đến 50 ký tự")]
        [RegularExpression(@"^\S+$", ErrorMessage = "Mật khẩu xác nhận không được chứa khoảng trắng")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp!")]
        [Display(Name = "Mật khẩu xác nhận")]
        public string ConfirmedNewPassword { get; set; }
    }
}