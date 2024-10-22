using Infrastucture.DTO.Dto_Users;
using Infrastucture.Helpers;
using Infrastucture.Params;
using MediatR;
using Models.Abstractions;

namespace WebAPI.Queries.UserQueries
{
    public class GetAllUsersQuery : IQuery<Pagination<UserwithdetailsDto>>
    {
        public UserParams UserParams { get; set; }

        public GetAllUsersQuery(UserParams userparams)
        {
            UserParams = userparams;
        }
    }
}
