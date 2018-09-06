using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ToyForSI.Models.Enum;
using System.Collections.Generic;
using ToyForSI.Data;
using System.Linq;

namespace ToyForSI.Models
{
    public class Device
    {
        public int deviceId{get;set;}
        [Display(Name="合同编号",Prompt="如：[2017]12A3456")]
        [Required(ErrorMessage="合同编号不能为空")]
        public string contractNo{get;set;}
        [Display(Name="设备型号")]
        public DevModel devModel{get;set;}
        [Display(Name="设备型号")]
        [Required(ErrorMessage="设备型号不能为空")]
        public int? devModelId{get;set;}
        [Required(ErrorMessage="入库时间不能为空")]
        [Display(Name="入库时间")]
        [DataType(DataType.DateTime)]
        public DateTime createTime{get;set;}
        [Display(Name="网卡信息")]
        public IEnumerable<NetworkAdepter> networkAdepters{get;set;}
        [Display(Name ="局内编号")]
        
        public String siSN
        {
            get
            {
                String sn;
                if(devModel==null)
                {
                    return String.Empty;
                }
                sn = devModel.brandId.ToString().PadLeft(3, '0')
                    + devModel.equipmentTypeId.ToString().PadLeft(3, '0')
                    + deviceId.ToString().PadLeft(6, '0');
                return sn;
            }
        }
        [Display(Name ="是否在库")]
        public ToyForSI.Models.Enum.Bool inWareHouse { get; set; }
        public IEnumerable<DeviceFlowHistory> historys{get;set;}

        [Display(Name="设备概要")]
        public string DeviceSummary{
            get
            {
                string deviceSummary=string.Empty;
                if(this.devModel!=null)
                {
                    if(this.devModel.brand!=null)
                    {
                        deviceSummary+=this.devModel.brand.brandName;
                    }
                    deviceSummary+=this.devModel.devModelName;
                    if(this.devModel.equipmentType!=null)
                    {
                        deviceSummary+=this.devModel.equipmentType.equipmentTypeName;
                    } 
                }
                return deviceSummary;
            }
        }

        public DeviceFlowHistory LastHistory
        {
            get
            {
                DeviceFlowHistory lastHistory=null;
                if(historys!=null)
                {
                    lastHistory=historys.OrderBy(h=>h.deviceFlowHistoryId).Last();
                }
                return lastHistory;
            }
        }

        [Display(Name="使用者")]
        public string User
        {
            get
            {
                string user=string.Empty;
                if(LastHistory!=null)
                {
                    if(LastHistory.toMember!=null)
                    {
                        user=LastHistory.toMember.name;
                    }
                }
                return user;
            }
        }
        [Display(Name="归属部门")]
        public string UserDepartment
        {
            get
            {
                string userDepartment=string.Empty;
                if(LastHistory!=null)
                {
                    if(LastHistory.toDepartment!=null)
                    {
                        userDepartment=LastHistory.toDepartment.departmentName;
                    }
                }
                return userDepartment;
            }

        }
    }
}