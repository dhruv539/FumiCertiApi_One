﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using FumicertiApi.Models;
using Microsoft.Extensions.Configuration;

namespace FumicertiApi.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                 new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.UserEmail),
                new Claim(ClaimTypes.Role, user.UserRoleId),
              
        new Claim("CompanyId", user.UserCompanyId.ToString())

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
