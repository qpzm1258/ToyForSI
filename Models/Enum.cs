using System;
using System.ComponentModel.DataAnnotations;

namespace ToyForSI.Models.Enum
{
    public enum ValueType
    {
        [Display(Name="整数")]
        TypeInteger=0,
        [Display(Name="分数")]
        TypeDecimal=1,
        [Display(Name="日期")]
        TypeDate=2,
        [Display(Name="文字")]
        TypeText=3
    };

    public enum Bool
    {
        [Display(Name="否")]
        TypeTrue=0,
        [Display(Name="是")]
        TypeFalsel=1
    };

    public enum Sex
    {
        [Display(Name="女")]
        Female=0,
        [Display(Name="男")]
        Male=1,
        [Display(Name="未知性别")]
        Unknow=2
    };
}