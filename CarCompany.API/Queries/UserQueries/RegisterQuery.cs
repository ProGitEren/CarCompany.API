using Infrastucture.DTO.Dto_Users;
using Models.Abstractions;

namespace WebAPI.Queries.UserQueries
{
    public class RegisterQuery :IQuery<UserDto>
    {
        public RegisterDto Register { get; set; }

        public RegisterQuery(RegisterDto register)
        {
            Register = register;
        }
    }
}
