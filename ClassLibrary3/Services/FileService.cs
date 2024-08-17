using Infrastucture.Interface.Service_Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Services
{
    public class FileService : IFileService
    {
        private readonly IFileProvider _fileProvider;

        public FileService(IFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public string SaveFile(IFormFile file, string folderPath)
        {
            var rootPath = _fileProvider.GetFileInfo(folderPath).PhysicalPath;
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(rootPath, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return Path.Combine(folderPath, uniqueFileName);
        }

        public void DeleteFile(string picturePath) 
        {
            var picInfo = _fileProvider.GetFileInfo(picturePath);
            var rootPath = picInfo.PhysicalPath;
            System.IO.File.Delete(rootPath);
        }
       
    }
}