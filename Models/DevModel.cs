using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ToyForSI.Models.Enum;
using System.Collections.Generic;
using System.Linq;

namespace ToyForSI.Models
{
    public class DevModel
    {
        public int devModelId{get;set;}
        [Display(Name="型号",Prompt="请输入设备型号")]
        [Required(ErrorMessage="设备型号不能为空")]
        public string devModelName{get;set;}
        [Display(Name="品牌")]
        public Brand brand{get;set;}
        [Display(Name="品牌")]
        [Required(ErrorMessage="品牌不能为空")]
        public int? brandId{get;set;}
        [Display(Name="设备类型")]
        public EquipmentType equipmentType{get;set;}
        [Display(Name="设备类型")]
        [Required(ErrorMessage="设备类型不能为空")]
        public int? equipmentTypeId{get;set;}

        public IEnumerable<Device> devices{get;set;}

         public int devCount
        {
            get
            {
                if (devices!=null)
                {
                    return devices.ToList().Count;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}