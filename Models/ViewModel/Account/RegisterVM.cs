using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Đề_tài.Models.ViewModel.Account
{
    public class RegisterVM
    {
        [Required(ErrorMessage ="Buộc phải nhập tên đăng nhập!")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Tên đăng nhập phải từ 1 đến 50 ký tự")]
        [RegularExpression(@"^(?=.*[a-zA-Z])[a-zA-Z0-9_.-]+$",
    ErrorMessage = "Tên đăng nhập chỉ được chứa chữ, số và các ký tự _ . - và phải có ít nhất 1 chữ cái")]
        [Display(Name ="Tên đăng nhập")]
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
        [Compare("Password",ErrorMessage ="Mật khẩu và mật khẩu xác nhận không khớp!")]
        [Display(Name = "Mật khẩu xác nhận")]
        public string ConfirmedPassword { get; set; }


        [Required(ErrorMessage = "Buộc phải nhập họ và tên!")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Họ và tên phải từ 1 đến 50 ký tự")]
        [RegularExpression(@"^[a-zA-Z\s]+$",
    ErrorMessage = "Họ và tên chỉ được chứa chữ")]
        [Display(Name = "Họ và tên")]
        public string CustomerName { get; set; }


        [Required(ErrorMessage ="Số điện thoại bắt buộc phải nhập")]
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Chỉ được chứa số!")]
        [StringLength(12, MinimumLength = 10, ErrorMessage = "Số điện thoại phải từ 10 đến 12 số")]
        [Display(Name ="Số điện thoại")]
        [DataType(DataType.PhoneNumber)]
        public string CustomerPhone { get; set; }


        [Required(ErrorMessage = "Làm ơn, email nè nhập hộ tui nha!")]
        [Display(Name = "Nhập email(Chỉ nhận gmail)")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Email phải có định dạng @gmail.com")]
        [DataType(DataType.EmailAddress)]
        public string CustomerEmail { get; set; }


        [Required(ErrorMessage = "Rồi không nhập địa chỉ rồi sao giao hàng?")]
        [Display(Name = "Địa chỉ")]
        [StringLength(200, ErrorMessage = "Địa chỉ không quá 200 ký tự")]
        public string CustomerAddress { get; set; }
    }
}