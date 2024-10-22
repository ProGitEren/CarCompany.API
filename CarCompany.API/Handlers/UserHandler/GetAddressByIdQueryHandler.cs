using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Address;
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
    public class GetAddressByIdQueryHandler : IQueryHandler<GetAddressByIdQuery, AddressDto>
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IUnitOfWork _uow;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;

        public GetAddressByIdQueryHandler(IHttpContextAccessor httpContextAccessor,
            IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger)
        {
            _contextAccessor = httpContextAccessor;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<AddressDto>> Handle(GetAddressByIdQuery request, CancellationToken cancellationToken)
        {
            var correlationId = _contextAccessor.GetCorrelationId();
            _logger.Information("Retrieving user address by ID: {AddressId}, CorrelationId: {CorrelationId}", request.Id, correlationId);

            if (request.Id == Guid.Empty)
            {
                _logger.Warning("Invalid Address ID provided. CorrelationId: {CorrelationId}", correlationId);
                return new Result<AddressDto>(new HttpResponseException(System.Net.HttpStatusCode.NotFound, "The Address ID is null."));
            }

            var address = await _uow.AddressRepository.GetByIdAsync(request.Id);

            if (address != null)
            {
                _logger.Information("User address found for ID: {AddressId}, CorrelationId: {CorrelationId}", request.Id, correlationId);
                return _mapper.Map<AddressDto>(address);
            }

            _logger.Error("Error retrieving address with ID: {AddressId}, CorrelationId: {CorrelationId}", request.Id, correlationId);
            return new Result<AddressDto>(new HttpResponseException(System.Net.HttpStatusCode.BadRequest, ["Problem in retrieving address."]));
        }
    }
}