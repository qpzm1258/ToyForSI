using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ToyForSI.Models.Enum;

namespace ToyForSI.Models
{
    public class MultiDev:Device
    {
        [Display(Name ="数量")]
        [Required(ErrorMessage ="请正确输入数量")]
        [RegularExpression(@"[0-9]+$",ErrorMessage = "请正确输入数量")]
        public int devCount { get; set; }
        public string toLocation{get;set;}
    }
}
