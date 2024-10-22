using Infrastucture.DTO.Dto_Users;
using Models.Abstractions;

namespace WebAPI.Queries.UserQueries
{
    public class GetUserWithEmailWithDetailQuery : IQuery<UsernotokenDto>
    {
        public string? Email { get; set; }

        public GetUserWithEmailWithDetailQuery(string? email)
        {
            Email = email;
        }
    }
}
