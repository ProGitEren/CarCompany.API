using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class CylinderAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cylinder",
                table: "Engines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1000,
                column: "Cylinder",
                value: "i4");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1001,
                column: "Cylinder",
                value: "v6");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1002,
                column: "Cylinder",
                value: "v8");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1003,
                column: "Cylinder",
                value: "i4");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1004,
                columns: new[] { "Cylinder", "EngineName" },
                values: new object[] { "i4", "Unknown Engine" });

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1005,
                column: "Cylinder",
                value: "v8");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1006,
                column: "Cylinder",
                value: "i4");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1007,
                column: "Cylinder",
                value: "v6");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1008,
                column: "Cylinder",
                value: "v6");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1009,
                column: "Cylinder",
                value: "v8");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1010,
                column: "Cylinder",
                value: "i4");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1011,
                column: "Cylinder",
                value: "v6");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1012,
                column: "Cylinder",
                value: "v6");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1013,
                column: "Cylinder",
                value: "v8");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1014,
                column: "Cylinder",
                value: "i4");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1015,
                column: "Cylinder",
                value: "v6");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1016,
                column: "Cylinder",
                value: "v6");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1017,
                column: "Cylinder",
                value: "v8");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1018,
                column: "Cylinder",
                value: "i4");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1019,
                column: "Cylinder",
                value: "v6");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1020,
                column: "Cylinder",
                value: "v6");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1021,
                column: "Cylinder",
                value: "v8");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1022,
                column: "Cylinder",
                value: "i4");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1023,
                column: "Cylinder",
                value: "v6");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1024,
                column: "Cylinder",
                value: "v6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cylinder",
                table: "Engines");

            migrationBuilder.UpdateData(
                table: "Engines",
                keyColumn: "Id",
                keyValue: 1004,
                column: "EngineName",
                value: "Economy 2.0L Inline 4 Engine");
        }
    }
}
