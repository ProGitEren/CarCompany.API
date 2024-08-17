using AutoMapper;
using Infrastucture.Data;
using Infrastucture.DTO.Dto_Engines;
using Infrastucture.DTO.Dto_Vehicles;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Params;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repository
{
    public class EngineRepository : GenericRepository<Engines, int?>, IEngineRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public EngineRepository(ApplicationDbContext context,IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ParamsEngineDto> GetAllAsync(EngineParams engineParams)
        {
            var result = new ParamsEngineDto();
            var query = _context.Engines.AsNoTracking();
            //var query = await _context.Products
            //    .Include(x => x.Category)
            //    .AsNoTracking()
            //    .ToListAsync();

            //search by Name
            if (!string.IsNullOrEmpty(engineParams.Search))
                query = query.Where(x => x.EngineName.ToLower().Contains(engineParams.Search));

            //filtering 
            if (engineParams.Id.HasValue)
                query = query.Where(x => x.Id == engineParams.Id.Value);

            if (!string.IsNullOrEmpty(engineParams.EngineCode))
                query = query.Where(x => x.EngineCode == engineParams.EngineCode);

            if (!string.IsNullOrEmpty(engineParams.Cylinder))
                query = query.Where(x => x.Cylinder == (Cylinder)Enum.Parse(typeof(Cylinder),engineParams.Cylinder));


            //if (engineParams.Man.HasValue)
            //    query = query.Where(x => x.ModelYear == engineParams.ModelYear.Value).AsQueryable();
            //LATER ON THE MANUFACTURER NAME WILL BE ADDED AND WILL BE FILTERED


            //sorting
            if (!string.IsNullOrEmpty(engineParams.Sorting))
            {
                query = engineParams.Sorting switch
                {
                    "HpAsc" => query.OrderBy(x => x.Hp).ThenBy(x => x.EngineCode),
                    "HpDesc" => query.OrderByDescending(x => x.Hp).ThenByDescending(x => x.EngineCode),
                    "TorqueAsc" => query.OrderBy(x => x.Torque),
                    "TorqueDesc" => query.OrderByDescending(x => x.Torque),
                    "VolumeAsc" => query.OrderBy(x => x.Volume),
                    "VolumeDesc" => query.OrderByDescending(x => x.Volume),
                    _ => query.OrderBy(x => x.EngineName)
                };
            }

            //paging
            result.TotalItems = query.Count();
            query = query.Skip((engineParams.Pagesize) * (engineParams.PageNumber - 1)).Take(engineParams.Pagesize);

            var list = await query.ToListAsync(); // the execution will be done at the end
            result.EngineDtos = _mapper.Map<List<EngineDto>>(list);
            result.PageItemCount = list.Count;
            return result;
        }
        public Engines GetByEngineCode(string enginecode) 
        {
            var engine = this.GetAll().FirstOrDefault(x => x.EngineCode == enginecode);
            return engine;
        }
    }
}
