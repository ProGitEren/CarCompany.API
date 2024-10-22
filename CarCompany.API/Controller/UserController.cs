using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.Errors;
using Infrastucture.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Serilog;
using Infrastucture.DTO.Infrastucture.DTO;
using WebAPI.Validation;
using Models.Entities;
using Infrastucture.Interface.Repository_Interfaces;
using Infrastucture.Interface.Service_Interfaces;
using Infrastucture.Repository;
using Infrastucture.Services;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.DTO.Dto_Address;
using Infrastucture.DTO.Dto_Vehicles;
using Infrastucture.DTO.Dto_Engines;
using Infrastucture.Helpers;
using Infrastucture.Params;
using FluentValidation;
using MediatR;
using WebAPI.Queries.UserQueries;
using LanguageExt.Common;
using ErrorOr;
using WebAPI.Commands.UserCommands;




namespace CarCompany.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        //private readonly SignInManager<AppUsers> _signinmanager;
        //private readonly UserManager<AppUsers> _usermanager;
        //private readonly IMapper _mapper;
        //private readonly ITokenServices _tokenservices;
        //private readonly IUnitOfWork _uow;
        //private readonly IOptions<IdentityOptions> _identityoptions;
        //private readonly Serilog.ILogger _logger;
        //private readonly IHttpContextAccessor _httpcontextAccessor;
        //private readonly RoleManager<IdentityRole> _rolemanager;
        //private readonly IPasswordHasher<AppUsers> _passwordhasher;
        //private readonly EncryptionService _encryptionservice;
        //private readonly IVinGenerationService _vingenerationservice;
        private readonly IMediator _mediator;
        
        public UserController(/*SignInManager<AppUsers> signinmanager, UserManager<AppUsers> usermanager, IMapper mapper,*/
        //    ITokenServices tokenServices, IUnitOfWork uow, IOptions<IdentityOptions> options,
        //    Serilog.ILogger logger, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> rolemanager,
        //    IPasswordHasher<AppUsers> passwordHasher,EncryptionService encryptionService, IVinGenerationService vinGenerationService,
            IMediator mediator
            )
        {
            //_signinmanager = signinmanager;
            //_usermanager = usermanager;
            //_mapper = mapper;
            //_tokenservices = tokenServices;
            //_uow = uow;
            //_identityoptions = options;
            //_logger = logger;
            //_httpcontextAccessor = httpContextAccessor;
            //_rolemanager = rolemanager;
            //_httpcontextAccessor.HttpContext = _httpcontextAccessor.HttpContext;
            //_passwordhasher = passwordHasher;
            //_encryptionservice = encryptionService;
            //_vingenerationservice = vinGenerationService;
            _mediator = mediator;
         }

       

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto logindto)
        {
            var query = new LoginQuery(logindto);
            var result = await _mediator.Send(query);

            return result.ToOk();
        }

       

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerdto)
        {
            var query = new RegisterQuery(registerdto);
            var result = await _mediator.Send(query);

            return result.ToOk();

        }

        

        [HttpPost("register-admin")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> RegisterAdminAsync(RegisterDto registerdto) 
        {
           var query = new RegisterAdminQuery(registerdto);
           var result = await _mediator.Send(query);

           return result.ToOk();
        }

        //This function will be only used in API
        //[HttpPost("register-admin-api")]
        //[Authorize(Roles ="Admin")]
        //public async Task<IActionResult> RegisterAdminApiAsync(RegisterApiDto registerdto)
        //{
        //    var correlationId = GetCorrelationId();

        //    _logger.Information("Registration attempt for user {Email}, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);

        //    if (CheckEmailExist(registerdto.Email).Result.Value)
        //    {
        //        _logger.Warning("Registration failed for {Email}: Email already taken, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);
        //        return new BadRequestObjectResult(new ApiValidationErrorResponse
        //        {
        //            Errors = new[] { "This Email is already taken" }
        //        });
        //    }

        //    if (!await IsPasswordUnique(registerdto.Password))
        //    {
        //        _logger.Warning("Registration failed for {Email}: Password already in use, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);
        //        return new BadRequestObjectResult(new ApiValidationErrorResponse
        //        {
        //            Errors = new[] { "This password is already in use." }
        //        });
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        _logger.Warning("Registration failed for {Email}: Model state invalid, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);
        //        return BadRequest(ModelState);
        //    }

        //    var roleexists = await _rolemanager.RoleExistsAsync(registerdto.Role);

        //    if (!roleexists)
        //    {
        //        _logger.Warning("Registration failed for {Role}: Role does not exist, CorrelationId: {CorrelationId}", registerdto.Role, correlationId);
        //        return new BadRequestObjectResult(new ApiValidationErrorResponse
        //        {
        //            Errors = new[] { $"The role {registerdto.Role} does not exist in the system." }
        //        });
        //    }

        //    // THIS IS VERY IMPORTANT AS THE HTML CONTEXT CAN BE CHANGED THE STRING SHOULD BE CHECKED HERE

        //    //if (registerdto.Role.ToLower() == "admin")
        //    //{
        //    //    _logger.Error("Registration failed for {Role}: Unauthorized user tried to register as ADMIN!!, CorrelationId: {CorrelationId}", registerdto.Role, correlationId);
        //    //    return new BadRequestObjectResult(new ApiValidationErrorResponse
        //    //    {
        //    //        Errors = new[] { $"You are not authorized to register as Admin to the system." }
        //    //    });
        //    //}

        //    var address = _mapper.Map<Address>(registerdto.Address);
        //    try
        //    {
        //        await _uow.AddressRepository.AddAsync(address);


        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }


        //    var user = new AppUsers
        //    {
        //        FirstName = registerdto.FirstName,
        //        LastName = registerdto.LastName,
        //        Email = registerdto.Email,
        //        UserName = registerdto.Email,
        //        birthtime = registerdto.birthtime,
        //        AddressId = address.AddressId,

        //    };

        //    var result = await _usermanager.CreateAsync(user, registerdto.Password);
        //    if (result.Succeeded)
        //    {

        //        await _usermanager.AddToRoleAsync(user, registerdto.Role);
        //        _logger.Information("User {Email} registered successfully with role {role}, CorrelationId: {CorrelationId}", registerdto.Email, registerdto.Role, correlationId);
        //        return Ok(new UserDto
        //        {
        //            Email = user.Email,
        //            FirstName = user.FirstName,
        //            LastName = user.LastName,
        //            Token = await _tokenservices.CreateToken(user)
        //        });
        //    }

        //    _logger.Error("Unexpected error occurred during registration for {Email}, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);
        //    return BadRequest(new ApiValidationErrorResponse
        //    {
        //        Errors = new[] { $"Unexpected Error Hapoened: {result.Errors}." }
        //    });
        //}

        [HttpGet("get-users")]
        [Authorize(Roles ="Admin")]

        public async Task<IActionResult> GetAllPaginated([FromQuery] UserParams userParams)
        {
            var query = new GetAllUsersQuery(userParams);
            var result = await _mediator.Send(query);

            return result.ToOk();                                                               
        }


        //[Authorize]
        //[HttpGet("get-current-user")]
        //public async Task<ActionResult> GetCurrentUserAsync()
        //{
        //    var correlationId = GetCorrelationId();
        //    _logger.Information("Retrieving current user, CorrelationId: {CorrelationId}", correlationId);

        //    var user = await _usermanager.FindEmailByClaimAsync(User);
        //    if (user == null)
        //    {
        //        _logger.Warning("Current user could not be found by claims, CorrelationId: {CorrelationId}", correlationId);
        //        return Unauthorized(new ApiException(401, "The current user is not authenticated."));
        //    }

        //    var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        //    UserDto userdto = _mapper.Map<UserDto>(user);
        //    userdto.Token = token;

        //    _logger.Information("Current user retrieved successfully, CorrelationId: {CorrelationId}", correlationId);
        //    return Ok(userdto);
        //}

        [Authorize]
        [HttpGet("get-current-user-with-Detail")]
        public async Task<IActionResult> GetCurrentUserWithDetaisAsync()
        {
            
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var claim = User.Claims?.Where(x => x.Type == ClaimTypes.Email).SingleOrDefault();
            var query = new GetCurrentUserQuery(claim, token);
            var result = await _mediator.Send(query);

            return result.ToOk();
           
        } 
                                                                                        
        //[Authorize]
        //[HttpGet("get-user-Address")]
        //public async Task<ActionResult> GetUserAddress()
        //{ 
        //    var correlationId = GetCorrelationId();
        //    _logger.Information("Retrieving user address, CorrelationId: {CorrelationId}", correlationId);

        //    var user = await _usermanager.FindEmailByClaimWithDetailAsync(User);
        //    if (user == null)
        //    {
        //        _logger.Warning("User address could not be found by claims, CorrelationId: {CorrelationId}", correlationId);
        //        return Unauthorized(new ApiException(401, "The current user is not authenticated."));
        //    }

        //    var useraddressdto = _mapper.Map<AddressDto>(user.Address);

        //    _logger.Information("User address retrieved successfully, CorrelationId: {CorrelationId}", correlationId);
        //    return Ok(useraddressdto);
        //}

        [Authorize]
        [HttpPut("update-user-Address")]
        public async Task<IActionResult> UpdateUserAddress(AddressDto addressdto)
        {
            var command = new UpdateUserAddressCommand(addressdto);
            var result = await _mediator.Send(command);

            return result.ToOk();
        }

       
        [Authorize]
        [HttpGet("get-Address-by-id/{Id}")]
        public async Task<IActionResult> GetAddressById(Guid Id)
        {
           var query = new GetAddressByIdQuery(Id);
           var result = await _mediator.Send(query);

           return result.ToOk();
        }


        //[Authorize(Roles = "Admin")]
        //[HttpGet("get-all-users")]
        //public async Task<ActionResult> GetAllAsync()
        //{
        //    var correlationId = GetCorrelationId();
        //    _logger.Information("Retrieving all users, CorrelationId: {CorrelationId}", correlationId);

        //    var users = await _usermanager.GetAllUsersAsync();
        //    if (users != null)
        //    {
        //        _logger.Information("All users retrieved successfully, CorrelationId: {CorrelationId}", correlationId);
        //        return Ok(_mapper.Map<IReadOnlyList<UserDto>>(users));
        //    }

        //    _logger.Warning("No users found, CorrelationId: {CorrelationId}", correlationId);
        //    return new NotFoundObjectResult(new ApiException(404, "Any user could not be found in the application."));
        //}



        //[Authorize(Roles = "Admin")]
        //[HttpGet("get-all-users-with-Detail")]
        //public async Task<ActionResult> GetAllWithDetailsAsync()
        //{
        //    var correlationId = GetCorrelationId();
        //    _logger.Information("Retrieving all users with address, CorrelationId: {CorrelationId}", correlationId);

        //    var users = await _usermanager.GetAllUsersWithDetailsAsync();
        //    var userswithrole = await _usermanager.AddRolestoListAsync(users);
            
        //    if (users != null)
        //    {
        //        _logger.Information("All users with address retrieved successfully, CorrelationId: {CorrelationId}", correlationId);
        //        return Ok(_mapper.Map<IReadOnlyList<UsernotokenDto>>(users));
        //    }

        //    _logger.Warning("No users with address found, CorrelationId: {CorrelationId}", correlationId);
        //    return new NotFoundObjectResult(new ApiException(404, "Any user could not be found in the application."));
        //}

        //[Authorize]
        //[HttpGet("get-user-by-Id/{Id}")]
        //public async Task<ActionResult> GetUserWithId(string? Id)
        //{
        //    var correlationId = GetCorrelationId();
        //    _logger.Information("Retrieving user by Id {Id}, CorrelationId: {CorrelationId}", Id, correlationId);

        //    if (Id == null)
        //    {
        //        _logger.Warning("Retrieved Id is null, CorrelationId: {CorrelationId}", correlationId);
        //        return new BadRequestObjectResult(new ApiValidationErrorResponse
        //        {
        //            Errors = ["The retrieved Id is null."]
        //        });
        //    }

        //    var user = await _usermanager.FindByIdAsync(Id);
        //    if (user != null)
        //    {
        //        _logger.Information("User retrieved by Id {Id} successfully, CorrelationId: {CorrelationId}", Id, correlationId);
        //        return Ok(_mapper.Map<UserDto>(user));
        //    }

        //    _logger.Warning("User with Id {Id} could not be found, CorrelationId: {CorrelationId}", Id, correlationId);
        //    return new NotFoundObjectResult(new ApiException(404, "The User with this Id could not be found."));
        //}

        [Authorize]
        [HttpGet("get-user-by-Id-with-Address/{Id}")]
        public async Task<IActionResult> GetUserWithIdWithAddress(string? Id)
        {
            var query = new GetUserWithIdWithAddressQuery(Id);
            var result = await _mediator.Send(query);

            return result.ToOk();
        }

        //[Authorize]
        //[HttpGet("get-user-by-email/{email}")]
        //public async Task<ActionResult> GetUserWithEmail(string? email)
        //{
        //    var correlationId = GetCorrelationId();
        //    _logger.Information("Retrieving user by email {Email}, CorrelationId: {CorrelationId}", email, correlationId);

        //    if (email == null)
        //    {
        //        _logger.Warning("Retrieved email is null, CorrelationId: {CorrelationId}", correlationId);
        //        return new BadRequestObjectResult(new ApiValidationErrorResponse
        //        {
        //            Errors = ["The retrieved email is null."]
        //        });
        //    }

        //    var user = await _usermanager.FindByEmailAsync(email);
        //    if (user != null)
        //    {
        //        _logger.Information("User retrieved by email {Email} successfully, CorrelationId: {CorrelationId}", email, correlationId);
        //        return Ok(_mapper.Map<UserDto>(user));
        //    }

        //    _logger.Warning("User with email {Email} could not be found, CorrelationId: {CorrelationId}", email, correlationId);
        //    return new NotFoundObjectResult(new ApiException(404, "The User with this email could not be found."));
        //}

        [Authorize]
        [HttpGet("get-user-by-email-with-Detail/{email}")]
        public async Task<IActionResult> GetUserWithEmailWithDetail(string? email)
        {
            var query = new GetUserWithEmailWithDetailQuery(email);
            var result = await _mediator.Send(query);

            return result.ToOk();
        }

        [Authorize]
        [HttpPut("update-user")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto userdto)
        {
            var claim = User.Claims?.Where(x => x.Type == ClaimTypes.Email).SingleOrDefault();
            var command = new UpdateUserCommand(userdto, claim);
            var result = await _mediator.Send(command);

            return result.ToOk();
        }

        //[HttpPut("change-password")]
        //public async Task<IActionResult> ChangePassword(PasswordChangeDto dto)
        //{
        //    var correlationId = GetCorrelationId();
        //    _logger.Information("Changing password, CorrelationId: {CorrelationId}", correlationId);

        //    if (!ModelState.IsValid)
        //    {
        //        _logger.Warning("Model state invalid for password change, CorrelationId: {CorrelationId}", correlationId);
        //        return BadRequest(ModelState);
        //    }

        //    var user = await _usermanager.FindEmailByClaimAsync(User);
        //    if (user == null)
        //    {
        //        _logger.Warning("User not authorized for password change, CorrelationId: {CorrelationId}", correlationId);
        //        return Unauthorized(new ApiException(401, "You are not authorized"));
        //    }

        //    if (!await _usermanager.IsPasswordUniqueAsync(dto.NewPassword, _logger, _passwordhasher, correlationId))
        //    {
        //        _logger.Warning("Registration failed : Password already in use, CorrelationId: {CorrelationId}", correlationId);
        //        return new BadRequestObjectResult(new ApiValidationErrorResponse
        //        {
        //            Errors = ["This password is already in use."]
        //        });
        //    }

        //    if (dto.NewPassword != dto.ConfirmPassword)
        //    {
        //        _logger.Warning("Password change failed: Confirmation password does not match new password, CorrelationId: {CorrelationId}", correlationId);
        //        return new BadRequestObjectResult(new ApiValidationErrorResponse
        //        {
        //            Errors = ["Confirmation Password do not match with new password."]
        //        });
        //    }

        //    var changePasswordResult = await _usermanager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
        //    if (!changePasswordResult.Succeeded)
        //    {
        //        _logger.Error("Problem in changing password, CorrelationId: {CorrelationId}", correlationId);
        //        return new BadRequestObjectResult(new ApiValidationErrorResponse
        //        {
        //            Errors = ["Problem in changing the password."]
        //        });
        //    }

        //    _logger.Information("Password changed successfully, CorrelationId: {CorrelationId}", correlationId);
        //    return Ok("Password changed successfully.");
        //}

        [Authorize]
        [HttpDelete("delete-user/{email}")]
        public async Task<IActionResult> DeleteUser(string? email)
        {
            var query = new DeleteUserQuery(email);
            var result = await _mediator.Send(query);

            return result.ToOk();
        }

        //[Authorize]
        //[HttpDelete("delete-user-address/{id}")]
        //public async Task<ActionResult> DeleteUserAddress(string? id)
        //{
        //    var correlationId = GetCorrelationId();
        //    _logger.Information("Deleting address for user {Id}, CorrelationId: {CorrelationId}", id, correlationId);

        //    var user = await _usermanager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        _logger.Warning("User with Id {Id} could not be found, CorrelationId: {CorrelationId}", id, correlationId);
        //        return NotFound(new ApiException(404, "The user with this id could not find."));
        //    }

        //    var addressId = user.AddressId;
        //    try
        //    {
        //        await _uow.AddressRepository.DeleteAsync(addressId);
        //        _logger.Information("Address deleted successfully for user {Id}, CorrelationId: {CorrelationId}", id, correlationId);
        //        return Ok("The address successfully deleted.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.Error("Error deleting address for user {Id}, CorrelationId: {CorrelationId}. Exception: {Exception}", id, correlationId, ex);
        //        return new BadRequestObjectResult(new ApiValidationErrorResponse
        //        {
        //            Errors = [ex.Message]
        //        });
        //    }
        //}

        //[Authorize]
        //[HttpPost("create-user-address/{id}")]
        //public async Task<ActionResult> CreateUserAddress(string? id, AddressDto addressdto)
        //{
        //    var correlationId = GetCorrelationId();
        //    _logger.Information("Creating address for user {Id}, CorrelationId: {CorrelationId}", id, correlationId);

        //    var user = await _usermanager.FindByIdAsync(id);
        //    if (user == null)
        //    {
        //        _logger.Warning("User with Id {Id} could not be found, CorrelationId: {CorrelationId}", id, correlationId);
        //        return new BadRequestObjectResult(new ApiValidationErrorResponse
        //        {
        //            Errors = ["The user with this id could not retrieved."]
        //        });
        //    }

        //    if (user.AddressId != null)
        //    {
        //        _logger.Warning("User with Id {Id} already has an address, CorrelationId: {CorrelationId}", id, correlationId);
        //        return new BadRequestObjectResult(new ApiValidationErrorResponse
        //        {
        //            Errors = ["The user already has an existing address, if you want to change it go to the related section."]
        //        });
        //    }

        //    var address = _mapper.Map<Address>(addressdto);
        //    await _uow.AddressRepository.AddAsync(address);

        //    user.AddressId = address.AddressId;
        //    var result = await _usermanager.UpdateAsync(user);

        //    if (result.Succeeded)
        //    {
        //        _logger.Information("Address created successfully for user {Id}, CorrelationId: {CorrelationId}", id, correlationId);
        //        return Ok(_mapper.Map<AddressDto>(user.Address));
        //    }

        //    _logger.Error("Problem in updating user with new address {Id}, CorrelationId: {CorrelationId}", id, correlationId);
        //    return new BadRequestObjectResult(new ApiValidationErrorResponse
        //    {
        //        Errors = ["Problem in updating the user."]
        //    });
        //}

       

    }
}
