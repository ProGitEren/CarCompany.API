using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Address;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.Errors;
using Infrastucture.Exceptions;
using Infrastucture.Extensions;
using Infrastucture.Interface.Repository_Interfaces;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Abstractions;
using WebAPI.Queries.UserQueries;

namespace WebAPI.Handlers.UserHandler
{
    public class GetUserWithIdWithAddressQueryHandler : IQueryHandler<GetUserWithIdWithAddressQuery, UsernotokenDto>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _uow;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUsers> _userManager;

        public GetUserWithIdWithAddressQueryHandler(IHttpContextAccessor httpContextAccessor,
            IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, UserManager<AppUsers> userManager)
        {
            _contextAccessor = httpContextAccessor;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<Result<UsernotokenDto>> Handle(GetUserWithIdWithAddressQuery request, CancellationToken cancellationToken)
        {
            var correlationId = _contextAccessor.GetCorrelationId();
            _logger.Information("Retrieving user by Id {Id}, CorrelationId: {CorrelationId}", request.Id, correlationId);

            if (request.Id == null)
            {
                _logger.Warning("Retrieved Id is null, CorrelationId: {CorrelationId}", correlationId);
                return new Result<UsernotokenDto>(new HttpResponseException(System.Net.HttpStatusCode.BadRequest, ["The retrieved Id is null."]));
            }

            var user = await _userManager.Users
                .Where(x => x.Id == request.Id)
                .Include(y => y.Address)
                .FirstOrDefaultAsync();

            if (user != null)
            {
                var userdto = _mapper.Map<UsernotokenDto>(user);
                userdto.AddressDto = _mapper.Map<AddressDto>(user.Address);

                // Log user and address information
                _logger.Information("User retrieved by Id {Id}, Email: {Email}, Roles: {Roles}, Address: {@Address}, CorrelationId: {CorrelationId}",
                user.Id,
                user.Email,
                await _userManager.GetRolesAsync(user),  // Retrieves roles for the user
                user.Address,
                correlationId);

                return userdto;
            }

            _logger.Warning("User with Id {Id} could not be found, CorrelationId: {CorrelationId}", request.Id, correlationId);
            return new Result<UsernotokenDto>(new HttpResponseException(System.Net.HttpStatusCode.NotFound, "The User with this Id could not be found."));
        }
    }
}