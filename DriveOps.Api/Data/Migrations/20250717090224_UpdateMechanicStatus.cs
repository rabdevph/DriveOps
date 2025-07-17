using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriveOps.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMechanicStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Mechanics");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Mechanics",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Mechanics");

            migrationBuilder.AddColumn<string>(
                name: "IsActive",
                table: "Mechanics",
                type: "character varying(1)",
                nullable: false,
                defaultValue: "");
        }
    }
}
