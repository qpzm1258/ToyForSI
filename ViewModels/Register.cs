using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ToyForSI.ViewModels
{
    public class Register
    {
        [Required]
        [Display(Name="用户名")]
        [StringLength(20, ErrorMessage = "{0} 必须是至少 {2} 位至多 {1} 位数字或字母.", MinimumLength = 6)]
        [RegularExpression(@"[0-9a-zA-Z]+$",ErrorMessage ="用户名只能是数字或字母")]
        public string UserName { get; set; }

        [Required]
        [StringLength(12, ErrorMessage = "{0} 必须是至少 {2} 位至多 {1} 位.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "两次输入的密码不一致，请重新输入！")]
        public string ConfirmPassword { get; set; }
    }
    
}