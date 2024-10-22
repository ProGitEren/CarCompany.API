using Infrastucture.DTO.Dto_Users;
using Models.Abstractions;
using System.Security.Claims;

namespace WebAPI.Queries.UserQueries
{
    public class GetCurrentUserQuery : IQuery<UserwithdetailsDto>
    {
        public Claim? Claim { get; set; }

        public string Token { get; set; }

        public GetCurrentUserQuery(Claim? claim, string token)
        {
            Claim = claim;
            Token = token;  
        }
    }
}
