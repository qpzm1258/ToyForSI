using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ToyForSI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    departmentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    departmentName = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.departmentId);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentAttributes",
                columns: table => new
                {
                    departmentAttributeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    departmentAttributeName = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
                    departmentAttributeType = table.Column<int>(type: "INTEGER", nullable: false),
                    isRequired = table.Column<int>(type: "INTEGER", nullable: false),
                    valueLength = table.Column<int>(type: "INTEGER", nullable: true),
                    valueRegEx = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentAttributes", x => x.departmentAttributeId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    userId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    isActive = table.Column<int>(type: "INTEGER", nullable: false),
                    userName = table.Column<string>(type: "TEXT", nullable: false),
                    userPassword = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "DepartmentValue",
                columns: table => new
                {
                    departmentValueId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    departmentAttributeId = table.Column<int>(type: "INTEGER", nullable: false),
                    departmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    departmentValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepartmentValue", x => x.departmentValueId);
                    table.ForeignKey(
                        name: "FK_DepartmentValue_DepartmentAttributes_departmentAttributeId",
                        column: x => x.departmentAttributeId,
                        principalTable: "DepartmentAttributes",
                        principalColumn: "departmentAttributeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DepartmentValue_Department_departmentId",
                        column: x => x.departmentId,
                        principalTable: "Department",
                        principalColumn: "departmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentValue_departmentAttributeId",
                table: "DepartmentValue",
                column: "departmentAttributeId");

            migrationBuilder.CreateIndex(
                name: "IX_DepartmentValue_departmentId",
                table: "DepartmentValue",
                column: "departmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepartmentValue");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "DepartmentAttributes");

            migrationBuilder.DropTable(
                name: "Department");
        }
    }
}
