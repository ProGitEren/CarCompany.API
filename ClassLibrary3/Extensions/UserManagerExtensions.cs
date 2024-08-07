using ClassLibrary2.Entities;
using Infrastucture.DTO;
using Infrastucture.Errors;
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
            .Include(x => x.Vehicle)
            .SingleOrDefaultAsync(x => x.Email == email);

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
                .Include(x => x.Vehicle)
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

            return listusers as IReadOnlyList<AppUsers> ;
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

    }
} 
