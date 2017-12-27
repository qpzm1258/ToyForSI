using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ToyForSI.Models
{
    public class Department
    {
        public int departmentId{get;set;}
        [Required(ErrorMessage="请输入科室名")]
        [Display(Name = "科室名")]
        [StringLength(25)]
        [Remote(action: "VerifyName", controller: "Department", AdditionalFields="departmentId")]
        public string departmentName {get;set;}

        public ICollection<DepartmentValue> departmentValues{get;set;}
        public ICollection<Member> members { get; set; }

        public int memberCount
        {
            get
            {
                if(members!=null)
                {
                    return members.Count;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}