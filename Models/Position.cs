using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ToyForSI.Models
{
    public class Position
    {
        public int positionId { get; set; }
        [Display(Name ="职位名称",Prompt ="请输入职位名称")]
        [Remote(action: "VerifyName", controller: "Position", AdditionalFields="positionId")]
        public string positionName { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name ="职位简介",Prompt ="请输入职位简介")]
        public string positionAbstract { get; set; }
    }
}
