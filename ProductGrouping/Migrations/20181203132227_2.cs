using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductGrouping.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "ProductGroups");

            migrationBuilder.DropColumn(
                name: "Site",
                table: "ProductGroups");

            migrationBuilder.AlterColumn<string>(
                name: "ProductReference",
                table: "ProductGroups",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "ProductGroups",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_ParentId",
                table: "ProductGroups",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGroups_ProductGroups_ParentId",
                table: "ProductGroups",
                column: "ParentId",
                principalTable: "ProductGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGroups_ProductGroups_ParentId",
                table: "ProductGroups");

            migrationBuilder.DropIndex(
                name: "IX_ProductGroups_ParentId",
                table: "ProductGroups");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ProductGroups");

            migrationBuilder.AlterColumn<string>(
                name: "ProductReference",
                table: "ProductGroups",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "ProductGroups",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Site",
                table: "ProductGroups",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
