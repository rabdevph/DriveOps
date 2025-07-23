using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriveOps.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddJobOrderCustomerVehicleTechnicianNavigations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "JobOrders",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_CustomerId",
                table: "JobOrders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_TechnicianId",
                table: "JobOrders",
                column: "TechnicianId");

            migrationBuilder.CreateIndex(
                name: "IX_JobOrders_VehicleId",
                table: "JobOrders",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOrders_Customers_CustomerId",
                table: "JobOrders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOrders_Technicians_TechnicianId",
                table: "JobOrders",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOrders_Vehicles_VehicleId",
                table: "JobOrders",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOrders_Customers_CustomerId",
                table: "JobOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOrders_Technicians_TechnicianId",
                table: "JobOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOrders_Vehicles_VehicleId",
                table: "JobOrders");

            migrationBuilder.DropIndex(
                name: "IX_JobOrders_CustomerId",
                table: "JobOrders");

            migrationBuilder.DropIndex(
                name: "IX_JobOrders_TechnicianId",
                table: "JobOrders");

            migrationBuilder.DropIndex(
                name: "IX_JobOrders_VehicleId",
                table: "JobOrders");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "JobOrders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);
        }
    }
}
