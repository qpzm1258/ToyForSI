using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ToyForSI.Models.Enum;
using System.Collections.Generic;

namespace ToyForSI.Models
{
    public class NetworkAdepter
    {
        [Display(Name="网卡ID")]
        public int NetworkAdepterId{get;set;}
        [Display(Name="设备")]
        public Device device{get;set;}
        [Display(Name="设备ID")]
        [Required]
        public int deviceId{get;set;}
        [Display(Name="Mac地址")]
        [RegularExpression("([A-Fa-f0-9]{2}[-:]){5}[A-Fa-f0-9]{2}",ErrorMessage="mac地址格式错误")]
        [Required]
        public string MACAddress{get;set;}
        
    }
}