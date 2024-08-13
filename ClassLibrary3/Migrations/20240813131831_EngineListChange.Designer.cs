﻿// <auto-generated />
using System;
using Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastucture.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240813131831_EngineListChange")]
    partial class EngineListChange
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ClassLibrary2.Entities.Address", b =>
                {
                    b.Property<Guid>("AddressId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("state")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("zipcode")
                        .HasColumnType("int");

                    b.HasKey("AddressId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("ClassLibrary2.Entities.AppUsers", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<Guid?>("AddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("birthtime")
                        .HasColumnType("datetime2");

                    b.Property<string>("roles")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AddressId")
                        .IsUnique()
                        .HasFilter("[AddressId] IS NOT NULL");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Models.Entities.Engines", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1000L);

                    b.Property<int>("CompressionRatio")
                        .HasColumnType("int");

                    b.Property<string>("Cylinder")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EngineCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nchar(5)");

                    b.Property<string>("EngineName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Hp")
                        .HasColumnType("int");

                    b.Property<int>("Torque")
                        .HasColumnType("int");

                    b.Property<decimal>("Volume")
                        .HasColumnType("decimal(18,4)");

                    b.Property<decimal>("diameterCm")
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("Id");

                    b.ToTable("Engines");

                    b.HasData(
                        new
                        {
                            Id = 1000,
                            CompressionRatio = 10,
                            Cylinder = "i4",
                            EngineCode = "EN123",
                            EngineName = "Standard 2.0L Inline 4 Engine",
                            Hp = 150,
                            Torque = 200,
                            Volume = 2.0m,
                            diameterCm = 8.5m
                        },
                        new
                        {
                            Id = 1001,
                            CompressionRatio = 11,
                            Cylinder = "v6",
                            EngineCode = "EN456",
                            EngineName = "High Output 3.5L V6 Engine",
                            Hp = 250,
                            Torque = 350,
                            Volume = 3.5m,
                            diameterCm = 10.0m
                        },
                        new
                        {
                            Id = 1002,
                            CompressionRatio = 12,
                            Cylinder = "v8",
                            EngineCode = "EN789",
                            EngineName = "Performance 4.0L V8 Engine",
                            Hp = 300,
                            Torque = 400,
                            Volume = 4.0m,
                            diameterCm = 11.0m
                        },
                        new
                        {
                            Id = 1003,
                            CompressionRatio = 9,
                            Cylinder = "i4",
                            EngineCode = "EN101",
                            EngineName = "Economy 1.6L Inline 4 Engine",
                            Hp = 120,
                            Torque = 180,
                            Volume = 1.6m,
                            diameterCm = 7.5m
                        },
                        new
                        {
                            Id = 1004,
                            CompressionRatio = 10,
                            Cylinder = "i4",
                            EngineCode = "EN202",
                            EngineName = "Unknown Engine",
                            Hp = 200,
                            Torque = 250,
                            Volume = 2.5m,
                            diameterCm = 9.0m
                        },
                        new
                        {
                            Id = 1005,
                            CompressionRatio = 12,
                            Cylinder = "v8",
                            EngineCode = "F01V8",
                            EngineName = "Ford V8 Engine",
                            Hp = 450,
                            Torque = 500,
                            Volume = 5.0m,
                            diameterCm = 12.0m
                        },
                        new
                        {
                            Id = 1006,
                            CompressionRatio = 0,
                            Cylinder = "i4",
                            EngineCode = "F02EM",
                            EngineName = "Ford Electric Motor",
                            Hp = 200,
                            Torque = 300,
                            Volume = 0.0m,
                            diameterCm = 0.0m
                        },
                        new
                        {
                            Id = 1007,
                            CompressionRatio = 11,
                            Cylinder = "v6",
                            EngineCode = "F03HY",
                            EngineName = "Ford Hybrid Engine",
                            Hp = 350,
                            Torque = 450,
                            Volume = 2.5m,
                            diameterCm = 9.5m
                        },
                        new
                        {
                            Id = 1008,
                            CompressionRatio = 10,
                            Cylinder = "v6",
                            EngineCode = "F04V6",
                            EngineName = "Ford V6 Turbo Engine",
                            Hp = 350,
                            Torque = 400,
                            Volume = 3.0m,
                            diameterCm = 10.5m
                        },
                        new
                        {
                            Id = 1009,
                            CompressionRatio = 11,
                            Cylinder = "v8",
                            EngineCode = "T01V8",
                            EngineName = "Toyota V8 Engine",
                            Hp = 400,
                            Torque = 450,
                            Volume = 4.5m,
                            diameterCm = 11.5m
                        },
                        new
                        {
                            Id = 1010,
                            CompressionRatio = 0,
                            Cylinder = "i4",
                            EngineCode = "T02EM",
                            EngineName = "Toyota Electric Motor",
                            Hp = 250,
                            Torque = 350,
                            Volume = 0.0m,
                            diameterCm = 0.0m
                        },
                        new
                        {
                            Id = 1011,
                            CompressionRatio = 10,
                            Cylinder = "v6",
                            EngineCode = "T03HY",
                            EngineName = "Toyota Hybrid Engine",
                            Hp = 300,
                            Torque = 400,
                            Volume = 2.5m,
                            diameterCm = 9.5m
                        },
                        new
                        {
                            Id = 1012,
                            CompressionRatio = 10,
                            Cylinder = "v6",
                            EngineCode = "T04V6",
                            EngineName = "Toyota V6 Turbo Engine",
                            Hp = 325,
                            Torque = 375,
                            Volume = 3.0m,
                            diameterCm = 10.5m
                        },
                        new
                        {
                            Id = 1013,
                            CompressionRatio = 12,
                            Cylinder = "v8",
                            EngineCode = "B01V8",
                            EngineName = "BMW V8 Engine",
                            Hp = 450,
                            Torque = 500,
                            Volume = 4.5m,
                            diameterCm = 12.0m
                        },
                        new
                        {
                            Id = 1014,
                            CompressionRatio = 0,
                            Cylinder = "i4",
                            EngineCode = "B02EM",
                            EngineName = "BMW Electric Motor",
                            Hp = 250,
                            Torque = 350,
                            Volume = 0.0m,
                            diameterCm = 0.0m
                        },
                        new
                        {
                            Id = 1015,
                            CompressionRatio = 11,
                            Cylinder = "v6",
                            EngineCode = "B03HY",
                            EngineName = "BMW Hybrid Engine",
                            Hp = 300,
                            Torque = 400,
                            Volume = 2.5m,
                            diameterCm = 9.5m
                        },
                        new
                        {
                            Id = 1016,
                            CompressionRatio = 10,
                            Cylinder = "v6",
                            EngineCode = "B04V6",
                            EngineName = "BMW V6 Turbo Engine",
                            Hp = 325,
                            Torque = 375,
                            Volume = 3.0m,
                            diameterCm = 10.5m
                        },
                        new
                        {
                            Id = 1017,
                            CompressionRatio = 12,
                            Cylinder = "v8",
                            EngineCode = "H01V8",
                            EngineName = "Honda V8 Engine",
                            Hp = 450,
                            Torque = 500,
                            Volume = 4.5m,
                            diameterCm = 12.0m
                        },
                        new
                        {
                            Id = 1018,
                            CompressionRatio = 0,
                            Cylinder = "i4",
                            EngineCode = "H02EM",
                            EngineName = "Honda Electric Motor",
                            Hp = 250,
                            Torque = 350,
                            Volume = 0.0m,
                            diameterCm = 0.0m
                        },
                        new
                        {
                            Id = 1019,
                            CompressionRatio = 11,
                            Cylinder = "v6",
                            EngineCode = "H03HY",
                            EngineName = "Honda Hybrid Engine",
                            Hp = 300,
                            Torque = 400,
                            Volume = 2.5m,
                            diameterCm = 9.5m
                        },
                        new
                        {
                            Id = 1020,
                            CompressionRatio = 10,
                            Cylinder = "v6",
                            EngineCode = "H04V6",
                            EngineName = "Honda V6 Turbo Engine",
                            Hp = 325,
                            Torque = 375,
                            Volume = 3.0m,
                            diameterCm = 10.5m
                        },
                        new
                        {
                            Id = 1021,
                            CompressionRatio = 12,
                            Cylinder = "v8",
                            EngineCode = "M01V8",
                            EngineName = "Mercedes V8 Engine",
                            Hp = 450,
                            Torque = 500,
                            Volume = 4.5m,
                            diameterCm = 12.0m
                        },
                        new
                        {
                            Id = 1022,
                            CompressionRatio = 0,
                            Cylinder = "i4",
                            EngineCode = "M02EM",
                            EngineName = "Mercedes Electric Motor",
                            Hp = 250,
                            Torque = 350,
                            Volume = 0.0m,
                            diameterCm = 0.0m
                        },
                        new
                        {
                            Id = 1023,
                            CompressionRatio = 11,
                            Cylinder = "v6",
                            EngineCode = "M03HY",
                            EngineName = "Mercedes Hybrid Engine",
                            Hp = 300,
                            Torque = 400,
                            Volume = 2.5m,
                            diameterCm = 9.5m
                        },
                        new
                        {
                            Id = 1024,
                            CompressionRatio = 10,
                            Cylinder = "v6",
                            EngineCode = "M04V6",
                            EngineName = "Mercedes V6 Turbo Engine",
                            Hp = 325,
                            Torque = 375,
                            Volume = 3.0m,
                            diameterCm = 10.5m
                        });
                });

            modelBuilder.Entity("Models.Entities.VehicleModels", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1000L);

                    b.Property<string>("CheckDigit")
                        .IsRequired()
                        .HasColumnType("nchar(1)");

                    b.Property<string>("EngineCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nchar(5)");

                    b.Property<int>("ManufacturedCountry")
                        .HasColumnType("int");

                    b.Property<string>("ManufacturedPlant")
                        .IsRequired()
                        .HasColumnType("nchar(1)");

                    b.Property<string>("ManufacturedYear")
                        .IsRequired()
                        .HasColumnType("nchar(1)");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("nchar(2)");

                    b.Property<string>("ModelLongName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ModelShortName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("ModelYear")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("VehicleType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("VehicleModels");

                    b.HasData(
                        new
                        {
                            Id = 1000,
                            CheckDigit = "1",
                            EngineCode = "EN123",
                            ManufacturedCountry = 1,
                            ManufacturedPlant = "A",
                            ManufacturedYear = "L",
                            Manufacturer = "TM",
                            ModelLongName = "Toyota Camry",
                            ModelShortName = "Camry",
                            ModelYear = 2020,
                            Quantity = 0,
                            VehicleType = "Automobile"
                        },
                        new
                        {
                            Id = 1001,
                            CheckDigit = "2",
                            EngineCode = "EN456",
                            ManufacturedCountry = 2,
                            ManufacturedPlant = "B",
                            ManufacturedYear = "M",
                            Manufacturer = "FD",
                            ModelLongName = "Ford Mustang",
                            ModelShortName = "Mustang",
                            ModelYear = 2021,
                            Quantity = 0,
                            VehicleType = "Automobile"
                        },
                        new
                        {
                            Id = 1002,
                            CheckDigit = "3",
                            EngineCode = "EN789",
                            ManufacturedCountry = 1,
                            ManufacturedPlant = "C",
                            ManufacturedYear = "K",
                            Manufacturer = "BM",
                            ModelLongName = "BMW X5",
                            ModelShortName = "X5",
                            ModelYear = 2019,
                            Quantity = 0,
                            VehicleType = "SUV"
                        },
                        new
                        {
                            Id = 1003,
                            CheckDigit = "4",
                            EngineCode = "EN101",
                            ManufacturedCountry = 1,
                            ManufacturedPlant = "D",
                            ManufacturedYear = "J",
                            Manufacturer = "HN",
                            ModelLongName = "Honda Civic",
                            ModelShortName = "Civic",
                            ModelYear = 2018,
                            Quantity = 0,
                            VehicleType = "Automobile"
                        },
                        new
                        {
                            Id = 1004,
                            CheckDigit = "5",
                            EngineCode = "EN202",
                            ManufacturedCountry = 1,
                            ManufacturedPlant = "E",
                            ManufacturedYear = "H",
                            Manufacturer = "NS",
                            ModelLongName = "Nissan Altima",
                            ModelShortName = "Altima",
                            ModelYear = 2017,
                            Quantity = 0,
                            VehicleType = "Automobile"
                        });
                });

            modelBuilder.Entity("Models.Entities.Vehicles", b =>
                {
                    b.Property<string>("Vin")
                        .HasColumnType("nchar(17)");

                    b.Property<decimal>("Averagefuelin")
                        .HasColumnType("decimal(18, 4)");

                    b.Property<decimal>("Averagefuelout")
                        .HasColumnType("decimal(18, 4)");

                    b.Property<int>("BaggageVolume")
                        .HasColumnType("int");

                    b.Property<int>("COemmission")
                        .HasColumnType("int");

                    b.Property<int>("DrivenKM")
                        .HasColumnType("int");

                    b.Property<int>("EngineId")
                        .HasColumnType("int");

                    b.Property<int>("FuelCapacity")
                        .HasColumnType("int");

                    b.Property<int>("MaxAllowedWeight")
                        .HasColumnType("int");

                    b.Property<int>("MinWeight")
                        .HasColumnType("int");

                    b.Property<int>("ModelId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Vin");

                    b.HasIndex("EngineId");

                    b.HasIndex("ModelId");

                    b.HasIndex("UserId");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("ClassLibrary2.Entities.AppUsers", b =>
                {
                    b.HasOne("ClassLibrary2.Entities.Address", "Address")
                        .WithOne("User")
                        .HasForeignKey("ClassLibrary2.Entities.AppUsers", "AddressId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ClassLibrary2.Entities.AppUsers", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ClassLibrary2.Entities.AppUsers", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClassLibrary2.Entities.AppUsers", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ClassLibrary2.Entities.AppUsers", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.Entities.Vehicles", b =>
                {
                    b.HasOne("Models.Entities.Engines", "Engine")
                        .WithMany("Vehicles")
                        .HasForeignKey("EngineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Entities.VehicleModels", "VehicleModel")
                        .WithMany("Vehicles")
                        .HasForeignKey("ModelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClassLibrary2.Entities.AppUsers", "User")
                        .WithMany("Vehicles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Engine");

                    b.Navigation("User");

                    b.Navigation("VehicleModel");
                });

            modelBuilder.Entity("ClassLibrary2.Entities.Address", b =>
                {
                    b.Navigation("User")
                        .IsRequired();
                });

            modelBuilder.Entity("ClassLibrary2.Entities.AppUsers", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("Models.Entities.Engines", b =>
                {
                    b.Navigation("Vehicles");
                });

            modelBuilder.Entity("Models.Entities.VehicleModels", b =>
                {
                    b.Navigation("Vehicles");
                });
#pragma warning restore 612, 618
        }
    }
}
