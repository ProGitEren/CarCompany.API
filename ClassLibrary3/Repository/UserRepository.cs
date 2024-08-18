using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.Data;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.Extensions;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Params;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Repository
{
    public class UserRepository : GenericRepository<AppUsers, string> , IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        public async Task<ParamsUserDto> GetUsersWithRoleAsync(UserManager<AppUsers> usermanager, UserParams userParams, IMapper mapper) 
        {
            var result = new ParamsUserDto();
            var query = await usermanager.GetAllAsync(userParams);

            // Apply Role filtering if specified
            if (!string.IsNullOrEmpty(userParams.Role))
            {
                var roleQuery = _context.UserRoles
                    .Join(_context.Roles,
                        userRole => userRole.RoleId,
                        role => role.Id,
                        (userRole, role) => new { userRole.UserId, RoleName = role.Name })
                    .Where(ur => ur.RoleName == userParams.Role)
                    .Select(ur => ur.UserId);

                query = query.Where(user => roleQuery.Contains(user.Id));
            }

            result.TotalItems = query.Count();

            var list = query.Skip((userParams.Pagesize) * (userParams.PageNumber-1)).Take(userParams.Pagesize).ToList();
            result.PageItemCount = list.Count;
            foreach (var user in list)
            {
                var userRoles = await usermanager.GetRolesAsync(user);
                user.roles = userRoles.ToList();  
            }

            result.UserDtos = mapper.Map<List<UserwithdetailsDto>>(list);

            return result;


        }

    }
}
