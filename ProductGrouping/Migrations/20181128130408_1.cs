using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductGrouping.Migrations
{
    public partial class _1 : Migration
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void Up(MigrationBuilder migrationBuilder)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_ProductReference",
                table: "ProductGroups",
                column: "ProductReference",
                unique: true);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void Down(MigrationBuilder migrationBuilder)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductGroups_ProductReference",
                table: "ProductGroups");
        }
    }
}
