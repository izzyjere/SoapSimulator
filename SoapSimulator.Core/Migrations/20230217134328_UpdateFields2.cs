using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoapSimulator.Core.Migrations
{
    public partial class UpdateFields2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Message",
                table: "ResponseFormats",
                newName: "XSDPath");

            migrationBuilder.AddColumn<string>(
                name: "XSDPath",
                table: "RequestFormats",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "XSDPath",
                table: "RequestFormats");

            migrationBuilder.RenameColumn(
                name: "XSDPath",
                table: "ResponseFormats",
                newName: "Message");
        }
    }
}
