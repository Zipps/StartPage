using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StartPage.Models;

namespace StartPage.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookmarkController : ControllerBase
    {

        private readonly ILogger<BookmarkController> _logger;

        public BookmarkController(ILogger<BookmarkController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        public IEnumerable<Bookmark> Get()
        {
            return new List<Bookmark>
            {
                new Bookmark { Title = "Reddit", Url = "https://old.reddit.com", Id = Guid.NewGuid() },
                new Bookmark { Title = "Google", Url = "https://google.com", Id = Guid.NewGuid() }
            };
        }
    }
}