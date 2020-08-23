using System.Threading.Tasks;
using StartPage.Helpers;
using StartPage.Models;
using StartPage.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace StartPage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Policy = Policies.User)]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _service;
        public UserController(IConfiguration config, IUserService service)
        {
            _config = config;
            _service = service;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task Create([FromBody]User user)
        {
            if (!bool.Parse(_config["Settings:AllowSignup"])) return;

            await _service.Create(user);
        }

        [HttpPost]
        [Route("{username}")]
        public async Task Update([FromBody]User user)
        {
            await _service.Update(user);
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<object> Get(string username)
        {
            var user = await _service.Get(username);
            return user.WithoutSensitive();
        }

        [HttpDelete]
        [Route("{username}")]
        public async Task Delete(string username)
        {
            await _service.Delete(username);
        }
    }
}