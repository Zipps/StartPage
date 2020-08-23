using System;
using System.Threading.Tasks;
using StartPage.Helpers;
using StartPage.Models;
using StartPage.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace StartPage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = Policies.User)]
    public class UserController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IUserService _service;
        public UserController(IOptions<AppSettings> appSettings, IUserService service)
        {
            _appSettings = appSettings.Value;
            _service = service;
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody]User user)
        {
            if (!_appSettings.AllowSignup)
            {
                return Forbid();
            }

            return Created("user", await _service.Create(user));
        }

        [HttpPost]
        [Route("{userId}")]
        public async Task<IActionResult> Update(Guid userId, [FromBody]User user)
        {
            if (!HttpContext.IsCurrentUser(userId))
            {
                return Forbid();
            }

            user.UserId = userId;
            return Ok(await _service.Update(user));
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> Get(Guid userId)
        {
            if (!HttpContext.IsCurrentUser(userId))
            {
                return Forbid();
            }

            var user = await _service.Get(userId);
            return Ok(user.WithoutSensitive());
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var user = await _service.Get(username);
            if (!HttpContext.IsCurrentUser(user.UserId))
            {
                return Forbid();
            }
            return Ok(user.WithoutSensitive());
        }

        [HttpDelete]
        [Route("{userId}")]
        public async Task<IActionResult> Delete(Guid userId)
        {
            if (!HttpContext.IsCurrentUser(userId))
            {
                return Forbid();
            }

            await _service.Delete(userId);
            return Ok();
        }

        [HttpGet]
        [Route("{username}/bookmarks")]
        public async Task<IActionResult> GetBookmarksByUser(Guid userId)
        {
            if (!HttpContext.IsCurrentUser(userId))
            {
                return Forbid();
            }

            var user = await _service.Get(userId);
            return Ok(user.Bookmarks);
        }
    }
}