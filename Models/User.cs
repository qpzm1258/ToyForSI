using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ToyForSI.Models
{
    public class User
    {
        public int userId{get;set;}
        [Required(ErrorMessage = "用户名不能为空")]
        [Remote(action: "VerifyName", controller: "User",ErrorMessage="{0}已被占用",AdditionalFields="userId")]
        [Display(Name="账号")]
        public string userName{get;set;}
        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(12, ErrorMessage = "{0} 长度必须是 {2} 位到 {1} 位.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string userPassword{get;set;} 
        public int isActive{get;set;}       
    }
}