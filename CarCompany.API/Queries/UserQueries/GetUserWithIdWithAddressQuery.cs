using Infrastucture.DTO.Dto_Users;
using Models.Abstractions;

namespace WebAPI.Queries.UserQueries
{
    public class GetUserWithIdWithAddressQuery : IQuery<UsernotokenDto>
    {
        public string? Id { get; set; }

        public GetUserWithIdWithAddressQuery(string? id)
        {
            Id = id;
        }
    }
}
