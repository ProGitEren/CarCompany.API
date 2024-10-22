using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.Errors;
using Infrastucture.Exceptions;
using Infrastucture.Extensions;
using LanguageExt.Pipes;
using Microsoft.AspNetCore.Identity;
using Models.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.Net;
using WebAPI.Queries.UserQueries;
using LanguageExt.Common;

namespace WebAPI.Handlers.UserHandler
{
    public class GetCurrentUserHandler : IQueryHandler<GetCurrentUserQuery, UserwithdetailsDto>
    {
        private readonly UserManager<AppUsers> _userManager;
        private readonly IMapper _mapper;
        private readonly Serilog.ILogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public GetCurrentUserHandler(UserManager<AppUsers> userManager, IMapper mapper, Serilog.ILogger logger, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _contextAccessor = httpContextAccessor;
        }

        public async Task<Result<UserwithdetailsDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var correlationId = _contextAccessor.GetCorrelationId();
            _logger.Information("Retrieving current user, CorrelationId: {CorrelationId}", correlationId);

            var user = await _userManager.FindEmailByClaimWithDetailAsync(request.Claim);
            if (user == null)
            {
                _logger.Warning("Current user could not be found by claims, CorrelationId: {CorrelationId}", correlationId);
                return new Result<UserwithdetailsDto>(new HttpResponseException(HttpStatusCode.Unauthorized, "Current User could not be found by claims"));
            }

            await _userManager.AddRolestoUserAsync(user);

            var userdto = _mapper.Map<UserwithdetailsDto>(user);
            userdto.Token = request.Token;

            // Log detailed user information (UserId, Claims, Token) to the database
            _logger.Information("User ID: {UserId}, Claim Type: {ClaimType}, Claim Value: {ClaimValue}, Token: {Token}, CorrelationId: {CorrelationId}",
                user.Id,
                request.Claim.Type,
                request.Claim.Value,
                userdto.Token,
                correlationId);

            _logger.Information("Current user retrieved successfully, CorrelationId: {CorrelationId}", correlationId);
            return userdto;
        }
    }
}