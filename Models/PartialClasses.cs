using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Đề_tài.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
        [NotMapped]
        public HttpPostedFileBase UploadImg { get; set; }
    }
    [MetadataType(typeof(CategoryMetadata))]
    public partial class Category
    {
        [NotMapped]
        public HttpPostedFileBase UploadImg { get; set; }
    }
}