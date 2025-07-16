using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriveOps.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnershipTrackingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "OwnershipEndDate",
                table: "VehicleOwnerships",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "OwnershipStartDate",
                table: "VehicleOwnerships",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisteredAt",
                table: "VehicleOwnerships",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnershipEndDate",
                table: "VehicleOwnerships");

            migrationBuilder.DropColumn(
                name: "OwnershipStartDate",
                table: "VehicleOwnerships");

            migrationBuilder.DropColumn(
                name: "RegisteredAt",
                table: "VehicleOwnerships");
        }
    }
}
