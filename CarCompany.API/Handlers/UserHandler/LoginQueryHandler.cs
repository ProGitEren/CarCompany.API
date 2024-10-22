using AutoMapper;
using ClassLibrary2.Entities;
using Infrastucture.DTO.Dto_Users;
using Infrastucture.Errors;
using Infrastucture.Exceptions;
using Infrastucture.Extensions;
using Infrastucture.Interface.Service_Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Models.Abstractions;
using System.Runtime.CompilerServices;
using WebAPI.Queries.UserQueries;
using LanguageExt.Common;

namespace WebAPI.Handlers.UserHandler
{
    public class LoginQueryHandler : IQueryHandler<LoginQuery, UserDto>
    {
        private readonly EncryptionService _encryptionservice;
        private readonly Serilog.ILogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<AppUsers> _userManager;
        private readonly SignInManager<AppUsers> _signInManager;
        private readonly ITokenServices _tokenServices;
        private readonly IOptions<IdentityOptions> _identityoptions;


        public LoginQueryHandler(EncryptionService encryptionService, Serilog.ILogger logger, IHttpContextAccessor httpContextAccessor,
            UserManager<AppUsers> userManager, SignInManager<AppUsers> signInManager, ITokenServices tokenServices, IOptions<IdentityOptions> identityoptions)
        {
            _contextAccessor = httpContextAccessor;
            _encryptionservice = encryptionService;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;
            _identityoptions = identityoptions;
        }

        public async Task<Result<UserDto>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var correlationId = _contextAccessor.GetCorrelationId();
            string password;

            try
            {
                // Try to decrypt the password
                password = _encryptionservice.Decrypt(request.Login.EncryptedPassword);
                _logger.Information("Decrypting encrypted password for user {Email}, CorrelationId: {CorrelationId}", request.Login.Email, correlationId);
            }
            catch
            {
                // If decryption fails, assume the password is not encrypted and log accordingly
                password = request.Login.EncryptedPassword; // If API administrator enters a normal password
                _logger.Information("Using provided encrypted password for user {Email}, CorrelationId: {CorrelationId}", request.Login.Email, correlationId);
            }

            // Find the user by email
            var user = await _userManager.FindByEmailAsync(request.Login.Email);

            if (user is null)
            {
                _logger.Warning("Login failed for user {Email}: User not found, CorrelationId: {CorrelationId}", request.Login.Email, correlationId);
                return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.Unauthorized, $"Login failed for user {request.Login.Email}: User not found"));
            }

            // Check the password sign-in
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (result.Succeeded)
            {
                // Reset failed access count on successful login
                await _userManager.ResetAccessFailedCountAsync(user);
                _logger.Information("User {Email} logged in successfully, CorrelationId: {CorrelationId}", request.Login.Email, correlationId);

                return new UserDto
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Token = await _tokenServices.CreateToken(user)
                };
            }

            // Handle failed login attempt
            await _userManager.AccessFailedAsync(user);

            // Check if the user is locked out
            if (await _userManager.IsLockedOutAsync(user))
            {
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                var timeRemaining = lockoutEnd.HasValue ? lockoutEnd.Value - DateTimeOffset.UtcNow : TimeSpan.Zero;
                var formattedTime = $"{(int)timeRemaining.TotalMinutes:D2}:{timeRemaining.Seconds:D2}";

                _logger.Warning("User {Email} account locked. Time remaining: {TimeRemaining}, CorrelationId: {CorrelationId}", request.Login.Email, formattedTime, correlationId);
                return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.Unauthorized, $"Account is locked. Try again after {formattedTime} minutes."));
            }

            // Log invalid password attempt and return the result with attempts left
            int attemptsLeft = _identityoptions.Value.Lockout.MaxFailedAccessAttempts - await _userManager.GetAccessFailedCountAsync(user);
            _logger.Warning("Invalid password attempt for user {Email}. Attempts left: {AttemptsLeft}, CorrelationId: {CorrelationId}", request.Login.Email, attemptsLeft, correlationId);

            return new Result<UserDto>(new HttpResponseException(System.Net.HttpStatusCode.Unauthorized, $"Invalid password. {attemptsLeft} attempts left."));
        }
    }
}