using Infrastucture.DTO.Dto_Users;
using Models.Abstractions;

namespace WebAPI.Queries.UserQueries
{
    public class RegisterAdminQuery : IQuery<UserDto>
    {
        public RegisterDto Register { get; set; }

        public RegisterAdminQuery(RegisterDto register)
        {
            Register = register;
        }
    }
}
