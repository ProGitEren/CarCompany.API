using Infrastucture.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface.Service_Interfaces
{
    public interface IFileService
    {
        string SaveFile(IFormFile file, string folderPath);

        void DeleteFile(string picturePath);

        string GenerateUniqueFolderName(string rootDirectory);

        void DeleteFolder(string folderPath);

        IEnumerable<string> GetFilesInDirectory(string folderPath);

        bool CheckFileType(IFormFile file);
    }

}