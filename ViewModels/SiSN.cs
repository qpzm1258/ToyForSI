using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ToyForSI.ViewModels
{
    public class SiSN
    {
        [Display(Name="设备编号")]
        [Required(ErrorMessage="设备编号不能为空")]
        [RegularExpression(@"[0-9]{12}$",ErrorMessage ="设备SN格式错误，请输入12位数字")]
        [Remote(action: "CheckSN", controller: "DeviceFlow",ErrorMessage ="sn无效，该设备不存在")]
        public string siSN { get; set; }
    }
}
