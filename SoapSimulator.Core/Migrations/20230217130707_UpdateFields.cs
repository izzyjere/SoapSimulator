using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoapSimulator.Core.Migrations
{
    public partial class UpdateFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInUse",
                table: "RequestFormats");

            migrationBuilder.RenameColumn(
                name: "BodyFormatXML",
                table: "ResponseFormats",
                newName: "Body");

            migrationBuilder.RenameColumn(
                name: "BodyFormatXML",
                table: "RequestFormats",
                newName: "Body");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Body",
                table: "ResponseFormats",
                newName: "BodyFormatXML");

            migrationBuilder.RenameColumn(
                name: "Body",
                table: "RequestFormats",
                newName: "BodyFormatXML");

            migrationBuilder.AddColumn<bool>(
                name: "IsInUse",
                table: "RequestFormats",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
