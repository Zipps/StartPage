using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StartPage.Models;
using StartPage.Services;

namespace StartPage.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = Policies.User)]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _service;
        private readonly ILogger<BookmarkController> _logger;

        public BookmarkController(IBookmarkService service, ILogger<BookmarkController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPut]
        public async Task Create([FromBody]Bookmark bookmark)
        {
            await _service.Create(bookmark);
        }

        [HttpPost]
        [Route("{id}")]
        public async Task Update(Guid id, [FromBody]Bookmark bookmark)
        {
            bookmark.BookmarkId = id;
            await _service.Update(bookmark);
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<object> Get(Guid id)
        {
            return await _service.Get(id);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(Guid id)
        {
            await _service.Delete(id);
        }
    }
}