using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ToyForSI.Models.Enum;
using System.Collections.Generic;

namespace ToyForSI.Models
{
    public class MultiDev
    {
        public int deviceId { get; set; }
        [Display(Name = "合同编号", Prompt = "如：[2017]12A3456")]
        [Required(ErrorMessage = "合同编号不能为空")]
        public string contractNo { get; set; }
        [Display(Name = "设备型号")]
        public DevModel devModel { get; set; }
        [Display(Name = "设备型号")]
        [Required(ErrorMessage = "设备型号不能为空")]
        public int? devModelId { get; set; }
        [Required(ErrorMessage = "入库时间不能为空")]
        [Display(Name = "入库时间")]
        [DataType(DataType.DateTime)]
        public DateTime createTime { get; set; }
        [Display(Name ="数量")]
        [Required(ErrorMessage ="请正确输入数量")]
        [RegularExpression(@"[0-9]+$",ErrorMessage = "请正确输入数量")]
        public int devCount { get; set; }
        public string toLocation{get;set;}
        [Display(Name = "是否在库")]
        public ToyForSI.Models.Enum.Bool inWareHouse { get; set; }

    }
}
