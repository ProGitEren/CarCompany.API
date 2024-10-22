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
using WebAPI.Queries.UserQueries;
using WebAPI.Validation;

public class RegisterQueryHandler : IQueryHandler<RegisterQuery, UserDto>
{
    private readonly EncryptionService _encryptionService;
    private readonly UserManager<AppUsers> _userManager;
    private readonly IMapper _mapper;
    private readonly Serilog.ILogger _logger;
    private readonly ITokenServices _tokenServices;
    private readonly IUnitOfWork _uow;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IPasswordHasher<AppUsers> _passwordHasher;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterQueryHandler(EncryptionService encryptionService, UserManager<AppUsers> userManager,
        IMapper mapper, Serilog.ILogger logger, ITokenServices tokenServices, IUnitOfWork uow, IHttpContextAccessor httpContextAccessor,
        IPasswordHasher<AppUsers> passwordHasher, RoleManager<IdentityRole> roleManager)
    {
        _contextAccessor = httpContextAccessor;
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _tokenServices = tokenServices;
        _uow = uow;
        _encryptionService = encryptionService;
        _passwordHasher = passwordHasher;
        _roleManager = roleManager;
    }

    public async Task<Result<UserDto>> Handle(RegisterQuery request, CancellationToken cancellationToken)
    {
        var correlationId = _contextAccessor.GetCorrelationId();
        var password = string.Empty;

        try
        {
            password = _encryptionService.Decrypt(request.Register.EncryptedPassword);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to decrypt password for user {Email}, CorrelationId: {CorrelationId}", request.Register.Email, correlationId);
            return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, [ex.Message]));
        }

        _logger.Information("Registration attempt for user {Email}, CorrelationId: {CorrelationId}", request.Register.Email, correlationId);

        if (await CheckEmailExist(request.Register.Email))
        {
            _logger.Warning("Registration failed for {Email}: Email already taken, CorrelationId: {CorrelationId}", request.Register.Email, correlationId);
            return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.BadRequest, ["This Email is already taken"]));
        }

        if (!await _userManager.IsPasswordUniqueAsync(password, _logger, _passwordHasher, correlationId))
        {
            _logger.Warning("Registration failed for {Email}: Password already in use, CorrelationId: {CorrelationId}", request.Register.Email, correlationId);
            return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.BadRequest, ["This password is already in use."]));
        }

        var passwordvalidationresult = PasswordValidation.ValidatePassword(password);

        if (!passwordvalidationresult.IsValid)
        {
            _logger.Warning("Password validation failed for {Email}, CorrelationId: {CorrelationId}", request.Register.Email, correlationId);
            return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.BadRequest, passwordvalidationresult.Errors));
        }

        var roleexists = await _roleManager.RoleExistsAsync(request.Register.Role);

        if (!roleexists)
        {
            _logger.Warning("Registration failed for {Role}: Role does not exist, CorrelationId: {CorrelationId}", request.Register.Role, correlationId);
            return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.BadRequest, [$"The role {request.Register.Role} does not exist in the system."]));
        }

        if (request.Register.Role.ToLower() == "admin")
        {
            _logger.Error("Unauthorized attempt to register as admin for {Email}, CorrelationId: {CorrelationId}", request.Register.Email, correlationId);
            return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.BadRequest, ["You are not authorized to register as Admin to the system."]));
        }

        var address = _mapper.Map<Address>(request.Register.Address);

        try
        {
            await _uow.AddressRepository.AddAsync(address);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to add address for {Email}, CorrelationId: {CorrelationId}", request.Register.Email, correlationId);
            return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, [ex.Message]));
        }

        var user = new AppUsers
        {
            FirstName = request.Register.FirstName,
            LastName = request.Register.LastName,
            Email = request.Register.Email,
            UserName = request.Register.Email,
            Phone = request.Register.Phone,
            birthtime = request.Register.birthtime,
            AddressId = address.AddressId,
        };

        try
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, request.Register.Role);
                _logger.Information("User {Email} registered successfully with role {Role}, CorrelationId: {CorrelationId}", request.Register.Email, request.Register.Role, correlationId);
                return new UserDto
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = await _tokenServices.CreateToken(user)
                };
            }
        }
        catch (Exception ex)
        {
            await _uow.AddressRepository.DeleteAsync(address.AddressId); // Roll back address creation if user registration fails
            _logger.Error(ex, "Failed to register user {Email}, CorrelationId: {CorrelationId}", request.Register.Email, correlationId);
            return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, [ex.Message]));
        }

        await _uow.AddressRepository.DeleteAsync(address.AddressId); // Roll back address creation if user registration fails
        _logger.Error("Unexpected error occurred during registration for {Email}, CorrelationId: {CorrelationId}", request.Register.Email, correlationId);

        return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.InternalServerError, ["Unexpected Error Happened."]));
    }

    private async Task<bool> CheckEmailExist(string email)
    {
        var correlationId = _contextAccessor.GetCorrelationId();
        var result = await _userManager.FindByEmailAsync(email);
        if (result != null)
        {
            _logger.Information("Email check for {Email}: exists, CorrelationId: {CorrelationId}", email, correlationId);
            return true;
        }

        _logger.Information("Email check for {Email}: does not exist, CorrelationId: {CorrelationId}", email, correlationId);
        return false;
    }
}