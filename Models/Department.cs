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

        [Display(Name = "科室负责人")]
        public Member departmentManager { get; set; }
        [Display(Name = "科室负责人")]
        public int? departmentManagerId { get; set; }
        [Display(Name = "分管领导")]
        public Member departmentLeader { get; set; }
        [Display(Name = "分管领导")]
        public int? departmentLeaderId { get; set; }
        [Display(Name = "排序")]
        [Range(0,999,ErrorMessage="最大不超过999")]
        [RegularExpression("([0-9]+)", ErrorMessage = "只能输入整数")]
        public int? order{get;set;}

        public List<DepartmentValue> departmentValues{get;set;}
        public List<Member> members { get; set; }

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