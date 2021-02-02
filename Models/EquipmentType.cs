using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToyForSI.Models.Enum;

namespace ToyForSI.Models
{
    public class EquipmentType
    {
        public int equipmentTypeId{get;set;}
        [Display(Name="设备类型",Prompt="请输入设备类型名")]
        [Required(ErrorMessage="设备类型不能为空")]
        public string equipmentTypeName{get;set;}

        [Display(Name="使用年限",Prompt="请输入设备使用年限")]
        [Required(ErrorMessage="使用年限不能为空")]
        [RegularExpression(@"[0-9]+$",ErrorMessage = "请正确输入使用年限")]
        public int life{get;set;}
        [Display(Name="设备类型备注",Prompt="请输入设备类型备注")]
        public string equipmentTypeRemarks{get;set;}

        public IEnumerable<DevModel> devModels{get;set;}
    }
}