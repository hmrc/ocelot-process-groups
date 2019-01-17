using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace ProductGrouping.Migrations
{
    public partial class inital : Migration
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void Up(MigrationBuilder migrationBuilder)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            migrationBuilder.CreateTable(
                name: "ProductGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductReference = table.Column<string>(maxLength: 10, nullable: false),
                    ProductOwner = table.Column<string>(maxLength: 7, nullable: false),
                    Group = table.Column<string>(maxLength: 100, nullable: false),
                    Site = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGroups", x => x.Id);
                });
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void Down(MigrationBuilder migrationBuilder)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            migrationBuilder.DropTable(
                name: "ProductGroups");
        }
    }
}
