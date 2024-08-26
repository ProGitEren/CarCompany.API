using AutoMapper;
using Infrastucture.DTO.DTO_OrderVehicles;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Helpers
{
    public class OrderVehicleUrlResolver : IValueResolver<OrderVehicle, OrderVehicleDto, List<string>>
    {
        private readonly IConfiguration _configuration;
        private readonly IFileProvider _fileProvider;

        public OrderVehicleUrlResolver(IConfiguration configuration, IFileProvider fileProvider)
        {
            _configuration = configuration;
            _fileProvider = fileProvider;
        }

        public List<string> Resolve(OrderVehicle source, OrderVehicleDto destination, List<string> destMember, ResolutionContext context)
        {
            var picturePaths = new List<string>();

            if (!string.IsNullOrEmpty(source.PictureFolderPath))
            {
                // Get the contents of the directory using IFileProvider
                var directoryContents = _fileProvider.GetDirectoryContents(source.PictureFolderPath);

                foreach (var file in directoryContents.Where(fi => !fi.IsDirectory))
                {
                    var relativePath = Path.Combine(source.PictureFolderPath, file.Name);

                    // Ensure the URL is constructed correctly
                    var normalizedRelativePath = relativePath.Replace("\\", "/").TrimStart('/');
                    var baseUrl = _configuration["API_url"].TrimEnd('/');

                    picturePaths.Add($"{baseUrl}/{normalizedRelativePath}");
                }
            }

            return picturePaths;
        }
    }
}

