using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ToyForSI.Models.Enum;

namespace ToyForSI.Models
{
    public class Device
    {
        public int deviceId{get;set;}
        [Display(Name="合同编号",Prompt="如：[2017]12A3456")]
        [Required(ErrorMessage="合同编号不能为空")]
        public string contractNo{get;set;}
        [Display(Name="设备型号")]
        public DevModel devModel{get;set;}
        [Display(Name="设备型号")]
        [Required(ErrorMessage="设备型号不能为空")]
        public int? devModelId{get;set;}
        [Required(ErrorMessage="入库时间不能为空")]
        [Display(Name="入库时间")]
        [DataType(DataType.DateTime)]
        public DateTime createTime{get;set;}
        [Display(Name ="局内编号")]
        
        public String siSN
        {
            get
            {
                String sn;
                if(devModel==null)
                {
                    return String.Empty;
                }
                sn = devModel.brandId.ToString().PadLeft(3, '0')
                    + devModel.equipmentTypeId.ToString().PadLeft(3, '0')
                    + deviceId.ToString().PadLeft(6, '0');
                return sn;
            }
        }
        [Display(Name ="是否在库")]
        public ToyForSI.Models.Enum.Bool inWareHouse { get; set; }
    }
}