using AutoMapper;
using Infrastucture.Data;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.Interface.Repository_Interfaces;
using Microsoft.Extensions.FileProviders;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repository
{
    public class VehicleModelRepository : GenericRepository<VehicleModels, int?>, IVehicleModelRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileProvider _fileProvider;
        private readonly IMapper _mapper;


        public VehicleModelRepository(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper) : base(context)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;

        }


        public async Task<bool> AddAsync(RegisterVehicleModelDto dto)
        {

            if (dto.ModelPicture is not null)
            {
                var root = "/images/product/";
                var picname = $"{Guid.NewGuid()}" + dto.ModelPicture.FileName;
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                var src = root + picname;
                var pic_info = _fileProvider.GetFileInfo(src);
                var root_path = pic_info.PhysicalPath;
                using (var file_streem = new FileStream(root_path, FileMode.Create))
                {
                    await dto.ModelPicture.CopyToAsync(file_streem);
                }
                // Create New Product
                var res = _mapper.Map<VehicleModels>(dto);
                res.ModelPicture = src;
                await _context.VehicleModels.AddAsync(res);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        //Asp.Net Core 8 Web API :https://www.youtube.com/watch?v=UqegTYn2aKE&list=PLazvcyckcBwitbcbYveMdXlw8mqoBDbTT&index=1

        //update
        public async Task<bool> UpdateAsync(UpdateVehicleModelDto dto)
        {
            var vehiclemodel = await this.GetByIdAsync(dto.Id);
            if (vehiclemodel is not null)
            {
                var src = "";
                if (dto.ModelPicture is not null)
                {
                    var root = "/images/product/";
                    var productName = $"{Guid.NewGuid()}" + dto.ModelPicture.FileName;
                    if (!Directory.Exists(root))
                    {
                        Directory.CreateDirectory(root);
                    }

                    src = root + productName;
                    var picInfo = _fileProvider.GetFileInfo(src);
                    var rootPath = picInfo.PhysicalPath;
                    using (var fileStream = new FileStream(rootPath, FileMode.Create))
                    {
                        await dto.ModelPicture.CopyToAsync(fileStream);
                    }
                }
                //remove old picture
                if (!string.IsNullOrEmpty(vehiclemodel.ModelPicture))
                {
                    //delete old picture
                    var picInfo = _fileProvider.GetFileInfo(vehiclemodel.ModelPicture);
                    var rootPath = picInfo.PhysicalPath;
                    System.IO.File.Delete(rootPath);
                }

                //update model
                _mapper.Map(dto,vehiclemodel);
                vehiclemodel.ModelPicture = src;

                _context.VehicleModels.Update(vehiclemodel);
                await _context.SaveChangesAsync();


                return true;

            }
            return false;
        }

        //Asp.Net Core 8 Web API :https://www.youtube.com/watch?v=UqegTYn2aKE&list=PLazvcyckcBwitbcbYveMdXlw8mqoBDbTT&index=1

        //delete 
        public async Task<bool> DeleteAsync(int id)
        {
            var vehiclemodel = await this.GetByIdAsync(id);
            if (!string.IsNullOrEmpty(vehiclemodel.ModelPicture))
            {
                //delete old pic
                var pic_info = _fileProvider.GetFileInfo(vehiclemodel.ModelPicture);
                var root_path = pic_info.PhysicalPath;
                System.IO.File.Delete($"{root_path}");

                // Delete Database
                _context.VehicleModels.Remove(vehiclemodel);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


    }
}
