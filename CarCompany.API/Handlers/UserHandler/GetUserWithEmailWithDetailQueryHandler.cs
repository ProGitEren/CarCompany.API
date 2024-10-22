using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.Errors;
using Infrastucture.Exceptions;
using Infrastucture.Extensions;
using Infrastucture.Interface.Repository_Interfaces;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.Abstractions;
using WebAPI.Queries.UserQueries;

namespace WebAPI.Handlers.UserHandler
{
    public class GetUserWithEmailWithDetailQueryHandler : IQueryHandler<GetUserWithEmailWithDetailQuery, UsernotokenDto>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _uow;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUsers> _userManager;

        public GetUserWithEmailWithDetailQueryHandler(IHttpContextAccessor httpContextAccessor,
            IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, UserManager<AppUsers> userManager)
        {
            _contextAccessor = httpContextAccessor;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<Result<UsernotokenDto>> Handle(GetUserWithEmailWithDetailQuery request, CancellationToken cancellationToken)
        {
            var correlationId = _contextAccessor.GetCorrelationId();
            _logger.Information("Retrieving user by email {Email}, CorrelationId: {CorrelationId}", request.Email, correlationId);

            if (string.IsNullOrEmpty(request.Email))
            {
                _logger.Warning("Retrieved email is null or empty, CorrelationId: {CorrelationId}", correlationId);
                return new Result<UsernotokenDto>(new HttpResponseException(System.Net.HttpStatusCode.BadRequest, ["The retrieved email is null or empty."]));
            }

            var user = await _userManager.FindEmailByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.Warning("User with email {Email} could not be found, CorrelationId: {CorrelationId}", request.Email, correlationId);
                return new Result<UsernotokenDto>(new HttpResponseException(System.Net.HttpStatusCode.NotFound, "The user with this email could not be found."));
            }

            user = await _userManager.AddRolestoUserAsync(user);

            var userdto = _mapper.Map<UsernotokenDto>(user);

            // Log the relevant user information (UserId, Email, Roles, CorrelationId)
            var roles = await _userManager.GetRolesAsync(user);
            _logger.Information("User ID: {UserId}, Email: {Email}, Roles: {Roles}, CorrelationId: {CorrelationId}",
                user.Id,
                request.Email,
                string.Join(", ", roles).TrimEnd(','),
                correlationId);

            _logger.Information("User retrieved by email {Email} successfully, CorrelationId: {CorrelationId}", request.Email, correlationId);
            return userdto;
        }
    }
}