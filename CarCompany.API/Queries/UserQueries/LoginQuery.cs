using Infrastucture.DTO.Dto_Users;
using Models.Abstractions;

namespace WebAPI.Queries.UserQueries
{
    public class LoginQuery : IQuery<UserDto>
    {
        public LoginDto Login { get; set; }

        public LoginQuery(LoginDto dto)
        {
            Login = dto;
        }
    }
}
