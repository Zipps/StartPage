using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using StartPage.Models;
using StartPage.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace StartPage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class LoginController
    {
        private readonly IConfiguration _config;
        private readonly IUserService _service;
        public LoginController(IConfiguration config, IUserService service)
        {
            _config = config;
            _service = service;
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<string> Authenticate([FromBody]User authenticationRequest)
        {
            var user = await _service.Get(authenticationRequest.Username);
            if (user == null) return null;

            if (!_service.Authenticate(user, authenticationRequest.Password)) return null;

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SecretKey"])), SecurityAlgorithms.HmacSha256);
            var claims = new Claim[] 
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                    new Claim("username", user.Username),
                    new Claim("role", Policies.User),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    } 
}