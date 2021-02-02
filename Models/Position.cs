using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
namespace ToyForSI.Models
{
    public class Position
    {
        public int positionId { get; set; }
        [Display(Name ="职位名称",Prompt ="请输入职位名称")]
        [Remote(action: "VerifyName", controller: "Position", AdditionalFields="positionId")]
        public string positionName { get; set; }
        [Display(Name = "排序")]
        [Range(0,999,ErrorMessage="最大不超过999")]
        [RegularExpression("([0-9]+)", ErrorMessage = "只能输入整数")]
        public int? order{get;set;}

        [DataType(DataType.MultilineText)]
        [Display(Name ="职位简介",Prompt ="请输入职位简介")]
        
        public string positionAbstract { get; set; }
        public List<Member> members{get;set;}
    }
}
