using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class AssemblyChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleModels_ModelId",
                table: "Vehicles");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleModels_ModelId",
                table: "Vehicles",
                column: "ModelId",
                principalTable: "VehicleModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_VehicleModels_ModelId",
                table: "Vehicles");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_VehicleModels_ModelId",
                table: "Vehicles",
                column: "ModelId",
                principalTable: "VehicleModels",
                principalColumn: "Id");
        }
    }
}
