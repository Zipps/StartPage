using System;

namespace StartPage.Models
{
    public class Bookmark
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
    }
}