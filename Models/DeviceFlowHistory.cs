using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
using ToyForSI.Models.Enum;
using System.Reflection;

namespace ToyForSI.Models
{
    //[AtLeastOneProperty("toLocation", "toDepartmentId","toMemberId", ErrorMessage="“新所在地”、“接收科室”、“接收者”至少一个不能为空！")]
    public class DeviceFlowHistory
    {
        public int deviceFlowHistoryId{get;set;}
        [Display(Name="设备")]
        public int deviceId{get;set;}
        [Display(Name="设备")]
        public Device device{get;set;}
        [Display(Name="当前所在地点")]
        public string fromLocation{get;set;}
        [Display(Name="新所在地点",Prompt="请输入新的所在地")]
        public string toLocation{get;set;}
        [ForeignKey("formdepartment")]
        [Display(Name="当前科室")]
        public int? fromDepartmentId{get;set;}
        [Display(Name="当前科室")]
        public Department formdepartment{get;set;}
        [ForeignKey("todepartment")]
        [Display(Name="接收科室")]
        public int? toDepartmentId{get;set;}
        [Display(Name="接收科室")]
        public Department toDepartment{get;set;}
        [ForeignKey("fromMember")]
        [Display(Name="当前使用者")]
        public int? fromMemberId{get;set;}
        [Display(Name="当前使用者")]
        public Member fromMember{get;set;}
        [ForeignKey("toMember")]
        [Display(Name="接收者")]
        public int? toMemberId{get;set;}
        [Display(Name="接收者")]
        public Member toMember{get;set;}
        [Display(Name="交接时间")]
        [DataType(DataType.DateTime)]
        public DateTime flowDateTime{get;set;}
        [Display(Name="交接状态")]
        [Required(ErrorMessage="设备状态不能为空！")]
        public DeviceStatus deviceStatus{get;set;}

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        public class AtLeastOnePropertyAttribute : ValidationAttribute
        {
            private string[] PropertyList { get; set; }

            public AtLeastOnePropertyAttribute(params string[] propertyList)
            {
                this.PropertyList = propertyList;
            }

            //See http://stackoverflow.com/a/1365669
            public override object TypeId
            {
                get
                {
                    return this;
                }
            }

            public override bool IsValid(object value)
            {
                PropertyInfo propertyInfo;
                foreach (string propertyName in PropertyList)
                {
                    propertyInfo =value.GetType().GetProperty(propertyName);

                    if (propertyInfo != null && propertyInfo.GetValue(value, null) != null)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
    }
}