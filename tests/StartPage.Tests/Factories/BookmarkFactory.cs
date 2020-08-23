using StartPage.Framework;
using StartPage.Models;

namespace StartPage.Tests.Factories
{
    public class BookmarkFactory : FactoryBase
    {
        public Bookmark Bookmark { get; set; }
        public UserFactory UserFactory { get; set; }

        public BookmarkFactory(StartPageContext context) : base(context)
        {
            UserFactory = new UserFactory(context);
            UserFactory.CreateUser().Save();
        }

        public BookmarkFactory CreateBookmark()
        {
            Bookmark = new Bookmark
            {
                Title = RandomString(30),
                Url = $"{RandomString(50)}.{RandomString(3)}",
                UserId = UserFactory.User.UserId
            };

            return this;
        }

        public BookmarkFactory Save()
        {
            _context.Bookmarks.Add(Bookmark);
            _context.SaveChanges();

            return this;
        }
    }
}