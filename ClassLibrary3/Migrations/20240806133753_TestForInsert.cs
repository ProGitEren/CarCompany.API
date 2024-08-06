using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class TestForInsert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Vin",
                table: "Vehicles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(17)",
                oldFixedLength: true,
                oldMaxLength: 17);

            migrationBuilder.AlterColumn<string>(
                name: "VehicleId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nchar(17)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Vin",
                table: "Vehicles",
                type: "nchar(17)",
                fixedLength: true,
                maxLength: 17,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleId",
                table: "AspNetUsers",
                type: "nchar(17)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
