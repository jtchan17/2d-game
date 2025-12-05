using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sudoku.Api.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Sudoku.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(User user)
        {
            var jwtSection = _config.GetSection("Jwt");
            var secret = jwtSection.GetValue<string>("Key") ?? "please-change-this-secret";
            var issuer = jwtSection.GetValue<string>("Issuer") ?? "sudoku-api";
            var audience = jwtSection.GetValue<string>("Audience") ?? "sudoku-api-users";

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role ?? "User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(jwtSection.GetValue<int>("ExpireMinutes", 1440));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
