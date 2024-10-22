using Infrastucture.DTO.Dto_Users;
using Models.Abstractions;
using System.Security.Claims;

namespace WebAPI.Commands.UserCommands
{
    public class UpdateUserCommand : ICommand<UserDto>
    {
        public UpdateUserDto UpdateUserDto { get; set; }
        
        public Claim? Claim { get; set; }

        public UpdateUserCommand(UpdateUserDto updateUserDto, Claim? claim)
        {
            UpdateUserDto = updateUserDto;
            Claim = claim;
        }
    }
}
