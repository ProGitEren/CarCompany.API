using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.Helpers;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Params;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Models.Abstractions;
using WebAPI.Queries.UserQueries;
using LanguageExt.Common;

namespace WebAPI.Handlers.UserHandler
{
    public class GetAllUsersHandler : IQueryHandler<GetAllUsersQuery, Pagination<UserwithdetailsDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUsers> _userManager;
        private readonly Serilog.ILogger _logger;

        public GetAllUsersHandler(IUnitOfWork uow, IMapper mapper, UserManager<AppUsers> userManager, Serilog.ILogger logger)
        {
            _mapper = mapper;
            _uow = uow;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result<Pagination<UserwithdetailsDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            _logger.Information("Fetching all users with pagination. Page: {PageNumber}, PageSize: {PageSize}", request.UserParams.PageNumber, request.UserParams.Pagesize);

            var src = await _uow.UserRepository.GetUsersWithRoleAsync(_userManager, request.UserParams, _mapper);
            var users = src.UserDtos.ToList() as IReadOnlyList<UserwithdetailsDto>;

            _logger.Information("Successfully fetched {UserCount} users.", users.Count);

            return new Pagination<UserwithdetailsDto>(request.UserParams.Pagesize, request.UserParams.PageNumber, src.PageItemCount, src.TotalItems, users);
        }
    }
}