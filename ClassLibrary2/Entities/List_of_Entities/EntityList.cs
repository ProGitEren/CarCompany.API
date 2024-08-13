using Models.Enums;
using Models.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities.List_of_Entities
{
    public static class EntityList
    {
        public static readonly List<Engines> EnginesList = new List<Engines>
        {
            new Engines
            {
                Id = 1000,
                Volume = 2.0m,
                Hp = 150,
                Torque = 200,
                CompressionRatio = 10,
                diameterCm = 8.5m,
                EngineCode = "EN123",
                EngineName = "Standard 2.0L Inline 4 Engine",
                Cylinder = Cylinder.i4
            },
            new Engines
            {
                Id = 1001,
                Volume = 3.5m,
                Hp = 250,
                Torque = 350,
                CompressionRatio = 11,
                diameterCm = 10.0m,
                EngineCode = "EN456",
                EngineName = "High Output 3.5L V6 Engine",
                Cylinder = Cylinder.v6
            },
            new Engines
            {
                Id = 1002,
                Volume = 4.0m,
                Hp = 300,
                Torque = 400,
                CompressionRatio = 12,
                diameterCm = 11.0m,
                EngineCode = "EN789",
                EngineName = "Performance 4.0L V8 Engine",
                Cylinder = Cylinder.v8
            },
            new Engines
            {
                Id = 1003,
                Volume = 1.6m,
                Hp = 120,
                Torque = 180,
                CompressionRatio = 9,
                diameterCm = 7.5m,
                EngineCode = "EN101",
                EngineName = "Economy 1.6L Inline 4 Engine",
                Cylinder = Cylinder.i4
            },
            new Engines
            {
                Id = 1004,
                Volume = 2.5m,
                Hp = 200,
                Torque = 250,
                CompressionRatio = 10,
                diameterCm = 9.0m,
                EngineCode = "EN202",
                EngineName = "Unknown Engine", // Assuming the engine code "EN202" is not in the dictionary
                Cylinder = Cylinder.i4
            },
            new Engines
            {
                Id = 1005,
                Volume = 5.0m,
                Hp = 450,
                Torque = 500,
                CompressionRatio = 12,
                diameterCm = 12.0m,
                EngineCode = "F01V8",
                EngineName = "Ford V8 Engine",
                Cylinder = Cylinder.v8
            },
            new Engines
            {
                Id = 1006,
                Volume = 0.0m, // Electric motors might not have a traditional volume
                Hp = 200,
                Torque = 300,
                CompressionRatio = 0, // Not applicable for electric motors
                diameterCm = 0.0m, // Not applicable for electric motors
                EngineCode = "F02EM",
                EngineName = "Ford Electric Motor",
                Cylinder = Cylinder.i4 // Placeholder, as electric motors don't have cylinders
            },
            new Engines
            {
                Id = 1007,
                Volume = 2.5m,
                Hp = 350,
                Torque = 450,
                CompressionRatio = 11,
                diameterCm = 9.5m,
                EngineCode = "F03HY",
                EngineName = "Ford Hybrid Engine",
                Cylinder = Cylinder.v6
            },
            new Engines
            {
                Id = 1008,
                Volume = 3.0m,
                Hp = 350,
                Torque = 400,
                CompressionRatio = 10,
                diameterCm = 10.5m,
                EngineCode = "F04V6",
                EngineName = "Ford V6 Turbo Engine",
                Cylinder = Cylinder.v6
            },
            new Engines
            {
                Id = 1009,
                Volume = 4.5m,
                Hp = 400,
                Torque = 450,
                CompressionRatio = 11,
                diameterCm = 11.5m,
                EngineCode = "T01V8",
                EngineName = "Toyota V8 Engine",
                Cylinder = Cylinder.v8
            },
            new Engines
            {
                Id = 1010,
                Volume = 0.0m,
                Hp = 250,
                Torque = 350,
                CompressionRatio = 0,
                diameterCm = 0.0m,
                EngineCode = "T02EM",
                EngineName = "Toyota Electric Motor",
                Cylinder = Cylinder.i4 // Placeholder, as electric motors don't have cylinders
            },
            new Engines
            {
                Id = 1011,
                Volume = 2.5m,
                Hp = 300,
                Torque = 400,
                CompressionRatio = 10,
                diameterCm = 9.5m,
                EngineCode = "T03HY",
                EngineName = "Toyota Hybrid Engine",
                Cylinder = Cylinder.v6
            },
            new Engines
            {
                Id = 1012,
                Volume = 3.0m,
                Hp = 325,
                Torque = 375,
                CompressionRatio = 10,
                diameterCm = 10.5m,
                EngineCode = "T04V6",
                EngineName = "Toyota V6 Turbo Engine",
                Cylinder = Cylinder.v6
            },
            new Engines
            {
                Id = 1013,
                Volume = 4.5m,
                Hp = 450,
                Torque = 500,
                CompressionRatio = 12,
                diameterCm = 12.0m,
                EngineCode = "B01V8",
                EngineName = "BMW V8 Engine",
                Cylinder = Cylinder.v8
            },
            new Engines
            {
                Id = 1014,
                Volume = 0.0m,
                Hp = 250,
                Torque = 350,
                CompressionRatio = 0,
                diameterCm = 0.0m,
                EngineCode = "B02EM",
                EngineName = "BMW Electric Motor",
                Cylinder = Cylinder.i4 // Placeholder, as electric motors don't have cylinders
            },
            new Engines
            {
                Id = 1015,
                Volume = 2.5m,
                Hp = 300,
                Torque = 400,
                CompressionRatio = 11,
                diameterCm = 9.5m,
                EngineCode = "B03HY",
                EngineName = "BMW Hybrid Engine",
                Cylinder = Cylinder.v6
            },
            new Engines
            {
                Id = 1016,
                Volume = 3.0m,
                Hp = 325,
                Torque = 375,
                CompressionRatio = 10,
                diameterCm = 10.5m,
                EngineCode = "B04V6",
                EngineName = "BMW V6 Turbo Engine",
                Cylinder = Cylinder.v6
            },
            new Engines
            {
                Id = 1017,
                Volume = 4.5m,
                Hp = 450,
                Torque = 500,
                CompressionRatio = 12,
                diameterCm = 12.0m,
                EngineCode = "H01V8",
                EngineName = "Honda V8 Engine",
                Cylinder = Cylinder.v8
            },
            new Engines
            {
                Id = 1018,
                Volume = 0.0m,
                Hp = 250,
                Torque = 350,
                CompressionRatio = 0,
                diameterCm = 0.0m,
                EngineCode = "H02EM",
                EngineName = "Honda Electric Motor",
                Cylinder = Cylinder.i4 // Placeholder, as electric motors don't have cylinders
            },
            new Engines
            {
                Id = 1019,
                Volume = 2.5m,
                Hp = 300,
                Torque = 400,
                CompressionRatio = 11,
                diameterCm = 9.5m,
                EngineCode = "H03HY",
                EngineName = "Honda Hybrid Engine",
                Cylinder = Cylinder.v6
            },
            new Engines
            {
                Id = 1020,
                Volume = 3.0m,
                Hp = 325,
                Torque = 375,
                CompressionRatio = 10,
                diameterCm = 10.5m,
                EngineCode = "H04V6",
                EngineName = "Honda V6 Turbo Engine",
                Cylinder = Cylinder.v6
            },
            new Engines
            {
                Id = 1021,
                Volume = 4.5m,
                Hp = 450,
                Torque = 500,
                CompressionRatio = 12,
                diameterCm = 12.0m,
                EngineCode = "M01V8",
                EngineName = "Mercedes V8 Engine",
                Cylinder = Cylinder.v8
            },
            new Engines
            {
                Id = 1022,
                Volume = 0.0m,
                Hp = 250,
                Torque = 350,
                CompressionRatio = 0,
                diameterCm = 0.0m,
                EngineCode = "M02EM",
                EngineName = "Mercedes Electric Motor",
                Cylinder = Cylinder.i4 // Placeholder, as electric motors don't have cylinders
            },
            new Engines
            {
                Id = 1023,
                Volume = 2.5m,
                Hp = 300,
                Torque = 400,
                CompressionRatio = 11,
                diameterCm = 9.5m,
                EngineCode = "M03HY",
                EngineName = "Mercedes Hybrid Engine",
                Cylinder = Cylinder.v6
            },
            new Engines
            {
                Id = 1024,
                Volume = 3.0m,
                Hp = 325,
                Torque = 375,
                CompressionRatio = 10,
                diameterCm = 10.5m,
                EngineCode = "M04V6",
                EngineName = "Mercedes V6 Turbo Engine",
                Cylinder = Cylinder.v6
            }
        };

        public static readonly List<VehicleModels> ModelsList = new List<VehicleModels>
        {
            new VehicleModels
            {
                Id = 1000,
                CheckDigit = "1",
                ModelYear = 2020,
                ManufacturedCountry = 1,
                ManufacturedPlant = "A",
                ManufacturedYear = VinYearMapper.GetManufacturedYearCode(2020).ToString(),
                EngineCode = "EN123",
                Manufacturer = "TM",
                ModelLongName = "Toyota Camry",
                ModelShortName = "Camry",
                VehicleType = VehicleType.Automobile,
                
            },
            new VehicleModels
            {
                Id = 1001,
                CheckDigit = "2",
                ModelYear = 2021,
                ManufacturedCountry = 2,
                ManufacturedPlant = "B",
                ManufacturedYear = VinYearMapper.GetManufacturedYearCode(2021).ToString(),
                EngineCode = "EN456",
                Manufacturer = "FD",
                ModelLongName = "Ford Mustang",
                ModelShortName = "Mustang",
                VehicleType = VehicleType.Automobile
            },
            new VehicleModels
            {
                Id = 1002,
                CheckDigit = "3",
                ModelYear = 2019,
                ManufacturedCountry = 1,
                ManufacturedPlant = "C",
                ManufacturedYear = VinYearMapper.GetManufacturedYearCode(2019).ToString(),
                EngineCode = "EN789",
                Manufacturer = "BM",
                ModelLongName = "BMW X5",
                ModelShortName = "X5",
                VehicleType = VehicleType.SUV
            },
            new VehicleModels
            {
                Id = 1003,
                CheckDigit = "4",
                ModelYear = 2018,
                ManufacturedCountry = 1,
                ManufacturedPlant = "D",
                ManufacturedYear = VinYearMapper.GetManufacturedYearCode(2018).ToString(),
                EngineCode = "EN101",
                Manufacturer = "HN",
                ModelLongName = "Honda Civic",
                ModelShortName = "Civic",
                VehicleType = VehicleType.Automobile
            },
            new VehicleModels
            {
                Id = 1004,
                CheckDigit = "5",
                ModelYear = 2017,
                ManufacturedCountry = 1,
                ManufacturedPlant = "E",
                ManufacturedYear = VinYearMapper.GetManufacturedYearCode(2017).ToString(),
                EngineCode = "EN202",
                Manufacturer = "NS",
                ModelLongName = "Nissan Altima",
                ModelShortName = "Altima",
                VehicleType = VehicleType.Automobile
            }
        };
    }
}
