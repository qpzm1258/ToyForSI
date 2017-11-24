﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using ToyForSI.Data;
using ToyForSI.Models.Enum;

namespace ToyForSI.Migrations
{
    [DbContext(typeof(ToyForSIContext))]
    partial class ToyForSIContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

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

                    b.Property<int>("departmentAttributeType");

                    b.Property<int>("isRequired");

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
#pragma warning restore 612, 618
        }
    }
}
