using ClassLibrary2.Entities;
using Infrastucture.Interface.Service_Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.Services
{
    public class TokenServices : ITokenServices
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        private readonly UserManager<AppUsers> _usermanager;

        public TokenServices(IConfiguration config, UserManager<AppUsers> usermanager)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["token:key"]));
            _usermanager = usermanager;
        }

        public async Task<string> CreateToken(AppUsers appUsers)
        {
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var Claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Email, appUsers.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, appUsers.Email),


            };

            var roles = await _usermanager.GetRolesAsync(appUsers);
            Claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.Now.AddDays(10),
                Issuer = _config["token:issuer"],
                SigningCredentials = creds,

            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
