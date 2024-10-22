using AutoMapper;
using FluentValidation;
using Infrastucture.Data;
using Infrastucture.DTO.Dto_Engines;
using Infrastucture.DTO.DTO_OrderVehicles;
using Infrastucture.Errors;
using Infrastucture.Extensions;
using Infrastucture.Helpers;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
using Infrastucture.Params;
using Infrastucture.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Models.Entities;
using Models.Enums;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repository
{
    public class OrderVehicleRepository : GenericRepository<OrderVehicle, int?> , IOrderVehicleRepository
    {
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public OrderVehicleRepository(IMapper mapper, ApplicationDbContext context, IFileService fileService) : base(context)
        {
            _mapper = mapper;
            _context = context;
            _fileService = fileService;
        }
        public async Task<Tuple<OrderVehicle,List<string>>> AddAsync(CreateOrderVehicleDto dto, IValidator<OrderVehicle> _validator)
        {
            var validationerrorlist = new List<string>();
            if (dto is not null)
            {
                try
                {
                    if (await _context.Vehicles.FirstOrDefaultAsync(x => x.Vin == dto.VehicleId) is null)
                    {
                        validationerrorlist.Add("The Vehicle with this Id could not found in the system");
                    }


                    if (dto.Images.Any(x => !_fileService.CheckFileType(x))) 
                    {
                        validationerrorlist.AddRange(
                            dto.Images.Where(x => !_fileService.CheckFileType(x))
                            .Select(x => $"Type of file {x.FileName} is not allowed"));
                    }


                    if (validationerrorlist.Any())
                    {
                        return Tuple.Create(_mapper.Map<OrderVehicle>(dto),validationerrorlist); // no database operation
                    }
                    // Create new folder and save new files
                    var rootDirectory = "/images/ordervehicles/";


                    var folderName = _fileService.GenerateUniqueFolderName(rootDirectory);
                    var folderPath = Path.Combine(rootDirectory, folderName);

                    var orderVehicle = _mapper.Map<OrderVehicle>(dto);
                    orderVehicle.PictureFolderPath = folderPath;
                    validationerrorlist.AddRange(_validator.Validate(orderVehicle).stringErrors());

                    if (validationerrorlist.Any())
                    {
                        return Tuple.Create(orderVehicle, validationerrorlist); // no database operation
                    }


                    foreach (var file in dto.Images)
                    {
                        
                        var src = _fileService.SaveFile(file, folderPath);
                    }

                    

                    await _context.OrderVehicles.AddAsync(orderVehicle);
                    await _context.SaveChangesAsync();

                    return Tuple.Create(orderVehicle,validationerrorlist);
                }
                catch (Exception ex) 
                {
                    throw new Exception(ex.Message);
                }
            }
            return Tuple.Create(new OrderVehicle(), validationerrorlist);
        }

        public async Task<Tuple<OrderVehicle,List<string>>> UpdateAsync(UpdateOrderVehicleDto dto, IValidator<OrderVehicle> _validator)
        {
            var orderVehicle = await _context.OrderVehicles.FindAsync(dto.Id);
            var validationerrorlist = new List<string>();

            if (orderVehicle != null)
            {
                try
                {
                    if (await _context.Vehicles.FirstOrDefaultAsync(x => x.Vin == dto.VehicleId) is null)
                    {
                        validationerrorlist.Add("The Vehicle with this Id could not found in the system");
                    }



                    //if (dto.Images != null && dto.Images.Count > 0)
                    //{
                        //string folderPath;
                        //// Delete existing folder
                        //if (orderVehicle.PictureFolderPath is not null)
                        //{
                        //    folderPath = orderVehicle.PictureFolderPath;
                        ////    var existingFiles = _fileService.GetFilesInDirectory(folderPath).ToList();

                        ////    // Identify files to delete (those not in the new upload)
                        ////    var newFileNames = dto.Images.Select(f => f.FileName).ToHashSet();

                        ////    // Create a list to keep track of files to delete
                        ////    var filesToDelete = new List<string>();

                        ////    foreach (var existingFile in existingFiles)
                        ////    {
                        ////        var fileName = Path.GetFileName(existingFile);
                        ////        var originalFileName = fileName.Substring(fileName.IndexOf('_') + 1);
                        ////        if (!newFileNames.Contains(originalFileName))
                        ////        {
                        ////            // Delete the file if it's not in the new upload list
                        ////            _fileService.DeleteFile(Path.Combine(folderPath, fileName));
                        ////        }
                               
                        ////    }

                        ////    dto.Images = dto.Images.Where(file =>
                        ////    {
                        ////        var existingFileMatch = existingFiles.Any(existingFile =>
                        ////        {
                        ////            var existingFileName = Path.GetFileName(existingFile);
                        ////            var underscoreIndex = existingFileName.IndexOf('_');
                        ////            var originalExi
                        ////            return originalExistingFileName == file.FileName;
                        ////        });

                        ////        return !existingFileMatch;
                        ////    }).ToList();

                        //}
                        //else
                        //{
                        //    // Create new folder and save new files
                        //    var rootDirectory = "/images/ordervehicles/";

                        //    var folderName = _fileService.GenerateUniqueFolderName(rootDirectory);
                        //    folderPath = Path.Combine(rootDirectory, folderName);
                        //}

                       
                        //foreach (var file in dto.Images)
                        //{
                        //    // Create updated file
                        //    var src = _fileService.SaveFile(file, folderPath);
                        //}

                        //orderVehicle.PictureFolderPath = folderPath;stingFileName = underscoreIndex >= 0 ? existingFileName.Substring(underscoreIndex + 1) : existingFileName;

                    //}

                    _mapper.Map(dto, orderVehicle);
                    validationerrorlist.AddRange(_validator.Validate(orderVehicle).stringErrors());

                    if (validationerrorlist.Any())
                    {
                        return Tuple.Create(orderVehicle, validationerrorlist); // no database operation
                    }

                    _context.OrderVehicles.Update(orderVehicle);
                    await _context.SaveChangesAsync();

                    return Tuple.Create(orderVehicle, validationerrorlist);
                }

                catch (Exception ex) 
                {
                    throw new Exception(ex.Message);
                }
            }

            return Tuple.Create(new OrderVehicle(), validationerrorlist);
        }

        public async Task<bool> DeleteAsync(int? id)
        {
            var orderVehicle = await _context.OrderVehicles.FindAsync(id);

            if (orderVehicle != null)
            {
                // Delete the image files from the server
                if (!string.IsNullOrEmpty(orderVehicle.PictureFolderPath))
                {
                    string rootDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var files = _fileService.GetFilesInDirectory(orderVehicle.PictureFolderPath);
                    foreach (var file in files) 
                    {
                        var relativePath = file.Replace(rootDirectory, "").TrimStart(Path.DirectorySeparatorChar);
                        _fileService.DeleteFile(relativePath); 
                    }
                    _fileService.DeleteFolder(orderVehicle.PictureFolderPath);                   

                }

                // Delete the OrderVehicle record from the database
                _context.OrderVehicles.Remove(orderVehicle);

                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        

        public async Task<IdentityResult> DeleteFileAsync(DeleteFileDto dto)
        {
            var orderVehicle = await _context.OrderVehicles.FindAsync(dto.Id);

            if (orderVehicle is null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "The Order Vehicle could not be found." });
            }
            var folderPath = orderVehicle.PictureFolderPath;
            var skipstring = "localhost:7218/";
            var originalRelativeFilePath = dto.FilePath.Substring(dto.FilePath.IndexOf(skipstring) + skipstring.Length);
            var filesindirectory = _fileService.GetFilesInDirectory(folderPath).Select(x => Path.GetFileName(x)).ToList();
            var fileName = originalRelativeFilePath.Substring(originalRelativeFilePath.IndexOf(folderPath) + folderPath.Length + 1);
            if (!filesindirectory.Contains(fileName)) 
            {
                return IdentityResult.Failed(new IdentityError { Description = "File delete failed: File does not exist in the given folder path." });
            }
            
            try
            {
                _fileService.DeleteFile(originalRelativeFilePath);
                return IdentityResult.Success;
            }
            catch (Exception)
            {
                return IdentityResult.Failed(new IdentityError { Description = "File delete failed: Unexpected error occured." });
            }
        }

        public async Task<IdentityResult> AddFileAsync(AddFileDto dto)
        {
            var orderVehicle = await _context.OrderVehicles.FindAsync(dto.Id);

            if (orderVehicle is null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "The Order Vehicle could not be found." });
            }

            var folderPath = orderVehicle.PictureFolderPath;
            

            if (!_fileService.CheckFileType(dto.Picture))
                return IdentityResult.Failed(new IdentityError { Description = "Only JPEG and PNG type files are allowed."});

            try
            {
                _fileService.SaveFile(dto.Picture,folderPath);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = "File create failed: File path could not be found." });
            }
        }

        public async Task<ParamsOrderVehicleDto> GetAllAsync(OrderVehicleParams orderVehicleParams)
        {
            var result = new ParamsOrderVehicleDto();
            var query = _context.OrderVehicles.AsNoTracking();
            //var query = await _context.Products
            //    .Include(x => x.Category)
            //    .AsNoTracking()
            //    .ToListAsync();

            //search by Name
            if (!string.IsNullOrEmpty(orderVehicleParams.Search))
                query = query.Where(x => x.ModelName.ToLower().Contains(orderVehicleParams.Search));

            //filtering 

            if (!string.IsNullOrEmpty(orderVehicleParams.VehicleId))
                query = query.Where(x => x.VehicleId == orderVehicleParams.VehicleId);


            //sorting
            if (!string.IsNullOrEmpty(orderVehicleParams.Sorting))
            {
                query = orderVehicleParams.Sorting switch
                {
                    "PriceAsc" => query.OrderBy(x => x.Price),
                    "PriceDesc" => query.OrderByDescending(x => x.Price),
                    "VehicleAsc" => query.OrderBy(x => x.VehicleId),
                    "VehicleDesc" => query.OrderByDescending(x => x.VehicleId),
                    _ => query.OrderBy(x => x.ModelName)
                };
            }

            //paging
            result.TotalItems = query.Count();
            query = query.Skip((orderVehicleParams.Pagesize) * (orderVehicleParams.PageNumber - 1)).Take(orderVehicleParams.Pagesize);

            var list = await query.ToListAsync(); // the execution will be done at the end
            result.OrderVehicleDtos = _mapper.Map<List<OrderVehicleDto>>(list);
            result.PageItemCount = list.Count;
            return result;
        }
    }
}
