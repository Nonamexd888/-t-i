using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Đề_tài.Models
{
    public  class ProductMetadata
    {
        [HiddenInput]
        public int ProductID { get; set; }
        [HiddenInput]
        public int CategoryID { get; set; }
        [Required(ErrorMessage ="Buộc phải nhập tên sản phẩm")]
        [StringLength(50,MinimumLength =5,ErrorMessage ="Tên tối đa 50 kí tự và tối thiểu 5 kí tự")]
        public string ProductName { get; set; }

        [StringLength(2000,ErrorMessage = "Chỉ tối đa 300 từ")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage ="Buộc phải nhập giá")]
        [Range(0,double.MaxValue,ErrorMessage ="Giá phải dương")]

        public decimal ProductPrice { get; set; }
        public string ProductImage { get; set; }
        [Range(0, 99, ErrorMessage = "Độ tuổi từ 0 đến 99")]
        [Required(ErrorMessage ="Buộc phải nhập tuổi tối thiểu")]
        public int ProductMinAge { get; set; }
        [Required(ErrorMessage = "Buộc phải nhập số lượng tồn")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn không được âm")]
        public int ProductStock { get; set; }
    }
    public class CategoryMetadata
    {
        [HiddenInput]
        public int CategoryID { get; set; }
        [Required(ErrorMessage = "Buộc phải nhập tên danh mục")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Tên tối đa 50 kí tự và tối thiểu 5 kí tự")]
        public string CategoryName { get; set; }
        public string CategoryImage { get; set; }
    }
}