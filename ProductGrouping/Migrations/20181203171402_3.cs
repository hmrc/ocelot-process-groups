using Microsoft.EntityFrameworkCore.Migrations;

namespace ProductGrouping.Migrations
{
    public partial class _3 : Migration
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void Up(MigrationBuilder migrationBuilder)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        protected override void Down(MigrationBuilder migrationBuilder)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
