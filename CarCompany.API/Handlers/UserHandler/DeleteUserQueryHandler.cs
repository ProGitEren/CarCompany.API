using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.Errors;
using Infrastucture.Exceptions;
using Infrastucture.Extensions;
using Infrastucture.Interface.Repository_Interfaces;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;
using WebAPI.Queries.UserQueries;
using Models.Abstractions;

namespace WebAPI.Handlers.UserHandler
{
    public class DeleteUserQueryHandler : IQueryHandler<DeleteUserQuery, string>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _uow;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUsers> _userManager;

        public DeleteUserQueryHandler(IHttpContextAccessor httpContextAccessor,
            IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, UserManager<AppUsers> userManager)
        {
            _contextAccessor = httpContextAccessor;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<Result<string>> Handle(DeleteUserQuery request, CancellationToken cancellationToken)
        {
            var correlationId = _contextAccessor.GetCorrelationId();
            _logger.Information("Attempting to delete user with Email: {Email}, CorrelationId: {CorrelationId}", request.Email, correlationId);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.Warning("User with Email {Email} could not be found. CorrelationId: {CorrelationId}", request.Email, correlationId);
                return new Result<string>(new HttpResponseException(HttpStatusCode.NotFound, "The user with this email could not be found."));
            }

            var addressId = user.AddressId;
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                try
                {
                    await _uow.AddressRepository.DeleteAsync(addressId);
                    _logger.Information("User, vehicle, and address deleted successfully. CorrelationId: {CorrelationId}", correlationId);
                    return "The user and the address were successfully deleted.";
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while deleting the address for user {Email}. CorrelationId: {CorrelationId}", request.Email, correlationId);
                    return new Result<string>(new HttpResponseException(HttpStatusCode.BadRequest, [ex.Message]));
                }
            }

            _logger.Error("Failed to delete user and address. Email: {Email}, CorrelationId: {CorrelationId}", request.Email, correlationId);
            return new Result<string>(new HttpResponseException(HttpStatusCode.BadRequest, $"Problem in deleting the user with email {request.Email}."));
        }
    }
}