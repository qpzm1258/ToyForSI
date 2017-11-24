using System;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ToyForSI.Models.Enum;
using System.Collections.Generic;

namespace ToyForSI.Models
{
    public class DepartmentAttributes
    {
        [Key]
        public int departmentAttributeId{get;set;}

        [Required(ErrorMessage="部门属性名不能为空")]
        [Display(Name="部门属性名")]
        [StringLength(25,ErrorMessage="{0}长度不能超过25字节")]
        [Remote(action:"VerifyName",controller:"DepartmentAttributes", AdditionalFields="departmentAttributeId")]
        public string departmentAttributeName{get;set;}

        [Required(ErrorMessage="属性类型不能为空")]
        [Display(Name="部门属性类型")]
        [EnumDataType(typeof(ToyForSI.Models.Enum.ValueType))]
        public ToyForSI.Models.Enum.ValueType departmentAttributeType{get;set;}

        [Display(Name="数据长度")]
        [Range(0,25,ErrorMessage="长度不能超过15，0为不限制长度")]
        [RegularExpression("([0-9]+)", ErrorMessage = "只能输入整数")]
        public int? valueLength{get;set;}

        [Display(Name="是否必填")]
        [EnumDataType(typeof(ToyForSI.Models.Enum.Bool))]
        public ToyForSI.Models.Enum.Bool isRequired{get;set;}

        [Display(Name="正则表达式")]
        public string valueRegEx{get;set;}
        public ICollection<DepartmentValue> departmentValues{get;set;}
    }
}