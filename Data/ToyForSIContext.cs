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
    }
}