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




namespace CarCompany.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly SignInManager<AppUsers> _signinmanager;
        private readonly UserManager<AppUsers> _usermanager;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenservices;
        private readonly IUnitOfWork _uow;
        private readonly IOptions<IdentityOptions> _identityoptions;
        private readonly Serilog.ILogger _logger;
        private readonly IHttpContextAccessor _httpcontextAccessor;
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly IPasswordHasher<AppUsers> _passwordhasher;
        private readonly EncryptionService _encryptionservice;
        private readonly IVinGenerationService _vingenerationservice;

        public UserController(SignInManager<AppUsers> signinmanager, UserManager<AppUsers> usermanager, IMapper mapper,
            ITokenServices tokenServices, IUnitOfWork uow, IOptions<IdentityOptions> options,
            Serilog.ILogger logger, IHttpContextAccessor httpContextAccessor, RoleManager<IdentityRole> rolemanager,
            IPasswordHasher<AppUsers> passwordHasher,EncryptionService encryptionService, IVinGenerationService vinGenerationService)
        {
            _signinmanager = signinmanager;
            _usermanager = usermanager;
            _mapper = mapper;
            _tokenservices = tokenServices;
            _uow = uow;
            _identityoptions = options;
            _logger = logger;
            _httpcontextAccessor = httpContextAccessor;
            _rolemanager = rolemanager;
            _httpcontextAccessor.HttpContext = _httpcontextAccessor.HttpContext;
            _passwordhasher = passwordHasher;
            _encryptionservice = encryptionService;
            _vingenerationservice = vinGenerationService;
           
             
         }

        private string GetCorrelationId()
        {
            return _httpcontextAccessor.HttpContext?.TraceIdentifier;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto logindto)
        {
            var correlationId = GetCorrelationId();
            string password;

            try
            {
                // Try to decrypt the password
                password = _encryptionservice.Decrypt(logindto.EncryptedPassword);
                _logger.Information("Decrypting encrypted password for user {Email}, CorrelationId: {CorrelationId}", logindto.Email, correlationId);
            }
            catch
            {
                // If decryption fails, assume the password is not encrypted and encrypt it

                password = logindto.EncryptedPassword; // if the api administrator enters a normal password 
                _logger.Information("Using provided encrypted password for user {Email}, CorrelationId: {CorrelationId}", logindto.Email, correlationId);
            }



            var user = await _usermanager.FindByEmailAsync(logindto.Email);
            if (user is null)
            {
                _logger.Warning("Login failed for user {Email}: User not found, CorrelationId: {CorrelationId}", logindto.Email, correlationId);
                return Unauthorized(new ApiException(401,"The email could not found in the system."));
            }

            var result = await _signinmanager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                await _usermanager.ResetAccessFailedCountAsync(user);
                _logger.Information("User {Email} logged in successfully, CorrelationId: {CorrelationId}", logindto.Email, correlationId);
                return Ok(new UserDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Token = await _tokenservices.CreateToken(user)
                });
            }

            await _usermanager.AccessFailedAsync(user);

            if (await _usermanager.IsLockedOutAsync(user))
            {
                var lockoutEnd = await _usermanager.GetLockoutEndDateAsync(user);
                var timeRemaining = lockoutEnd.HasValue ? lockoutEnd.Value - DateTimeOffset.UtcNow : TimeSpan.Zero;
                var formattedTime = $"{(int)timeRemaining.TotalMinutes:D2}:{timeRemaining.Seconds:D2}";

                _logger.Warning("User {Email} account locked. Time remaining: {TimeRemaining}, CorrelationId: {CorrelationId}", logindto.Email, formattedTime, correlationId);
                return Unauthorized(new ApiException(401, $"Account is locked. Try again after {formattedTime} minutes."));
            }

            int attemptsLeft = _identityoptions.Value.Lockout.MaxFailedAccessAttempts - await _usermanager.GetAccessFailedCountAsync(user);
            _logger.Warning("Invalid password attempt for user {Email}. Attempts left: {AttemptsLeft}, CorrelationId: {CorrelationId}", logindto.Email, attemptsLeft, correlationId);
            return Unauthorized(new ApiException(401, $"Invalid password. {attemptsLeft} attempts left."));
        }

        [HttpPost("check-email")]
        public async Task<ActionResult<bool>> CheckEmailExist([FromQuery] string email)
        {
            var correlationId = GetCorrelationId();
            var result = await _usermanager.FindByEmailAsync(email);
            if (result != null)
            {
                _logger.Information("Email check for {Email}: exists, CorrelationId: {CorrelationId}", email, correlationId);
                return true;
            }

            _logger.Information("Email check for {Email}: does not exist, CorrelationId: {CorrelationId}", email, correlationId);
            return false;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerdto)
        {
            
            var correlationId = GetCorrelationId();
            var password = string.Empty;

            try
            {
                password = _encryptionservice.Decrypt(registerdto.EncryptedPassword);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            _logger.Information("Registration attempt for user {Email}, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);

            if ((await CheckEmailExist(registerdto.Email)).Value)
            {
                _logger.Warning("Registration failed for {Email}: Email already taken, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = ["This Email is already taken"]
                });
            }

            if (!await _usermanager.IsPasswordUniqueAsync(password, _logger, _passwordhasher, correlationId))
            {
                _logger.Warning("Registration failed for {Email}: Password already in use, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = ["This password is already in use."]
                });
            }

            var passwordvalidationresult =  PasswordValidation.ValidatePassword(password);

            if (!passwordvalidationresult.IsValid) 
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = passwordvalidationresult.Errors
                });

            }


            var roleexists = await _rolemanager.RoleExistsAsync(registerdto.Role);

            if (!roleexists) 
            {
                _logger.Warning("Registration failed for {Role}: Role does not exist, CorrelationId: {CorrelationId}", registerdto.Role, correlationId);
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = [$"The role {registerdto.Role} does not exist in the system."]
                });
            }

            // THIS IS VERY IMPORTANT AS THE HTML CONTEXT CAN BE CHANGED THE STRING SHOULD BE CHECKED HERE

            if (registerdto.Role.ToLower() == "admin")
            {
                _logger.Error("Registration failed for {Role}: Unauthorized user tried to register as ADMIN!!, CorrelationId: {CorrelationId}", registerdto.Role, correlationId);
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = [$"You are not authorized to register as Admin to the system."]
                });
            }

            var address = _mapper.Map<Address>(registerdto.Address);
            
            try
            {
                await _uow.AddressRepository.AddAsync(address);
            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            }

            var user = new AppUsers
            {
                FirstName = registerdto.FirstName,
                LastName = registerdto.LastName,
                Email = registerdto.Email,
                UserName = registerdto.Email,
                Phone = registerdto.Phone,
                birthtime = registerdto.birthtime,
                AddressId = address.AddressId,
            };

            var validationerrorlist = EntityValidator.GetValidationResults(user);

            if (validationerrorlist.Any())
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
            }

            try
            {
                var result = await _usermanager.CreateAsync(user, password);
                if (result.Succeeded)
                {

                    await _usermanager.AddToRoleAsync(user, registerdto.Role);
                    _logger.Information("User {Email} registered successfully with role {role}, CorrelationId: {CorrelationId}", registerdto.Email, registerdto.Role, correlationId);
                    return Ok(new UserDto
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Token = await _tokenservices.CreateToken(user)
                    });
                }
            }
            catch (Exception ex)
            {
                await _uow.AddressRepository.DeleteAsync(address.AddressId); // if the user registration failed address should be removed as well
                throw new Exception(ex.Message); // Internal Server Error
            }

            await _uow.AddressRepository.DeleteAsync(address.AddressId); // if the user registration failed address should be removed as well
            _logger.Error("Unexpected error occurred during registration for {Email}, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);
            return BadRequest(new ApiValidationErrorResponse
            {
                Errors = [$"Unexpected Error Happened: ."]
            });

        }

        // Later on this security will be increased only available for admin
        [HttpPost("register-admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RegisterAdminAsync(RegisterDto registerdto) 
        {
            var correlationId = GetCorrelationId();
            string password;

            try
            {
                // Try to decrypt the password
                password = _encryptionservice.Decrypt(registerdto.EncryptedPassword);
                _logger.Information("Using provided encrypted password for user {Email}, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);
            }
            catch
            {
                // If decryption fails, assume the password is not encrypted and encrypt it

                password = registerdto.EncryptedPassword; // if the api administrator enters a normal password 
                _logger.Information("Encrypting provided password for user {Email}, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);
            }

           
            _logger.Information("Registration attempt for user {Email}, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);

            if (CheckEmailExist(registerdto.Email).Result.Value)
            {
                _logger.Warning("Registration failed for {Email}: Email already taken, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = ["This Email is already taken"]
                });
            }
          
            if (!await _usermanager.IsPasswordUniqueAsync(password,_logger,_passwordhasher,correlationId))
            {
                _logger.Warning("Registration failed for {Email}: Password already in use, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = ["This password is already in use."]
                });
            }

            if (!ModelState.IsValid)
            {
                _logger.Warning("Registration failed for {Email}: Model state invalid, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);
                return BadRequest(ModelState);
            }



            var passwordvalidationresult = PasswordValidation.ValidatePassword(password);

            if (!passwordvalidationresult.IsValid)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = passwordvalidationresult.Errors
                });

            }

            var roleexists = await _rolemanager.RoleExistsAsync(registerdto.Role);

            if (!roleexists)
            {
                _logger.Warning("Registration failed for {Role}: Role does not exist, CorrelationId: {CorrelationId}", registerdto.Role, correlationId);
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = [$"The role {registerdto.Role} does not exist in the system."]
                });
            }

            // THIS IS VERY IMPORTANT AS THE HTML CONTEXT CAN BE CHANGED THE STRING SHOULD BE CHECKED HERE

            //if (registerdto.Role.ToLower() == "admin")
            //{
            //    _logger.Error("Registration failed for {Role}: Unauthorized user tried to register as ADMIN!!, CorrelationId: {CorrelationId}", registerdto.Role, correlationId);
            //    return new BadRequestObjectResult(new ApiValidationErrorResponse
            //    {
            //        Errors = new[] { $"You are not authorized to register as Admin to the system." }
            //    });
            //}

            var address = _mapper.Map<Address>(registerdto.Address);
            
            
            try
            {
                await _uow.AddressRepository.AddAsync(address);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            


            var user = new AppUsers
            {
                FirstName = registerdto.FirstName,
                LastName = registerdto.LastName,
                Email = registerdto.Email,
                UserName = registerdto.Email,
                Phone = registerdto.Phone,
                birthtime = registerdto.birthtime,
                AddressId = address.AddressId,
            };

            var validationerrorlist = EntityValidator.GetValidationResults(user);

            if (validationerrorlist.Any())
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
            }

            try
            {
                var result = await _usermanager.CreateAsync(user, password);
                if (result.Succeeded)
                {

                    await _usermanager.AddToRoleAsync(user, registerdto.Role);
                    _logger.Information("User {Email} registered successfully with role {role}, CorrelationId: {CorrelationId}", registerdto.Email, registerdto.Role, correlationId);
                    return Ok(new UserDto
                    {
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Token = await _tokenservices.CreateToken(user)
                    });
                }
            }
            catch (Exception ex)
            {
                await _uow.AddressRepository.DeleteAsync(address.AddressId); // if the user registration failed address should be removed as well
                throw new Exception(ex.Message); // Internal Server Error
            }

            await _uow.AddressRepository.DeleteAsync(address.AddressId); // if the user registration failed address should be removed as well
            _logger.Error("Unexpected error occurred during registration for {Email}, CorrelationId: {CorrelationId}", registerdto.Email, correlationId);
            return BadRequest(new ApiValidationErrorResponse
            {
                Errors = [$"Unexpected Error Happened: ."]
            });
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

        [Authorize]
        [HttpGet("get-current-user")]
        public async Task<ActionResult> GetCurrentUserAsync()
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Retrieving current user, CorrelationId: {CorrelationId}", correlationId);

            var user = await _usermanager.FindEmailByClaimAsync(User);
            if (user == null)
            {
                _logger.Warning("Current user could not be found by claims, CorrelationId: {CorrelationId}", correlationId);
                return Unauthorized(new ApiException(401, "The current user is not authenticated."));
            }

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            UserDto userdto = _mapper.Map<UserDto>(user);
            userdto.Token = token;

            _logger.Information("Current user retrieved successfully, CorrelationId: {CorrelationId}", correlationId);
            return Ok(userdto);
        }

        [Authorize]
        [HttpGet("get-current-user-with-Detail")]
        public async Task<ActionResult> GetCurrentUserWithDetaisAsync()
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Retrieving current user with address, CorrelationId: {CorrelationId}", correlationId);

            var user = await _usermanager.FindEmailByClaimWithDetailAsync(User);
            user = await _usermanager.AddRolestoUserAsync(user);

            if (user == null)
            {
                _logger.Warning("Current user with address could not be found by claims, CorrelationId: {CorrelationId}", correlationId);
                return Unauthorized(new ApiException(401, "The current user is not authenticated."));
            }

            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userwithaddressdto = _mapper.Map<UserwithdetailsDto>(user);
            userwithaddressdto.Token = token;

            _logger.Information("Current user with address retrieved successfully, CorrelationId: {CorrelationId}", correlationId);
            return Ok(userwithaddressdto);
        }

        [Authorize]
        [HttpGet("get-user-Address")]
        public async Task<ActionResult> GetUserAddress()
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Retrieving user address, CorrelationId: {CorrelationId}", correlationId);

            var user = await _usermanager.FindEmailByClaimWithDetailAsync(User);
            if (user == null)
            {
                _logger.Warning("User address could not be found by claims, CorrelationId: {CorrelationId}", correlationId);
                return Unauthorized(new ApiException(401, "The current user is not authenticated."));
            }

            var useraddressdto = _mapper.Map<AddressDto>(user.Address);

            _logger.Information("User address retrieved successfully, CorrelationId: {CorrelationId}", correlationId);
            return Ok(useraddressdto);
        }

        [Authorize]
        [HttpPut("update-user-Address")]
        public async Task<ActionResult> UpdateUserAddress(AddressDto addressdto)
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Updating user address, CorrelationId: {CorrelationId}", correlationId);

            var user = await _usermanager.Users.Where(x => x.AddressId == addressdto.AddressId).Include(x=> x.Address).FirstOrDefaultAsync(); // find the user assigned to the address
            if (user == null)
            {
                _logger.Warning("User address could not be found by claims, CorrelationId: {CorrelationId}", correlationId);
                return Unauthorized(new ApiException(401, "The current user is not authenticated."));
            }
            
            _mapper.Map(addressdto, user.Address);
            var validationerrorlist = EntityValidator.GetValidationResults(user.Address);
            if (validationerrorlist.Any())
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
            }


            await _uow.AddressRepository.UpdateAsync(user.Address);
            var result = await _usermanager.UpdateAsync(user);

            if (result.Succeeded)
            {
                _logger.Information("User address updated successfully, CorrelationId: {CorrelationId}", correlationId);
                return Ok(_mapper.Map<AddressDto>(user.Address));
            }

            _logger.Error("Problem in updating user address, CorrelationId: {CorrelationId}", correlationId);
            return new BadRequestObjectResult(new ApiValidationErrorResponse
            {
                Errors = [$"Problem in updating this {User}'s Address."]
            });
        }

       
        [Authorize]
        [HttpGet("get-Address-by-id/{Id}")]
        public async Task<ActionResult> GetAddressById(Guid Id)
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Updating user address, CorrelationId: {CorrelationId}", correlationId);

            if (Id == Guid.Empty)
            {
                return NotFound(new ApiException(404, "The address Id is null"));
            }

            var address = await _uow.AddressRepository.GetByIdAsync(Id);
            

            if (address != null)
            {
                _logger.Information("User address found, CorrelationId: {CorrelationId}", correlationId);
                return Ok(_mapper.Map<AddressDto>(address));
            }

            _logger.Error("Problem in retrieving user address, CorrelationId: {CorrelationId}", correlationId);
            return new BadRequestObjectResult(new ApiValidationErrorResponse
            {
                Errors = [$"Problem in retrieving address."]
            });
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-users")]
        public async Task<ActionResult> GetAllAsync()
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Retrieving all users, CorrelationId: {CorrelationId}", correlationId);

            var users = await _usermanager.GetAllUsersAsync();
            if (users != null)
            {
                _logger.Information("All users retrieved successfully, CorrelationId: {CorrelationId}", correlationId);
                return Ok(_mapper.Map<IReadOnlyList<UserDto>>(users));
            }

            _logger.Warning("No users found, CorrelationId: {CorrelationId}", correlationId);
            return new NotFoundObjectResult(new ApiException(404, "Any user could not be found in the application."));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("get-all-users-with-Detail")]
        public async Task<ActionResult> GetAllWithDetailsAsync()
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Retrieving all users with address, CorrelationId: {CorrelationId}", correlationId);

            var users = await _usermanager.GetAllUsersWithDetailsAsync();
            var userswithrole = await _usermanager.AddRolestoListAsync(users);
            
            if (users != null)
            {
                _logger.Information("All users with address retrieved successfully, CorrelationId: {CorrelationId}", correlationId);
                return Ok(_mapper.Map<IReadOnlyList<UsernotokenDto>>(users));
            }

            _logger.Warning("No users with address found, CorrelationId: {CorrelationId}", correlationId);
            return new NotFoundObjectResult(new ApiException(404, "Any user could not be found in the application."));
        }

        [Authorize]
        [HttpGet("get-user-by-Id/{Id}")]
        public async Task<ActionResult> GetUserWithId(string? Id)
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Retrieving user by Id {Id}, CorrelationId: {CorrelationId}", Id, correlationId);

            if (Id == null)
            {
                _logger.Warning("Retrieved Id is null, CorrelationId: {CorrelationId}", correlationId);
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = ["The retrieved Id is null."]
                });
            }

            var user = await _usermanager.FindByIdAsync(Id);
            if (user != null)
            {
                _logger.Information("User retrieved by Id {Id} successfully, CorrelationId: {CorrelationId}", Id, correlationId);
                return Ok(_mapper.Map<UserDto>(user));
            }

            _logger.Warning("User with Id {Id} could not be found, CorrelationId: {CorrelationId}", Id, correlationId);
            return new NotFoundObjectResult(new ApiException(404, "The User with this Id could not be found."));
        }

        [Authorize]
        [HttpGet("get-user-by-Id-with-Address/{Id}")]
        public async Task<ActionResult> GetUserWithIdWithAddress(string? Id)
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Retrieving user by Id with address {Id}, CorrelationId: {CorrelationId}", Id, correlationId);

            if (Id == null)
            {
                _logger.Warning("Retrieved Id is null, CorrelationId: {CorrelationId}", correlationId);
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = ["The retrieved Id is null."]
                });
            }

            var user = await _usermanager.Users.Where(x => x.Id == Id).Include(y => y.Address).FirstOrDefaultAsync();
            if (user != null)
            {
                var userdto = _mapper.Map<UsernotokenDto>(user);
                userdto.AddressDto = _mapper.Map<AddressDto>(user.Address);

                _logger.Information("User retrieved by Id with address {Id} successfully, CorrelationId: {CorrelationId}", Id, correlationId);
                return Ok(userdto);
            }

            _logger.Warning("User with Id {Id} could not be found, CorrelationId: {CorrelationId}", Id, correlationId);
            return new NotFoundObjectResult(new ApiException(404, "The User with this Id could not be found."));
        }

        [Authorize]
        [HttpGet("get-user-by-email/{email}")]
        public async Task<ActionResult> GetUserWithEmail(string? email)
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Retrieving user by email {Email}, CorrelationId: {CorrelationId}", email, correlationId);

            if (email == null)
            {
                _logger.Warning("Retrieved email is null, CorrelationId: {CorrelationId}", correlationId);
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = ["The retrieved email is null."]
                });
            }

            var user = await _usermanager.FindByEmailAsync(email);
            if (user != null)
            {
                _logger.Information("User retrieved by email {Email} successfully, CorrelationId: {CorrelationId}", email, correlationId);
                return Ok(_mapper.Map<UserDto>(user));
            }

            _logger.Warning("User with email {Email} could not be found, CorrelationId: {CorrelationId}", email, correlationId);
            return new NotFoundObjectResult(new ApiException(404, "The User with this email could not be found."));
        }

        [Authorize]
        [HttpGet("get-user-by-email-with-Detail/{email}")]
        public async Task<ActionResult> GetUserWithEmailWithDetail(string? email)
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Retrieving user by email with address {Email}, CorrelationId: {CorrelationId}", email, correlationId);

            if (email == null)
            {
                _logger.Warning("Retrieved email is null, CorrelationId: {CorrelationId}", correlationId);
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = ["The retrieved email is null."]
                });
            }

            var user = await _usermanager.Users.Where(x => x.Email == email).Include(y => y.Address).Include(x=> x.Vehicle).FirstOrDefaultAsync();
            user = await _usermanager.AddRolestoUserAsync(user);

            if (user != null)
            {
                var userdto = _mapper.Map<UsernotokenDto>(user);
                _logger.Information("User retrieved by email with address {Email} successfully, CorrelationId: {CorrelationId}", email, correlationId);
                return Ok(userdto);
            }

            _logger.Warning("User with email {Email} could not be found, CorrelationId: {CorrelationId}", email, correlationId);
            return new NotFoundObjectResult(new ApiException(404, "The User with this email could not be found."));
        }

        [Authorize]
        [HttpPut("update-user")]
        public async Task<ActionResult> UpdateUser(UpdateUserDto userdto)
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Updating user, CorrelationId: {CorrelationId}", correlationId);

            var user = await _usermanager.FindEmailByClaimAsync(User);
            //user = await _usermanager.AddRolestoUserAsync(user);
            if (user == null)
            {
                _logger.Warning("User could not be found by claims, CorrelationId: {CorrelationId}", correlationId);
                return NotFound(new ApiException(404, "The current user could not be found by claims."));
            }

            _mapper.Map(userdto, user);
            var validationerrorlist = EntityValidator.GetValidationResults(user);
            if (validationerrorlist.Any())
            {
                return BadRequest(new ApiValidationErrorResponse { Errors = validationerrorlist });
            }

            user.UserName = user.Email;

            var result = await _usermanager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // As the user details change the token should be change
                // Regenerate the token with updated claims
                var newToken = await  _tokenservices.CreateToken(user);

                var updateduser = _mapper.Map<UserDto>(user);
                updateduser.Token = newToken;

                _logger.Information("User updated successfully, CorrelationId: {CorrelationId}", correlationId);
                return Ok(updateduser);
            }

            _logger.Error("Problem in updating user, CorrelationId: {CorrelationId}", correlationId);
            return new BadRequestObjectResult(new ApiValidationErrorResponse
            {
                Errors = [$"Problem in updating this {User}'s Address."]
            });
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
        public async Task<ActionResult> DeleteUser(string? email)
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Deleting user {Email}, CorrelationId: {CorrelationId}", email, correlationId);

            var user = await _usermanager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.Warning("User with Email {Email} could not be found, CorrelationId: {CorrelationId}", email, correlationId);
                return NotFound(new ApiException(404, "The user with this id could not find."));
            }

            var addressId = user.AddressId;
            var vehicleId = user.VehicleId;
            var result = await _usermanager.DeleteAsync(user);

            if (result.Succeeded)
            {
                try
                {
                    await _uow.AddressRepository.DeleteAsync(addressId);
                    await _uow.VehicleRepository.DeleteAsync(vehicleId);
                    _logger.Information("User,vehicle and address deleted successfully, CorrelationId: {CorrelationId}", correlationId);
                    return Ok("The user and the address successfully deleted.");
                }
                catch (Exception ex)
                {
                    _logger.Error("Error deleting address for user {Email}, CorrelationId: {CorrelationId}. Exception: {Exception}", email, correlationId, ex);
                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = [ex.Message]
                    });
                }
            }

            _logger.Error("Problem in deleting user and address {Email}, CorrelationId: {CorrelationId}", email, correlationId);
            return new BadRequestObjectResult(new ApiValidationErrorResponse
            {
                Errors = 
                [$"Problem in deleting the user with email {email}'s "]
            });
        }

        [Authorize]
        [HttpDelete("delete-user-address/{id}")]
        public async Task<ActionResult> DeleteUserAddress(string? id)
        {
            var correlationId = GetCorrelationId();
            _logger.Information("Deleting address for user {Id}, CorrelationId: {CorrelationId}", id, correlationId);

            var user = await _usermanager.FindByIdAsync(id);
            if (user == null)
            {
                _logger.Warning("User with Id {Id} could not be found, CorrelationId: {CorrelationId}", id, correlationId);
                return NotFound(new ApiException(404, "The user with this id could not find."));
            }

            var addressId = user.AddressId;
            try
            {
                await _uow.AddressRepository.DeleteAsync(addressId);
                _logger.Information("Address deleted successfully for user {Id}, CorrelationId: {CorrelationId}", id, correlationId);
                return Ok("The address successfully deleted.");
            }
            catch (Exception ex)
            {
                _logger.Error("Error deleting address for user {Id}, CorrelationId: {CorrelationId}. Exception: {Exception}", id, correlationId, ex);
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = [ex.Message]
                });
            }
        }

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
