﻿using AuthenticationService.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using RegistrationService.Models;
using AuthenticationService.DBContext;

namespace AuthenticationService.Services
{
    public class JWTTokenImpl : IJWTTokenInterface
    {
        private readonly IConfiguration _configuration;
        private readonly AuthenticationDBContext _authenticationDBContext;
        public JWTTokenImpl(IConfiguration configuration, AuthenticationDBContext authenticationDBContext)
        {
            _configuration = configuration;
            _authenticationDBContext = authenticationDBContext;
        }
        public Token GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"].ToString());
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.Email) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Token { RefreshToken = tokenHandler.WriteToken(token), UserId = user.Id, UserName = user.UserName, UserType = user.UserType};
        }

        public List<User> GetAllUsers()
        {
            return _authenticationDBContext.Users.ToList();
        }
    }
}
