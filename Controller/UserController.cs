using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO;
using Infrastucture.Errors;
using Infrastucture.Extensions;
using Infrastucture.Interface;
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

namespace WebApplication1.Controller
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

        public UserController(SignInManager<AppUsers> signinmanager, UserManager<AppUsers> usermanager, IMapper mapper,ITokenServices tokenServices, IUnitOfWork uow, IOptions<IdentityOptions> options)
        {
            _signinmanager = signinmanager;
            _usermanager = usermanager;
            _mapper = mapper;
            _tokenservices = tokenServices;
            _uow = uow;
            _identityoptions = options;
             
    }

        [HttpPost("Login")]

        public async Task<IActionResult> Login(LoginDto logindto) 
        {
            var user = await _usermanager.FindByEmailAsync(logindto.Email);

            if (user is null) return Unauthorized(new BaseCommonResponse(401));

           

            var result = await _signinmanager.CheckPasswordSignInAsync(user,logindto.Password, false);

            if (result.Succeeded)
            {
                await _usermanager.ResetAccessFailedCountAsync(user);
                return Ok(new UserDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Token = _tokenservices.CreateToken(user)
                });
            }

            await _usermanager.AccessFailedAsync(user); // This is the function which increases the attemp count therefore it is before the ıslockout

            if (await _usermanager.IsLockedOutAsync(user))
            {
                var lockoutEnd = await _usermanager.GetLockoutEndDateAsync(user);
                var timeRemaining = lockoutEnd.HasValue ? lockoutEnd.Value - DateTimeOffset.UtcNow : TimeSpan.Zero;

                // Format timeRemaining to show minutes and seconds
                var minutes = (int)timeRemaining.TotalMinutes;
                var seconds = timeRemaining.Seconds;

                var formattedTime = $"{minutes:D2}:{seconds:D2}";

               

                return Unauthorized(new BaseCommonResponse(401, $"Account is locked. Try again after {formattedTime} minutes."));
            }


            int attemptsLeft = _identityoptions.Value.Lockout.MaxFailedAccessAttempts - await _usermanager.GetAccessFailedCountAsync(user);
            return Unauthorized(new BaseCommonResponse(401, $"Invalid password. {attemptsLeft} attempts left."));
         
        }
        [HttpPost("check-email")]
        public async Task<ActionResult<bool>> CheckEmailExist([FromQuery] string email) 
        {
            var result = await _usermanager.FindByEmailAsync(email);
            if (result != null) 
            {
                return true;
            }   
            return false;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerdto) 
            {
            if (CheckEmailExist(registerdto.Email).Result.Value == true) 
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "This Email is already taken" }
                });
            }
            //var Address = _mapper.Map<Address>(registerdto.Address);
            //await _uow.AddressRepository.AddAsync(Address);



            // Check if the password is unique
            if (!await IsPasswordUnique(registerdto.Password))
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "This password is already in use." }
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var user = new AppUsers
            {
                
                FirstName = registerdto.FirstName,
                LastName = registerdto.LastName,
                Email = registerdto.Email,
                UserName = registerdto.Email,
                birthtime = registerdto.birthtime,
                Address = _mapper.Map<Address>(registerdto.Address),


            };

            var result = await _usermanager.CreateAsync(user, registerdto.Password);

            if (result.Succeeded)
            {
              

                return Ok(new UserDto
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = _tokenservices.CreateToken(user)
                });
            }

            return BadRequest(new ApiValidationErrorResponse
            {
                Errors = new[] { "Unexpected Error Hapoened." }

            });

        
        }

        [Authorize]
        [HttpGet("get-current-user")]

        public async Task<ActionResult> GetCurrentUser()
        {
            var user = await _usermanager.FindEmailByClaimAsync(User);

            if (user == null)
            {
                return NotFound(new ApiException(404, "The current user could not be found by claims."));
            }

            // Extract the token from the Authorization header
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            UserDto userdto = _mapper.Map<UserDto>(user);
            userdto.Token = token;


            return Ok(userdto);

        }

        [Authorize]
        [HttpGet("get-current-user-with-Address")]

        public async Task<ActionResult> GetCurrentUserWithAddress()
        {
            var user = await _usermanager.FindEmailByClaimAsyncWithAddress(User);

            if (user == null) { return NotFound(new ApiException(404, "The current user could not be found by claims.")); }

            // Extract the token from the Authorization header
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var userwithaddressdto = _mapper.Map<UserwithaddressDto>(user);

            userwithaddressdto.Token = token;

            return Ok(userwithaddressdto);

        }

        [Authorize]
        [HttpGet("get-user-Address")]

        public async Task<ActionResult> GetUserAddress()
        {
            var user = await _usermanager.FindEmailByClaimAsyncWithAddress(User);

            if (user == null) { return NotFound(new ApiException(404, "The current user could not be found by claims.")); }

            var useraddressdto = _mapper.Map<AddressDto>(user.Address);

            return Ok(useraddressdto);

        }

        [Authorize]
        [HttpPut("update-user-Address")]

        public async Task<ActionResult> UpdateUserAddress(AddressDto addressdto)
        {
          
            var user = await _usermanager.FindEmailByClaimAsyncWithAddress(User);
                
            if (user == null) { return NotFound(new ApiException(404, "The current user could not be found by claims.")); }

            await _uow.AddressRepository.UpdateAsync(_mapper.Map(addressdto,user.Address));

            //user.Address = _mapper.Map<Address>(addressdto);
            

            var result = await _usermanager.UpdateAsync(user);

            if (result.Succeeded) { return Ok(_mapper.Map<AddressDto>(user.Address)); }

         

            return new BadRequestObjectResult(new ApiValidationErrorResponse
            {
                Errors = new[] { $"Problem in updating this {User}'s Address." }
            });

        }

        [Authorize]
        [HttpGet("get-all-users")]

        public async Task<ActionResult> GetAll()
        {
            var users = await  _usermanager.GetAllUsers(); //Read Only List 


            // as we did not included the Address properties they wont be added
            
            if (users is not null) { return Ok(_mapper.Map<IReadOnlyList<UserDto>>(users)); }

            return new NotFoundObjectResult(new ApiException(404, "Any user could not be found in the application."));
            
        }

        [Authorize]
        [HttpGet("get-all-users-with-Address")]

        public async Task<ActionResult> GetAllWithAddress()
        {
            var users = await _usermanager.GetAllUsersWithAddress(); //Read Only List 


            // as we did not included the Address properties they wont be added

            if (users is not null) { return Ok(_mapper.Map<IReadOnlyList<UsernotokenDto>>(users)); }

            return new NotFoundObjectResult(new ApiException(404, "Any user could not be found in the application."));

        }

        [Authorize]
        [HttpGet("get-user-by-Id/{Id}")]

        public async Task<ActionResult> GetUserWithId(string? Id)
        {
            if (Id == null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "The retrieved AddressId is null." }
                });
            }

          
            var user = await _usermanager.FindByIdAsync(Id);

            // as we did not included the Address properties they wont be added

            if (user is not null) { return Ok(_mapper.Map<UserDto>(user)); }

            return new NotFoundObjectResult(new ApiException(404, "The User with this AddressId could not be found."));

        }

        [Authorize]
        [HttpGet("get-user-by-Id-with-Address/{Id}")]

        public async Task<ActionResult> GetUserWithIdWithAddress(string? Id)
        {
            if (Id == null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "The retrieved Id is null." }
                });
            }

            
            IQueryable<AppUsers> query = _usermanager.Users.Where(x => x.Id == Id).Include(y => y.Address);
            var user = await query.FirstOrDefaultAsync();

            //DTO

            var userdto = _mapper.Map<UserwithaddressDto>(user);
            var addressdto = _mapper.Map<AddressDto>(user.Address);
            userdto.AddressDto = addressdto;


            // as we did not included the Address properties they wont be added

            if (user is not null) { return Ok(userdto); }

            return new NotFoundObjectResult(new ApiException(404, "The User with this AddressId could not be found."));

        }

        [Authorize]
        [HttpGet("get-user-by-email/{email}")]

        public async Task<ActionResult> GetUserWithEmail(string? email)
        {
            if (email == null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "The retrieved email is null." }
                });
            }

            var user = await _usermanager.FindByEmailAsync(email);
            // as we did not included the Address properties they wont be added
            if (user is not null) { return Ok(_mapper.Map<UserDto>(user)); }

            return new NotFoundObjectResult(new ApiException(404, "The User with this email could not be found."));

        }

        [Authorize]
        [HttpGet("get-user-by-email-with-Address/{email}")]

        public async Task<ActionResult> GetUserWithEmailWithAddress(string? email)
        {
            if (email == null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "The retrieved email is null." }
                });
            }
            IQueryable<AppUsers> query = _usermanager.Users.Where(x => x.Email == email).Include(y => y.Address);
            var user = await query.FirstOrDefaultAsync();

            //DTO

            var userdto = _mapper.Map<UsernotokenDto>(user);
            
            

            if (user is not null) { return Ok(userdto); }

            return new NotFoundObjectResult(new ApiException(404, "The User with this AddressId could not be found."));

        }

        [Authorize]
        [HttpPut("update-user")]

        public async Task<ActionResult> UpdateUser(UpdateUserDto userdto)
        {

            var user = await _usermanager.FindEmailByClaimAsyncWithAddress(User);

            if (user == null) { return NotFound(new ApiException(404, "The current user could not be found by claims.")); }

            _mapper.Map(userdto, user);

            user.UserName = user.Email; // This isnt handled by the automapper

            var result = await _usermanager.UpdateAsync(user);

            if (result.Succeeded) { return Ok(_mapper.Map<UserDto>(userdto)); }

            return new BadRequestObjectResult(new ApiValidationErrorResponse
            {
                Errors = new[] { $"Problem in updating this {User}'s Address." }
            });

        }


        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(PasswordChangeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _usermanager.FindEmailByClaimAsync(User);
            if (user == null)
            {
                return Unauthorized(new ApiException(401, "You are not authorized"));
            }

          
            // Check if the password is unique
            if (!await IsPasswordUnique(dto.NewPassword))
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "This password is already in use." }
                });
            }


            if (dto.NewPassword != dto.ConfirmPassword)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "Confirmation Password do not match with new password." }
                });
            }

            
            var changePasswordResult = await _usermanager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "Problem in changing the password." }
                });
            }

            return Ok("Password changed successfully.");
        }


        [Authorize]
        [HttpDelete("delete-user/{id}")]

        public async Task<ActionResult> DeleteUser(string? id)
        {

            var user = await _usermanager.FindByIdAsync(id);

            if (user == null) { return NotFound(new ApiException(404, "The user with this id could not find.")); }

            var addressId = user.AddressId;

            var result = await _usermanager.DeleteAsync(user);   

            if (result.Succeeded) 
            {
                try
                {
                    await _uow.AddressRepository.DeleteAsync(addressId);
                    return Ok("The user and the address successfully deleted.");
                }
                catch (Exception ex) 
                {

                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[] { ex.Message }
                    });

                }
            
            }


            return new BadRequestObjectResult(new ApiValidationErrorResponse
            {
                Errors = new[] { $"Problem in deleting this {User}'s Address." }
            });

        }

        [Authorize]
        [HttpDelete("delete-user-address/{id}")]

        public async Task<ActionResult> DeleteUserAddress(string? id)
        {

            var user = await _usermanager.FindByIdAsync(id);

            if (user == null) { return NotFound(new ApiException(404, "The user with this id could not find.")); }

            var addressId = user.AddressId;

                try
                {
                    await _uow.AddressRepository.DeleteAsync(addressId);
                    return Ok("The address successfully deleted.");
                }
                catch (Exception ex)
                {

                    return new BadRequestObjectResult(new ApiValidationErrorResponse
                    {
                        Errors = new[] { ex.Message }
                    });

                
                }

        }

        [Authorize]
        [HttpPost("create-user-address/{id}")]

        public async Task<ActionResult> CreateUserAddress(string? id, AddressDto addressdto)
        {
            // Check if user's addressId is null

            var user = await _usermanager.FindByIdAsync(id);
            if (user == null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "The user with this id could not retrieved." }
                });

            }

            if (user.AddressId != null)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] { "The user already has existing address, if you want to change it go to the related section." }
                });

            }
            var address = _mapper.Map<Address>(addressdto);
            await _uow.AddressRepository.AddAsync(address);
            
            user.AddressId = address.AddressId;
            var result = await _usermanager.UpdateAsync(user);

            if (result.Succeeded) { return Ok(_mapper.Map<AddressDto>(user.Address)); }


            return new BadRequestObjectResult(new ApiValidationErrorResponse
            {
                Errors = new[] { "Problem in updating the user." }
            });

        }
            // Function for unique password 
            private async Task<bool> IsPasswordUnique(string password)
        {
            // Hash the password using the same hasher as the UserManager
            var passwordHasher = new PasswordHasher<AppUsers>();
            var users = await _usermanager.Users.ToListAsync();

            foreach (var user in users)
            {
                var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                if (verificationResult == PasswordVerificationResult.Success)
                {
                    return false; // Password is not unique
                }
            }

            return true; // Password is unique
        }

    }
}
