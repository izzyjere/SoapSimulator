using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoapSimulator.Core.Migrations
{
    public partial class UpdateStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "XSDPath",
                table: "ResponseFormats",
                newName: "XMLPath");

            migrationBuilder.RenameColumn(
                name: "XSDPath",
                table: "RequestFormats",
                newName: "XMLPath");

            migrationBuilder.CreateIndex(
                name: "IX_SoapActions_MethodName",
                table: "SoapActions",
                column: "MethodName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SoapActions_MethodName",
                table: "SoapActions");

            migrationBuilder.RenameColumn(
                name: "XMLPath",
                table: "ResponseFormats",
                newName: "XSDPath");

            migrationBuilder.RenameColumn(
                name: "XMLPath",
                table: "RequestFormats",
                newName: "XSDPath");
        }
    }
}
