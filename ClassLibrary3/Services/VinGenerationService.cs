using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Services
{
    public class VinGenerationService : IVinGenerationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VinGenerationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public string GenerateVin(VehicleModels vehicleModel)
        {
            if (vehicleModel == null)
            {
                throw new ArgumentNullException(nameof(vehicleModel));
            }

            // Convert all string properties of vehicleModel to uppercase
            var properties = vehicleModel.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

            foreach (var property in properties)
            {
                var value = (string)property.GetValue(vehicleModel);
                if (!string.IsNullOrEmpty(value))
                {
                    property.SetValue(vehicleModel, value.ToUpper());
                }
            }

            // Use StringBuilder to construct the first 12 characters of the VIN
            var vinBuilder = new StringBuilder();
            vinBuilder.Append(vehicleModel.ManufacturedCountry);
            vinBuilder.Append(vehicleModel.Manufacturer);
            vinBuilder.Append(vehicleModel.EngineCode);
            vinBuilder.Append(vehicleModel.securityCode);
            vinBuilder.Append(vehicleModel.ManufacturedYear);
            vinBuilder.Append(vehicleModel.ManufacturedPlant);

            var first11Characters = vinBuilder.ToString();

            // Get the current max VIN suffix
            var currentMaxSuffix = _unitOfWork.VehicleRepository
                .GetAll()
                .Where(v => v.Vin.StartsWith(first11Characters))
                .Max(v => (int?)int.Parse(v.Vin.Substring(11))) ?? 0;

            // Increment the suffix to get the new VIN
            var vinSuffix = (currentMaxSuffix + 1).ToString("D6");

            // Combine the first 12 characters with the new suffix
            return first11Characters + vinSuffix;
        }

    }
}
