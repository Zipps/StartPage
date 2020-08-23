using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using StartPage.Models;
using StartPage.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace StartPage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class LoginController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IUserService _service;
        public LoginController(IOptions<AppSettings> appSettings, IUserService service)
        {
            _appSettings = appSettings.Value;
            _service = service;
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]User authenticationRequest)
        {
            var user = await _service.Get(authenticationRequest.Username);
            if (user == null || !_service.Authenticate(user, authenticationRequest.Password))
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.SecretKey)), SecurityAlgorithms.HmacSha256);
            var claims = new Claim[] 
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                    new Claim("userId", user.UserId.ToString()),
                    new Claim("role", Policies.User),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var token = new JwtSecurityToken(
                issuer: _appSettings.Jwt.Issuer,
                audience: _appSettings.Jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    } 
}