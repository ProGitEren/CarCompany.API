using Infrastucture.DTO.Dto_Address;
using Models.Abstractions;
using System.Security.Claims;

namespace WebAPI.Commands.UserCommands
{
    public class UpdateUserAddressCommand : ICommand<AddressDto>
    {
        public AddressDto Address { get; set; }
        public UpdateUserAddressCommand(AddressDto address)
        {
            Address = address;
        }
    }
}
