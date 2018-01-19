using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ToyForSI.ViewModels
{
    public class Login
    {
        [Required(ErrorMessage="请输入用户名！")]
        [Display(Name = "用户名")]
        [StringLength(20, ErrorMessage = "{0} 必须是至少 {2} 位至多 {1} 位数字或字母.", MinimumLength = 4)]
        [RegularExpression(@"[0-9a-zA-Z]+$", ErrorMessage = "用户名只能是数字或字母")]
        public string UserName { get; set; }

        [Required(ErrorMessage="请输入密码！")]
        [StringLength(12, ErrorMessage = "{0} 必须是至少 {2} 位至多 {1} 位.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }

}