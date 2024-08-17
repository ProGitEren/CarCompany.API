using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.Data;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.DTO.Dto_Vehicles;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Params;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repository
{
    public class VehicleRepository :  GenericRepository<Vehicles,string?> , IVehicleRepository  
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public VehicleRepository(ApplicationDbContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ParamsVehicleDto> GetAllAsync(VehicleParams vehicleParams)
        {
            var result = new ParamsVehicleDto();
            var query = _context.Vehicles
                .Include(x => x.VehicleModel)
                .Include(x => x.User)
                .Include(x=> x.Engine)
                .AsNoTracking();

            //var userRolesQuery = from user in _context.Users
            //                     join userRole in _context.UserRoles on user.Id equals userRole.UserId
            //                     join role in _context.Roles on userRole.RoleId equals role.Id
            //                     select new { user.Id, role.Name };
           


            //search by Name
            if (!string.IsNullOrEmpty(vehicleParams.Search))
                query = query.Where(x => x.VehicleModel.ModelLongName.ToLower().Contains(vehicleParams.Search)
                || x.UserName.ToLower().Contains(vehicleParams.Search)
                || string.Concat(x.User.FirstName,x.User.LastName).ToLower().Contains(vehicleParams.Search));

            //filtering 
            if (vehicleParams.ModelId.HasValue)
                query = query.Where(x => x.ModelId == vehicleParams.ModelId.Value);

            if (vehicleParams.EngineId.HasValue)
                query = query.Where(x => x.EngineId == vehicleParams.EngineId.Value); 
            
            if (!string.IsNullOrEmpty(vehicleParams.VehicleId))
                query = query.Where(x => x.Vin == vehicleParams.VehicleId);

            if (!string.IsNullOrEmpty(vehicleParams.UserId))
                query = query.Where(x => x.UserId == vehicleParams.UserId);

            

            if (!string.IsNullOrEmpty(vehicleParams.Role))
            {
                var userRolesQuery = _context.Users
                               .Join(_context.UserRoles,
                                user => user.Id,
                                userRole => userRole.UserId,
                                (user, userRole) => new { user, userRole })
                               .Join(_context.Roles,
                                userRole => userRole.userRole.RoleId,
                                role => role.Id,
                                (userRole, role) => new { userRole.user.Id, RoleName = role.Name })
                               .Select(ur => new { ur.Id, ur.RoleName });
                var userIdsWithRole = userRolesQuery.Where(ur => ur.RoleName == vehicleParams.Role).Select(ur => ur.Id);
                query = query.Where(x => userIdsWithRole.Contains(x.UserId));
            }
            //if (engineParams.Man.HasValue)
            //    query = query.Where(x => x.ModelYear == engineParams.ModelYear.Value).AsQueryable();
            //LATER ON THE MANUFACTURER NAME WILL BE ADDED AND WILL BE FILTERED


            //sorting
            if (!string.IsNullOrEmpty(vehicleParams.Sorting))
            {
                query = vehicleParams.Sorting switch
                {
                    "PriceAsc" => query.OrderBy(x => x.VehicleModel.Price),
                    "PriceDesc" => query.OrderByDescending(x => x.VehicleModel.Price),
                    "YearAsc" => query.OrderBy(x => x.VehicleModel.ModelYear),
                    "YearDesc" => query.OrderByDescending(x => x.VehicleModel.ModelYear),
                    "DistanceAsc" => query.OrderBy(x => x.DrivenKM),
                    "DistanceDesc" => query.OrderByDescending(x => x.DrivenKM),
                    "FuelAsc" => query.OrderBy(x => (x.Averagefuelin+x.Averagefuelout)/2),
                    "FuelDesc" => query.OrderByDescending(x => (x.Averagefuelin + x.Averagefuelout) / 2),
                    _ => query.OrderBy(x => x.VehicleModel.ModelLongName)
                };
            }

            //paging
            result.TotalItems = query.Count();
            query = query.Skip((vehicleParams.Pagesize) * (vehicleParams.PageNumber - 1)).Take(vehicleParams.Pagesize);

            var list = await query.ToListAsync(); // the execution will be done at the end
            result.VehicleDtos = _mapper.Map<List<VehicleDto>>(list); 
            result.PageItemCount = list.Count;
            return result;
        }

    }
}
