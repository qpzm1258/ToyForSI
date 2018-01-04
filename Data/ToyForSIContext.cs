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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("ToyForSI.Models.Brand", b =>
                {
                    b.Property<int>("brandId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("brandEmail");

                    b.Property<string>("brandLogo");

                    b.Property<string>("brandName")
                        .IsRequired();

                    b.Property<string>("brandServiceHotLine");

                    b.Property<string>("brandUrl");

                    b.HasKey("brandId");

                    b.ToTable("Brand");
                });

            modelBuilder.Entity("ToyForSI.Models.Department", b =>
                {
                    b.Property<int>("departmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("departmentName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.HasKey("departmentId");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("ToyForSI.Models.DepartmentAttributes", b =>
                {
                    b.Property<int>("departmentAttributeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("departmentAttributeName")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.Property<ToyForSI.Models.Enum.ValueType>("departmentAttributeType");

                    b.Property<ToyForSI.Models.Enum.Bool>("isRequired");

                    b.Property<int?>("valueLength");

                    b.Property<string>("valueRegEx");

                    b.HasKey("departmentAttributeId");

                    b.ToTable("DepartmentAttributes");
                });

            modelBuilder.Entity("ToyForSI.Models.DepartmentValue", b =>
                {
                    b.Property<int>("departmentValueId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("departmentAttributeId");

                    b.Property<int>("departmentId");

                    b.Property<string>("departmentValue");

                    b.HasKey("departmentValueId");

                    b.HasIndex("departmentAttributeId");

                    b.HasIndex("departmentId");

                    b.ToTable("DepartmentValue");
                });

            modelBuilder.Entity("ToyForSI.Models.Device", b =>
                {
                    b.Property<int>("deviceId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("contractNo")
                        .IsRequired();

                    b.Property<DateTime>("createTime");

                    b.Property<int?>("devModelId")
                        .IsRequired();

                    b.Property<ToyForSI.Models.Enum.Bool>("inWareHouse");

                    b.HasKey("deviceId");

                    b.HasIndex("devModelId");

                    b.ToTable("Device");
                });

            modelBuilder.Entity("ToyForSI.Models.DeviceFlowHistory", b =>
                {
                    b.Property<int>("deviceFlowHistoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("deviceId");

                    b.Property<ToyForSI.Models.Enum.DeviceStatus>("deviceStatus");

                    b.Property<DateTime>("flowDateTime");

                    b.Property<int?>("fromDepartmentId");

                    b.Property<string>("fromLocation");

                    b.Property<int?>("fromMemberId");

                    b.Property<int?>("toDepartmentId");

                    b.Property<string>("toLocation");

                    b.Property<int?>("toMemberId");

                    b.HasKey("deviceFlowHistoryId");

                    b.HasIndex("deviceId");

                    b.HasIndex("fromDepartmentId");

                    b.HasIndex("fromMemberId");

                    b.HasIndex("toDepartmentId");

                    b.HasIndex("toMemberId");

                    b.ToTable("DeviceFlowHistory");
                });

            modelBuilder.Entity("ToyForSI.Models.DevModel", b =>
                {
                    b.Property<int>("devModelId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("brandId")
                        .IsRequired();

                    b.Property<string>("devModelName")
                        .IsRequired();

                    b.Property<int?>("equipmentTypeId")
                        .IsRequired();

                    b.HasKey("devModelId");

                    b.HasIndex("brandId");

                    b.HasIndex("equipmentTypeId");

                    b.ToTable("DevModel");
                });

            modelBuilder.Entity("ToyForSI.Models.EquipmentType", b =>
                {
                    b.Property<int>("equipmentTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("equipmentTypeName")
                        .IsRequired();

                    b.Property<string>("equipmentTypeRemarks");

                    b.HasKey("equipmentTypeId");

                    b.ToTable("EquipmentType");
                });

            modelBuilder.Entity("ToyForSI.Models.Member", b =>
                {
                    b.Property<int>("memberId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("IDCard");

                    b.Property<DateTime>("createTime");

                    b.Property<int?>("departmentId");

                    b.Property<string>("employeeId")
                        .IsRequired()
                        .HasMaxLength(6);

                    b.Property<string>("name")
                        .IsRequired();

                    b.Property<int?>("positionId");

                    b.Property<ToyForSI.Models.Enum.Sex>("sex");

                    b.HasKey("memberId");

                    b.HasIndex("departmentId");

                    b.HasIndex("positionId");

                    b.ToTable("Member");
                });

            modelBuilder.Entity("ToyForSI.Models.Position", b =>
                {
                    b.Property<int>("positionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("positionAbstract");

                    b.Property<string>("positionName");

                    b.HasKey("positionId");

                    b.ToTable("Position");
                });

            modelBuilder.Entity("ToyForSI.Models.User", b =>
                {
                    b.Property<int>("userId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("isActive");

                    b.Property<string>("userName")
                        .IsRequired();

                    b.Property<string>("userPassword")
                        .IsRequired()
                        .HasMaxLength(12);

                    b.HasKey("userId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ToyForSI.Models.DepartmentValue", b =>
                {
                    b.HasOne("ToyForSI.Models.DepartmentAttributes", "departmentAttributes")
                        .WithMany("departmentValues")
                        .HasForeignKey("departmentAttributeId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ToyForSI.Models.Department", "department")
                        .WithMany("departmentValues")
                        .HasForeignKey("departmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ToyForSI.Models.Device", b =>
                {
                    b.HasOne("ToyForSI.Models.DevModel", "devModel")
                        .WithMany("devices")
                        .HasForeignKey("devModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

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

            modelBuilder.Entity("ToyForSI.Models.DevModel", b =>
                {
                    b.HasOne("ToyForSI.Models.Brand", "brand")
                        .WithMany("devModels")
                        .HasForeignKey("brandId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("ToyForSI.Models.EquipmentType", "equipmentType")
                        .WithMany("devModels")
                        .HasForeignKey("equipmentTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
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
#pragma warning restore 612, 618
        }
    }
    
}