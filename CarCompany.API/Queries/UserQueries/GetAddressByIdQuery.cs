using Infrastucture.DTO.Dto_Address;
using Models.Abstractions;

namespace WebAPI.Queries.UserQueries
{
    public class GetAddressByIdQuery : IQuery<AddressDto>
    {
        public Guid? Id { get; set; }

        public GetAddressByIdQuery(Guid? id)
        {
            Id = id;
        }
    }
}
