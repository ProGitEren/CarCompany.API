using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Address;
using Infrastucture.Exceptions;
using Infrastucture.Extensions;
using Infrastucture.Interface.Repository_Interfaces;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Abstractions;
using WebAPI.Commands.UserCommands;

public class UpdateUserAddressCommandHandler : ICommandHandler<UpdateUserAddressCommand, AddressDto>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUnitOfWork _uow;
    private readonly Serilog.ILogger _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUsers> _userManager;

    public UpdateUserAddressCommandHandler(IHttpContextAccessor httpContextAccessor,
        IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, UserManager<AppUsers> userManager)
    {
        _contextAccessor = httpContextAccessor;
        _uow = uow;
        _mapper = mapper;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<Result<AddressDto>> Handle(UpdateUserAddressCommand request, CancellationToken cancellationToken)
    {
        var correlationId = _contextAccessor.GetCorrelationId();
        _logger.Information("Updating user address: {@Address}, CorrelationId: {CorrelationId}", request.Address, correlationId);

        var user = await _userManager.Users
            .Where(x => x.AddressId == request.Address.AddressId)
            .Include(x => x.Address)
            .FirstOrDefaultAsync(); // Find the user assigned to the address

        if (user == null)
        {
            _logger.Warning("User address could not be found for AddressId: {AddressId}, CorrelationId: {CorrelationId}", request.Address.AddressId, correlationId);
            return new Result<AddressDto>(new HttpResponseException(System.Net.HttpStatusCode.Unauthorized, "The user with this address could not be found."));
        }

        // Log the existing address before the update
        _logger.Information("Existing address before update: {@ExistingAddress}, CorrelationId: {CorrelationId}", user.Address, correlationId);

        _mapper.Map(request.Address, user.Address);

        await _uow.AddressRepository.UpdateAsync(user.Address);
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            _logger.Information("User address updated successfully to: {@UpdatedAddress}, CorrelationId: {CorrelationId}", user.Address, correlationId);
            return _mapper.Map<AddressDto>(user.Address);
        }

        _logger.Error("Problem updating user address for user {UserEmail}, CorrelationId: {CorrelationId}", user.Email, correlationId);
        return new Result<AddressDto>(new HttpResponseException(System.Net.HttpStatusCode.BadRequest, [$"Problem updating {user.Email}'s address."]));
    }
}