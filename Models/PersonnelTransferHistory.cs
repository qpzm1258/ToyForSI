using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using ToyForSI.Models.Enum;
using System.Reflection;

namespace ToyForSI.Models
{
    //[AtLeastOneProperty("toLocation", "toDepartmentId","toMemberId", ErrorMessage="“新所在地”、“接收科室”、“接收者”至少一个不能为空！")]
    public class PersonnelTransferHistory
    {
        public int personnelTransferHistoryId{get;set;}
        [Display(Name="姓名")]
        public int memberId{get;set;}
        [Display(Name="姓名")]
        public Member member{get;set;}
        [Display(Name="源科室")]
        public string formDepartment{get;set;}
        [Display(Name="新科室",Prompt="请输入新科室")]
        public string toDepartment{get;set;}

        [Display(Name="源职位")]
        public string formPosition{get;set;}
        [Display(Name="新职位",Prompt="请输入职位")]
        public string toPosition{get;set;}
        [Display(Name="变更原因")]
        public string reason{get;set;}

        [Display(Name="变更时间")]
        [DataType(DataType.DateTime)]
        public DateTime flowDateTime{get;set;}
    }
}