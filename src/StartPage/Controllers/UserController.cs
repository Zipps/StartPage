using System.Threading.Tasks;
using StartPage.Helpers;
using StartPage.Models;
using StartPage.Services;
using Microsoft.AspNetCore.Mvc;

namespace StartPage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task Create([FromBody]User user)
        {
            await _service.Create(user);
        }

        [HttpPost]
        public async Task Update([FromBody]User user)
        {
            await _service.Update(user);
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<object> Get(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                var users = await _service.GetAll();
                foreach (var u in users) { u.Password = null; }
                return users.WithoutSensitive();
            }

            var user = await _service.Get(username);
            return user.WithoutSensitive();
        }

        [HttpDelete]
        public async Task Delete(string username)
        {
            await _service.Delete(username);
        }

        [HttpPost]
        public async Task Authenticate(string username, string password)
        {
            await _service.Authenticate(username, password);
        }

    }
}