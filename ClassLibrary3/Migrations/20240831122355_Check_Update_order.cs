using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class Check_Update_order : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVehicleOwnerTransferred",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVehicleOwnerTransferred",
                table: "Orders");
        }
    }
}
