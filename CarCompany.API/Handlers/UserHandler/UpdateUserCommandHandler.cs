using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.Exceptions;
using Infrastucture.Extensions;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
using LanguageExt.Common;
using Microsoft.AspNetCore.Identity;
using Models.Abstractions;
using WebAPI.Commands.UserCommands;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserDto>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUnitOfWork _uow;
    private readonly Serilog.ILogger _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<AppUsers> _userManager;
    private readonly ITokenServices _tokenServices;

    public UpdateUserCommandHandler(IHttpContextAccessor httpContextAccessor,
        IUnitOfWork uow, IMapper mapper, Serilog.ILogger logger, UserManager<AppUsers> userManager, ITokenServices tokenServices)
    {
        _contextAccessor = httpContextAccessor;
        _uow = uow;
        _mapper = mapper;
        _logger = logger;
        _userManager = userManager;
        _tokenServices = tokenServices;
    }

    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var correlationId = _contextAccessor.GetCorrelationId();
        _logger.Information("Updating user with Claim: {Claim}, CorrelationId: {CorrelationId}", request.Claim, correlationId);

        var user = await _userManager.FindEmailByClaimAsync(request.Claim);

        if (user == null)
        {
            _logger.Warning("User could not be found by Claim: {Claim}, CorrelationId: {CorrelationId}", request.Claim, correlationId);
            return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.NotFound, "The current user could not be found by claims."));
        }

        _logger.Information("User found for update. Email: {Email}, Roles: {@Roles}, CorrelationId: {CorrelationId}", user.Email, await _userManager.GetRolesAsync(user), correlationId);

        _mapper.Map(request.UpdateUserDto, user);
        user.UserName = user.Email;

        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
        {
            // Regenerate the token with updated claims
            var newToken = await _tokenServices.CreateToken(user);

            var updatedUser = _mapper.Map<UserDto>(user);
            updatedUser.Token = newToken;

            _logger.Information("User updated successfully. Email: {Email}, Roles: {@Roles}, CorrelationId: {CorrelationId}", user.Email, await _userManager.GetRolesAsync(user), correlationId);
            return updatedUser;
        }

        _logger.Error("Problem in updating user. Email: {Email}, Claim: {Claim}, CorrelationId: {CorrelationId}", user.Email, request.Claim, correlationId);
        return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.BadRequest, [$"Problem in updating user with Claim: {request.Claim}."]));
    }
}