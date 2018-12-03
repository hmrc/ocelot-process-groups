using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductGrouping.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductOwner",
                table: "ProductGroups",
                unicode: false,
                fixedLength: true,
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 7);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductOwner",
                table: "ProductGroups",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldFixedLength: true,
                oldMaxLength: 7);
        }
    }
}
