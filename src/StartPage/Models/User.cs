using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;

namespace StartPage.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }

        public List<Bookmark> Bookmarks { get; set; }
    }
}
