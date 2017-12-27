using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToyForSI.Models.Enum;

namespace ToyForSI.Models
{
    public class Brand
    {
        public int brandId{get;set;}
        [Required(ErrorMessage="品牌名不能为空")]
        [Display(Name="品牌名",Prompt="请输入品牌名")]

        public string brandName{get;set;}
        [Display(Name="品牌链接",Prompt="例如：http://www.url.com")]
        [DataType(DataType.Url,ErrorMessage ="请输入正确的网址")]
        public string brandUrl { get; set; }
        [Display(Name = "客服电话")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "请输入正确的电话号码")]
        public string brandServiceHotLine { get; set; }
        [Display(Name = "客服邮箱",Prompt="例如：suport@mail.com")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "请输入正确的邮箱")]
        public string brandEmail { get; set; }
        [Display(Name = "log",Prompt="例如：http://www.url.com/logo.png")]
        [DataType(DataType.ImageUrl, ErrorMessage = "请输入正确logo网址")]
        public string brandLogo { get; set; }

        public ICollection<DevModel> devModels{get;set;}



    }
}