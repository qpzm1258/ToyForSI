using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToyForSI.Models;
using ToyForSI.Models.Enum;

namespace ToyForSI.Data
{
    public class ToyForSIContext : DbContext
    {
        public ToyForSIContext (DbContextOptions<ToyForSIContext> options)
            : base(options)
        {
        }

        public DbSet<ToyForSI.Models.User> User { get; set; }
        public DbSet<ToyForSI.Models.Department> Department { get; set; }
        public DbSet<ToyForSI.Models.DepartmentAttributes> DepartmentAttributes { get; set; }
        public DbSet<ToyForSI.Models.DepartmentValue> DepartmentValue { get; set; }
        public DbSet<ToyForSI.Models.Position> Position { get; set; }
        public DbSet<ToyForSI.Models.Member> Member { get; set; }
        public DbSet<ToyForSI.Models.Brand> Brand { get; set; }
        public DbSet<ToyForSI.Models.EquipmentType> EquipmentType { get; set; }
        public DbSet<ToyForSI.Models.DevModel> DevModel { get; set; }
        public DbSet<ToyForSI.Models.Device> Device { get; set; }
        public DbSet<ToyForSI.Models.DeviceFlowHistory> DeviceFlowHistory { get; set; }
    }
}