using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToyForSI.Models;
using ToyForSI.Models.Enum;

namespace ToyForSI.Data
{
    public class ToyForSIContext : IdentityDbContext<ApplicationUser>
    {
        public ToyForSIContext (DbContextOptions<ToyForSIContext> options)
            : base(options)
        {
        }

        //public DbSet<ToyForSI.Models.User> User { get; set; }
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
        public DbSet<ToyForSI.Models.ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ToyForSI.Models.NetworkAdepter> NetworkAdepter { get; set; }
        public DbSet<ToyForSI.Models.PersonnelTransferHistory> PersonnelTransferHistory {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity("ToyForSI.Models.DeviceFlowHistory", b =>
                {
                    b.HasOne("ToyForSI.Models.Device", "device")
                        .WithMany("historys")
                        .HasForeignKey("deviceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ToyForSI.Models.Department", "formdepartment")
                        .WithMany()
                        .HasForeignKey("fromDepartmentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ToyForSI.Models.Member", "fromMember")
                        .WithMany()
                        .HasForeignKey("fromMemberId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ToyForSI.Models.Department", "toDepartment")
                        .WithMany()
                        .HasForeignKey("toDepartmentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ToyForSI.Models.Member", "toMember")
                        .WithMany()
                        .HasForeignKey("toMemberId")
                        .OnDelete(DeleteBehavior.SetNull);
                });
            modelBuilder.Entity("ToyForSI.Models.Member", b =>
                {
                    b.HasOne("ToyForSI.Models.Department", "department")
                        .WithMany("members")
                        .HasForeignKey("departmentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ToyForSI.Models.Position", "position")
                        .WithMany("members")
                        .HasForeignKey("positionId")
                        .OnDelete(DeleteBehavior.SetNull);
                });
            modelBuilder.Entity<Position>().Property(b =>b.order).HasDefaultValue(999);
            modelBuilder.Entity("ToyForSI.Models.Department", b =>
            {
                b.HasOne("ToyForSI.Models.Member", "departmentManager")
                    .WithMany()
                    .HasForeignKey("departmentManagerId")
                    .OnDelete(DeleteBehavior.SetNull);

                b.HasOne("ToyForSI.Models.Member", "departmentLeader")
                    .WithMany()
                    .HasForeignKey("departmentLeaderId")
                    .OnDelete(DeleteBehavior.SetNull);
            });
            modelBuilder.Entity<Department>().Property(b =>b.order).HasDefaultValue(999);
        }
    }
}