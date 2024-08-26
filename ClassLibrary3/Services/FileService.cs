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

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

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

        public void DeleteFolder(string folderPath)
        {
            var picInfo = _fileProvider.GetFileInfo(folderPath);
            var rootPath = picInfo.PhysicalPath;
            if (System.IO.Directory.Exists(rootPath))
            {
                System.IO.Directory.Delete(rootPath);
            }
        }
        public string GenerateUniqueFolderName(string rootDirectory)
        {
            var random = new Random();
            string folderName;
            var rootPath = _fileProvider.GetFileInfo(rootDirectory).PhysicalPath;

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            // Get the list of existing folder names in the directory
            var existingFolders = Directory.GetDirectories(rootPath)
                                           .Select(Path.GetFileName)
                                           .Where(name => name.Length >= 6)
                                           .Select(name => name.Substring(0, 6))
                                           .ToHashSet();

            do
            {
                int randomNumber = random.Next(100000, 999999); // Generates a number between 100000 and 999999
                folderName = randomNumber.ToString("D6"); // Converts the number to a string with 6 digits
            }
            while (existingFolders.Contains(folderName)); // Check if the folder name is already in use

            return folderName;
        }

        public IEnumerable<string> GetFilesInDirectory(string folderPath)
        {
            var directoryContents = _fileProvider.GetDirectoryContents(folderPath);

            return directoryContents
                .Where(file => !file.IsDirectory)
                .Select(file => file.PhysicalPath);
        }

        public bool CheckFileType(IFormFile file)
        {
            // Check by content type
            var allowedContentTypes = new[] { "image/jpeg", "image/png" };

            if (!allowedContentTypes.Contains(file.ContentType.ToLower()))
            {
                return false;
            }

            

            return true;
        }

    }
}