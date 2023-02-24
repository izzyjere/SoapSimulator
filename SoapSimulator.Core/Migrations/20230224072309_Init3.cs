using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoapSimulator.Core.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ValidateRequestBody",
                table: "SystemConfigurations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidateRequestBody",
                table: "SystemConfigurations");
        }
    }
}
