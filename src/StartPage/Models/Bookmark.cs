using System;

namespace StartPage.Models
{
    public class Bookmark
    {
        public Guid BookmarkId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}