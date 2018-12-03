using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductGrouping.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductGroups_ProductReference",
                table: "ProductGroups",
                column: "ProductReference",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductGroups_ProductReference",
                table: "ProductGroups");
        }
    }
}
