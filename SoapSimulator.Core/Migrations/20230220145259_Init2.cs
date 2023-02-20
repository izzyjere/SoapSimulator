using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoapSimulator.Core.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ResponseFormats",
                table: "ResponseFormats");

            migrationBuilder.AddColumn<int>(
                name: "ActionStatus",
                table: "ResponseFormats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResponseFormats",
                table: "ResponseFormats",
                columns: new[] { "ActionId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ResponseFormats",
                table: "ResponseFormats");

            migrationBuilder.DropColumn(
                name: "ActionStatus",
                table: "ResponseFormats");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ResponseFormats",
                table: "ResponseFormats",
                column: "ActionId");
        }
    }
}
