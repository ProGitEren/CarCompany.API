using AutoMapper;
using Infrastucture.Data;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
using Infrastucture.Params;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Models.Entities;
using Models.Enums;
using Models.Mapping;
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
        private readonly IFileService _fileService;


        public VehicleModelRepository(ApplicationDbContext context, IFileProvider fileProvider, IMapper mapper,IFileService fileService) : base(context)
        {
            _context = context;
            _fileProvider = fileProvider;
            _mapper = mapper;
            _fileService = fileService;

        }

        public async Task<VehicleModels> AddAsync(RegisterVehicleModelDto dto)
        {

            if (dto.ModelPicture is not null)
            {
                try 
                {
                    var root = "/images/vehiclemodels/";
                   
                    var src = _fileService.SaveFile(dto.ModelPicture, root);
                    var vehiclemodel = _mapper.Map<VehicleModels>(dto);
                    vehiclemodel.ModelPicture = src;
                    vehiclemodel.ManufacturedYear = VinYearMapper.GetManufacturedYearCode(vehiclemodel.ModelYear).ToString();
                    await _context.VehicleModels.AddAsync(vehiclemodel);
                    await _context.SaveChangesAsync();
                    return vehiclemodel;
                }
                catch (Exception ex) 
                {
                    throw new Exception(ex.Message);
                }
                
            }
            return null;
        }

        //Asp.Net Core 8 Web API :https://www.youtube.com/watch?v=UqegTYn2aKE&list=PLazvcyckcBwitbcbYveMdXlw8mqoBDbTT&index=1

        //update
            public async Task<VehicleModels> UpdateAsync(UpdateVehicleModelDto dto)
            {
                var vehiclemodel = await this.GetByIdAsync(dto.Id);
                if (vehiclemodel is not null)
                {
                    var src = vehiclemodel.ModelPicture; // previous picture will be used if new picture is null
                    if (dto.ModelPicture is not null)
                    {
                        try
                        {
                            var root = "/images/vehiclemodels/";
                            
                            src = _fileService.SaveFile(dto.ModelPicture, root);

                            //remove old picture
                            if (!string.IsNullOrEmpty(vehiclemodel.ModelPicture))
                            {
                                if (System.IO.File.Exists(vehiclemodel.ModelPicture))
                                {
                                    _fileService.DeleteFile(vehiclemodel.ModelPicture);
                                }
                                else if (System.IO.File.Exists(vehiclemodel.ModelPicture.ToLower()))
                                {
                                _fileService.DeleteFile(vehiclemodel.ModelPicture.ToLower());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }


                    //update model
                    _mapper.Map(dto,vehiclemodel);
                    vehiclemodel.ManufacturedYear = VinYearMapper.GetManufacturedYearCode(vehiclemodel.ModelYear).ToString();
                    vehiclemodel.ModelPicture = src;
                    _context.VehicleModels.Update(vehiclemodel);
                    await _context.SaveChangesAsync();
                    return vehiclemodel;
                }
                return null;
            }

        //Asp.Net Core 8 Web API :https://www.youtube.com/watch?v=UqegTYn2aKE&list=PLazvcyckcBwitbcbYveMdXlw8mqoBDbTT&index=1

        //delete 
        public async Task<bool> DeleteAsync(int? id)
        {
            var vehiclemodel = await this.GetByIdAsync(id);

            if (!string.IsNullOrEmpty(vehiclemodel.ModelPicture))
            {
                if (!string.IsNullOrEmpty(vehiclemodel.ModelPicture))
                {
                    if (System.IO.File.Exists(vehiclemodel.ModelPicture))
                    {
                        _fileService.DeleteFile(vehiclemodel.ModelPicture);
                    }
                    else if (System.IO.File.Exists(vehiclemodel.ModelPicture.ToLower()))
                    {
                        _fileService.DeleteFile(vehiclemodel.ModelPicture.ToLower());
                    }
                }
                // Delete Database
                _context.VehicleModels.Remove(vehiclemodel);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<ParamsModelDto> GetAllAsync(VehiclemodelParams modelParams)
        {
            var result = new ParamsModelDto();
            var query =  _context.VehicleModels.AsNoTracking();
            //var query = await _context.Products
            //    .Include(x => x.Category)
            //    .AsNoTracking()
            //    .ToListAsync();

            //search by Name
            if (!string.IsNullOrEmpty(modelParams.Search))
                query = query.Where(x => x.ModelLongName.ToLower().Contains(modelParams.Search));

            //filtering 
            if (modelParams.ModelId.HasValue)
                query = query.Where(x => x.Id == modelParams.ModelId.Value);

            if (modelParams.StartingModelYear.HasValue && modelParams.EndingModelYear.HasValue)
            {
                query = query.Where(x => (x.ModelYear >= modelParams.StartingModelYear.Value) && (x.ModelYear <= modelParams.EndingModelYear));
            }
            else if (modelParams.StartingModelYear.HasValue)
            { 
                query = query.Where(x => x.ModelYear >= modelParams.StartingModelYear.Value);
            }
            else if (modelParams.EndingModelYear.HasValue)
            {
                query = query.Where(x => x.ModelYear <= modelParams.EndingModelYear.Value);
            }

            if (!string.IsNullOrEmpty(modelParams.VehicleType))
                query = query.Where(x => x.VehicleType == (VehicleType)Enum.Parse(typeof(VehicleType),modelParams.VehicleType));

            //if (modelParams.Man.HasValue)
            //    query = query.Where(x => x.ModelYear == modelParams.ModelYear.Value).AsQueryable();
            //LATER ON THE MANUFACTURER NAME WILL BE ADDED AND WILL BE FILTERED


            //sorting
            if (!string.IsNullOrEmpty(modelParams.Sorting))
            {
                query = modelParams.Sorting switch
                {
                    "PriceAsc" => query.OrderBy(x => x.Price),
                    "PriceDesc" => query.OrderByDescending(x => x.Price),
                    "YearAsc" => query.OrderBy(x => x.ModelYear).ThenBy(x => x.ModelLongName),
                    "YearDesc" => query.OrderByDescending(x => x.ModelYear).ThenByDescending(x => x.ModelLongName),
                    "QuantityAsc" => query.OrderBy(x => x.Quantity),
                    "QuantityDesc" => query.OrderByDescending(x => x.Quantity),
                    _ => query.OrderBy(x => x.ModelLongName)
                };
            }
            
            //paging
            
            result.TotalItems = query.Count();

            query = query.Skip((modelParams.Pagesize) * (modelParams.PageNumber - 1)).Take(modelParams.Pagesize);
            
            var list = await query.ToListAsync(); // the execution will be done at the end
            result.VehicleModelDtos = _mapper.Map<List<VehicleModelDto>>(list);
            result.PageItemCount = list.Count;
            return result;
            
        }

    }
}
