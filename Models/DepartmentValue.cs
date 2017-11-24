using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ToyForSI.Models
{
    public class DepartmentValue
    {
        [Key]
        public int departmentValueId{get;set;}
        public int departmentId{get;set;}
        public Department department{get;set;}
        public int departmentAttributeId{get;set;}
        public DepartmentAttributes departmentAttributes{get;set;}
        public string  departmentValue{get;set;}
    }
}