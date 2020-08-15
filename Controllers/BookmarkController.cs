using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StartPage.Models;
using StartPage.Services;

namespace StartPage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookmarkController : ControllerBase
    {
        private readonly IBookmarkService _service;
        private readonly ILogger<BookmarkController> _logger;

        public BookmarkController(IBookmarkService service, ILogger<BookmarkController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpPost]
        public async Task Create([FromBody]Bookmark bookmark)
        {
            await _service.Create(bookmark);
        }

        [HttpPost]
        public async Task Update([FromBody]Bookmark bookmark)
        {
            await _service.Update(bookmark);
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<object> Get(string idString)
        {
            if (!Guid.TryParse(idString, out var id)) return null;
            return await _service.Get(id);
        }

        [HttpGet]
        public async Task<IEnumerable<Bookmark>> GetAll()
        {
            return await _service.GetAll();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task Delete(Guid id)
        {
            await _service.Delete(id);
        }
    }
}