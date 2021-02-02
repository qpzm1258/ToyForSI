using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ToyForSI.Models.Enum;
using System.Collections.Generic;

namespace ToyForSI.Models
{
    public class Member
    {
        public int memberId{get;set;}
        [Display(Name="姓名",Prompt="请输入姓名")]
        [Required(ErrorMessage="姓名不能为空")]
        public string name{get;set;}
        [Display(Name="性别",Prompt="请选择性别")]
        [EnumDataType(typeof(ToyForSI.Models.Enum.Sex))]
        [Required(ErrorMessage ="性别不能为空")]
        public ToyForSI.Models.Enum.Sex sex{get;set;}
        [Display(Name="工号",Prompt="请输入工号")]
        [Required(ErrorMessage ="工号不能为空")]
        [RegularExpression(@"[a-zA-Z0-9]{6}$",ErrorMessage ="请输入正确的工号，工号由数字或字母组成")]
        [StringLength(6)]
        public string employeeId{get;set;}
        [Display(Name ="固定电话")]
        [RegularExpression("([0-9/]+)", ErrorMessage = "只能输入数字或\"/\"")]
        [MaxLength(20)]
        public string tel{get;set;}
        [Display(Name ="手机")]
        [RegularExpression("([0-9/]+)", ErrorMessage = "只能输入数字或\"/\"")]
        [MaxLength(20)]
        public string mobile{get;set;}
        [Display(Name ="部门")]
        public int? departmentId { get; set; }
        [Display(Name="部门")]
        public Department department { get; set; }
        [Display(Name ="职位")]
        public int? positionId { get; set; }
        [Display(Name ="职位")]
        public Position position { get; set; }
        [Display(Name ="身份证号",Prompt ="请输入身份证号")]
        [RegularExpression(@"[0-9]{17}[0-9Xx]{1}$",ErrorMessage ="请正确输入身份证号码")]
        public string IDCard { get; set; }
        [Display(Name="创建时间")]
        public DateTime createTime { get; set; }
        public IEnumerable<PersonnelTransferHistory> historys{get;set;}
    }
}