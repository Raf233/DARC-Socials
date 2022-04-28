using Darc.Models.Domain.Users;
using Darc.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Darc.Services.Services 
{
    public class JwtService : IJwtService
    {
        private readonly string _secret;
        private readonly string _jwtIssuer;
        private readonly string _jwtAudience;

        public JwtService(IConfiguration configuration) {
            _secret = configuration.GetSection("Jwt").GetSection("Key").Value;
            _jwtIssuer = configuration.GetSection("Jwt").GetSection("Issuer").Value;
            _jwtAudience = configuration.GetSection("Jwt").GetSection("Audience").Value;
        }

        public string GenerateJwtToken( UserAuth user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString(), ClaimValueTypes.String),
                 new Claim(ClaimTypes.Name, user.Email, ClaimValueTypes.String)
            };

            var token = new JwtSecurityToken( _jwtIssuer,
                 _jwtIssuer,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
