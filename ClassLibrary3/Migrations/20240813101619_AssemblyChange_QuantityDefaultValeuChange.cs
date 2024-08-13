using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class AssemblyChange_QuantityDefaultValeuChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "VehicleModels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "EngineName",
                table: "Engines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1000,
                column: "EngineName",
                value: "Standard 2.0L Inline 4 Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1001,
                column: "EngineName",
                value: "High Output 3.5L V6 Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1002,
                column: "EngineName",
                value: "Performance 4.0L V8 Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1003,
                column: "EngineName",
                value: "Economy 1.6L Inline 4 Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1004,
                column: "EngineName",
                value: "Economy 2.0L Inline 4 Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1005,
                column: "EngineName",
                value: "Ford V8 Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1006,
                column: "EngineName",
                value: "Ford Electric Motor");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1007,
                column: "EngineName",
                value: "Ford Hybrid Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1008,
                column: "EngineName",
                value: "Ford V6 Turbo Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1009,
                column: "EngineName",
                value: "Toyota V8 Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1010,
                column: "EngineName",
                value: "Toyota Electric Motor");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1011,
                column: "EngineName",
                value: "Toyota Hybrid Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1012,
                column: "EngineName",
                value: "Toyota V6 Turbo Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1013,
                column: "EngineName",
                value: "BMW V8 Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1014,
                column: "EngineName",
                value: "BMW Electric Motor");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1015,
                column: "EngineName",
                value: "BMW Hybrid Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1016,
                column: "EngineName",
                value: "BMW V6 Turbo Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1017,
                column: "EngineName",
                value: "Honda V8 Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1018,
                column: "EngineName",
                value: "Honda Electric Motor");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1019,
                column: "EngineName",
                value: "Honda Hybrid Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1020,
                column: "EngineName",
                value: "Honda V6 Turbo Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1021,
                column: "EngineName",
                value: "Mercedes V8 Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1022,
                column: "EngineName",
                value: "Mercedes Electric Motor");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1023,
                column: "EngineName",
                value: "Mercedes Hybrid Engine");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1024,
                column: "EngineName",
                value: "Mercedes V6 Turbo Engine");

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 1000,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 1001,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 1002,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 1003,
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 1004,
                column: "Quantity",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EngineName",
                table: "Engines");

            migrationBuilder.AlterColumn<int>(
                name: "Quantity",
                table: "VehicleModels",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 1000,
                column: "Quantity",
                value: 1);

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 1001,
                column: "Quantity",
                value: 1);

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 1002,
                column: "Quantity",
                value: 1);

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 1003,
                column: "Quantity",
                value: 1);

            migrationBuilder.UpdateData(
                table: "VehicleModels",
                keyColumn: "Id",
                keyValue: 1004,
                column: "Quantity",
                value: 1);
        }
    }
}
