using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.Params;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Interface.Repository_Interfaces
{
    public interface IUserRepository : IGenericRepository<AppUsers, string>
    {
        Task<ParamsUserDto> GetUsersWithRoleAsync(UserManager<AppUsers> usermanager, UserParams userParams, IMapper mapper);
    }
}
