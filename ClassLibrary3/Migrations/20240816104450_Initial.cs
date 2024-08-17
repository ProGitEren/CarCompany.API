using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastucture.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    state = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    zipcode = table.Column<int>(type: "int", nullable: false),
                    country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.AddressId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Engines",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    Cylinder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EngineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Volume = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Hp = table.Column<int>(type: "int", nullable: false),
                    CompressionRatio = table.Column<int>(type: "int", nullable: false),
                    Torque = table.Column<int>(type: "int", nullable: false),
                    diameterCm = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    EngineCode = table.Column<string>(type: "nchar(5)", maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Engines", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1000, 1"),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EngineCode = table.Column<string>(type: "nchar(5)", maxLength: 5, nullable: false),
                    ModelShortName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ModelLongName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    ModelYear = table.Column<int>(type: "int", nullable: false),
                    ModelPicture = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ManufacturedCountry = table.Column<int>(type: "int", nullable: false),
                    Manufacturer = table.Column<string>(type: "nchar(2)", nullable: false),
                    ManufacturedYear = table.Column<string>(type: "nchar(1)", nullable: false),
                    ManufacturedPlant = table.Column<string>(type: "nchar(1)", nullable: false),
                    CheckDigit = table.Column<string>(type: "nchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    birthtime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    roles = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "AddressId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Vin = table.Column<string>(type: "nchar(17)", nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EngineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Averagefuelin = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Averagefuelout = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    COemmission = table.Column<int>(type: "int", nullable: false),
                    FuelCapacity = table.Column<int>(type: "int", nullable: false),
                    MaxAllowedWeight = table.Column<int>(type: "int", nullable: false),
                    MinWeight = table.Column<int>(type: "int", nullable: false),
                    BaggageVolume = table.Column<int>(type: "int", nullable: false),
                    DrivenKM = table.Column<int>(type: "int", nullable: false),
                    ModelId = table.Column<int>(type: "int", nullable: false),
                    EngineId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Vin);
                    table.ForeignKey(
                        name: "FK_Vehicles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Vehicles_Engines_EngineId",
                        column: x => x.EngineId,
                        principalTable: "Engines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleModels_ModelId",
                        column: x => x.ModelId,
                        principalTable: "VehicleModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Engines",
                columns: new[] { "Id", "CompressionRatio", "Cylinder", "EngineCode", "EngineName", "Hp", "Torque", "Volume", "diameterCm" },
                values: new object[,]
                {
                    { 1000, 10, "i4", "EN123", "Standard 2.0L Inline 4 Engine", 150, 200, 2.0m, 8.5m },
                    { 1001, 11, "v6", "EN456", "High Output 3.5L V6 Engine", 250, 350, 3.5m, 10.0m },
                    { 1002, 12, "v8", "EN789", "Performance 4.0L V8 Engine", 300, 400, 4.0m, 11.0m },
                    { 1003, 9, "i4", "EN101", "Economy 1.6L Inline 4 Engine", 120, 180, 1.6m, 7.5m },
                    { 1004, 10, "i4", "EN202", "Unknown Engine", 200, 250, 2.5m, 9.0m },
                    { 1005, 12, "v8", "F01V8", "Ford V8 Engine", 450, 500, 5.0m, 12.0m },
                    { 1006, 0, "i4", "F02EM", "Ford Electric Motor", 200, 300, 0.0m, 0.0m },
                    { 1007, 11, "v6", "F03HY", "Ford Hybrid Engine", 350, 450, 2.5m, 9.5m },
                    { 1008, 10, "v6", "F04V6", "Ford V6 Turbo Engine", 350, 400, 3.0m, 10.5m },
                    { 1009, 11, "v8", "T01V8", "Toyota V8 Engine", 400, 450, 4.5m, 11.5m },
                    { 1010, 0, "i4", "T02EM", "Toyota Electric Motor", 250, 350, 0.0m, 0.0m },
                    { 1011, 10, "v6", "T03HY", "Toyota Hybrid Engine", 300, 400, 2.5m, 9.5m },
                    { 1012, 10, "v6", "T04V6", "Toyota V6 Turbo Engine", 325, 375, 3.0m, 10.5m },
                    { 1013, 12, "v8", "B01V8", "BMW V8 Engine", 450, 500, 4.5m, 12.0m },
                    { 1014, 0, "i4", "B02EM", "BMW Electric Motor", 250, 350, 0.0m, 0.0m },
                    { 1015, 11, "v6", "B03HY", "BMW Hybrid Engine", 300, 400, 2.5m, 9.5m },
                    { 1016, 10, "v6", "B04V6", "BMW V6 Turbo Engine", 325, 375, 3.0m, 10.5m },
                    { 1017, 12, "v8", "H01V8", "Honda V8 Engine", 450, 500, 4.5m, 12.0m },
                    { 1018, 0, "i4", "H02EM", "Honda Electric Motor", 250, 350, 0.0m, 0.0m },
                    { 1019, 11, "v6", "H03HY", "Honda Hybrid Engine", 300, 400, 2.5m, 9.5m },
                    { 1020, 10, "v6", "H04V6", "Honda V6 Turbo Engine", 325, 375, 3.0m, 10.5m },
                    { 1021, 12, "v8", "M01V8", "Mercedes V8 Engine", 450, 500, 4.5m, 12.0m },
                    { 1022, 0, "i4", "M02EM", "Mercedes Electric Motor", 250, 350, 0.0m, 0.0m },
                    { 1023, 11, "v6", "M03HY", "Mercedes Hybrid Engine", 300, 400, 2.5m, 9.5m },
                    { 1024, 10, "v6", "M04V6", "Mercedes V6 Turbo Engine", 325, 375, 3.0m, 10.5m }
                });

            migrationBuilder.InsertData(
                table: "VehicleModels",
                columns: new[] { "Id", "CheckDigit", "EngineCode", "ManufacturedCountry", "ManufacturedPlant", "ManufacturedYear", "Manufacturer", "ModelLongName", "ModelPicture", "ModelShortName", "ModelYear", "Price", "VehicleType" },
                values: new object[,]
                {
                    { 1000, "1", "EN123", 1, "A", "L", "TM", "Toyota Camry", "/images/vehiclemodels/Toyota_Camry.png", "Camry", 2020, 15000m, "Automobile" },
                    { 1001, "2", "EN456", 2, "B", "M", "FD", "Ford Mustang", "/images/vehiclemodels/Ford_Mustang.png", "Mustang", 2021, 50000m, "Automobile" },
                    { 1002, "3", "EN789", 1, "C", "K", "BM", "BMW X5", "/images/vehiclemodels/Bmw_X5.png", "X5", 2019, 35000m, "SUV" },
                    { 1003, "4", "EN101", 1, "D", "J", "HN", "Honda Civic", "/images/vehiclemodels/Honda_Civic.png", "Civic", 2018, 20000m, "Automobile" },
                    { 1004, "5", "EN202", 1, "E", "H", "NS", "Nissan Altima", "/images/vehiclemodels/Nissan_Altima.png", "Altima", 2017, 17500m, "Automobile" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AddressId",
                table: "AspNetUsers",
                column: "AddressId",
                unique: true,
                filter: "[AddressId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_EngineId",
                table: "Vehicles",
                column: "EngineId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ModelId",
                table: "Vehicles",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_UserId",
                table: "Vehicles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Engines");

            migrationBuilder.DropTable(
                name: "VehicleModels");

            migrationBuilder.DropTable(
                name: "Addresses");
        }
    }
}
