using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.DTO.Dto_VehicleModels;
using Infrastucture.Errors;
using Infrastucture.Params;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Extensions
{
    public static class UserManagerExtensions
    {

        public static async Task<AppUsers> FindEmailByClaimWithDetailAsync(this UserManager<AppUsers> userManager, ClaimsPrincipal user)
        {
            var email = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;


            var appUser = await userManager.Users
            .Include(x => x.Address)
            .Include(x => x.Vehicles)
            .SingleOrDefaultAsync(x => x.Email == email);

            return appUser;

        }


        public static async Task<AppUsers> FindEmailByEmailAsync(this UserManager<AppUsers> userManager, string? Email)
        {
            var appUser = userManager.Users.Where(x => x.Email == Email).Include(x => x.Address).Include(x => x.Vehicles).SingleOrDefault();
            
            return appUser;

        }


        public static async Task<AppUsers> AddRolestoUserAsync(this UserManager<AppUsers> userManager, AppUsers user)
        {
            

                var roles = await userManager.GetRolesAsync(user);

                user.roles = roles;

            return user;
        }

        public static async Task<AppUsers> FindEmailByClaimAsync(this UserManager<AppUsers> userManager, ClaimsPrincipal user)
        {
            var email = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

            var appUser = await userManager.Users
            .SingleOrDefaultAsync(x => x.Email == email);

            return appUser;

        }

        public static async Task<IReadOnlyList<AppUsers?>> GetAllUsersAsync(this UserManager<AppUsers> userManager)
        {
            var users = await userManager.Users.ToListAsync() as IReadOnlyList<AppUsers>;

            return users;

        }

        public static async Task<IList<AppUsers?>> GetAllUsersWithDetailsAsync(this UserManager<AppUsers> userManager)
        {

            var users = await userManager.Users
                .Include(x => x.Address)
                .Include(x => x.Vehicles)
                .ToListAsync();


            return users;

        }


        public static async Task<IReadOnlyList<AppUsers?>> AddRolestoListAsync(this UserManager<AppUsers> userManager, IList<AppUsers> listusers)
        {
            foreach (var user in listusers)
            {

                var roles = await userManager.GetRolesAsync(user);

                user.roles = roles;

            }

            return listusers as IReadOnlyList<AppUsers?> ;
        }

        public static async Task<bool> IsPasswordUniqueAsync(this UserManager<AppUsers> userManager, string password, Serilog.ILogger logger, IPasswordHasher<AppUsers> passwordhasher, string? correlationId)
        {
            
            logger.Information("Checking if password is unique, CorrelationId: {CorrelationId}", correlationId);


            var users = await userManager.Users.ToListAsync();
            foreach (var user in users)
            {
                var verificationResult = passwordhasher.VerifyHashedPassword(user, user.PasswordHash, password);
                if (verificationResult == PasswordVerificationResult.Success)
                {
                    logger.Warning("Password is not unique, CorrelationId: {CorrelationId}", correlationId);
                    return false; // Password is not unique
                }
            }

            logger.Information("Password is unique, CorrelationId: {CorrelationId}", correlationId);
            return true;
        }

        public static async Task<ParamsUserDto> GetAllAsync(this UserManager<AppUsers> userManager,UserParams userParams, IMapper mapper)
        {
            var result = new ParamsUserDto();
            var query = userManager.Users
                .Include(x => x.Address)
                .Include(x => x.Vehicles)
                .AsNoTracking();


            //search by Name
            if (!string.IsNullOrEmpty(userParams.Search))
                query = query.Where(x =>
                string.Concat(x.FirstName,x.LastName).ToLower().Contains(userParams.Search) ||
                x.Phone.Contains(userParams.Search)
                );

            //filtering 

            if (!string.IsNullOrEmpty(userParams.Email))
                query = query.Where(x => x.Email == userParams.Email);
            if (!string.IsNullOrEmpty(userParams.Phone))
                query = query.Where(x => x.Phone == userParams.Phone);
            if (userParams.AddressId.HasValue)
                query = query.Where(x => x.AddressId == userParams.AddressId.Value);
            if (!string.IsNullOrEmpty(userParams.VehicleId))
                query = query.Where(x => x.Vehicles.Any(x =>x.Vin == userParams.VehicleId));



            //sorting
            if (!string.IsNullOrEmpty(userParams.Sorting))
            {
                query = userParams.Sorting switch
                {
                    "PhoneAsc" => query.OrderBy(x => int.Parse(x.Phone)),
                    "PhoneDesc" => query.OrderByDescending(x => int.Parse(x.Phone)),
                    "VehicleCountAsc" => query.OrderBy(x => x.Vehicles.Count()),
                    "VehicleCountDesc" => query.OrderByDescending(x => x.Vehicles.Count()),
                    _ => query.OrderBy(x => string.Concat(x.FirstName,x.LastName))
                };
            }

            //paging
            result.TotalItems = query.Count();
            query = query.Skip((userParams.Pagesize) * (userParams.PageNumber - 1)).Take(userParams.Pagesize);

            var list = await query.ToListAsync(); // the execution will be done at the end
            result.UserDtos = mapper.Map<List<UserwithdetailsDto>>(list);
            result.PageItemCount = list.Count;
            return result;
        }

    }
} 
