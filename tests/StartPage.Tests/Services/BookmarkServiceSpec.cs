using System;
using System.Linq;
using System.Threading.Tasks;
using StartPage.Framework;
using StartPage.Services;
using StartPage.Tests.Factories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace StartPage.Tests.Services
{
    [TestFixture]
    // [Parallelizable(ParallelScope.All)]
    public class BookmarkServiceSpec
    {
        private SqliteConnection _connection;
        private StartPageContext _context;
        private BookmarkFactory _factory;

        [OneTimeSetUp]
        public void SetUp()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<StartPageContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new StartPageContext(options);
            _factory = new BookmarkFactory(_context);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _connection.Close();
            _context.Dispose();
        }

        [Test]
        public async Task CreateBookmarkTest()
        {
            var bookmark = _factory.CreateBookmark().Bookmark;
            Assert.AreEqual(Guid.Empty, bookmark.BookmarkId);

            var service = new BookmarkService(_context);
            var createdBookmark = await service.Create(bookmark);
            Assert.NotNull(createdBookmark);
            Assert.AreNotEqual(Guid.Empty, createdBookmark.BookmarkId);
        }

        [Test]
        public async Task ReadBookmarkTest()
        {
            _factory.CreateBookmark().Save();

            var service = new BookmarkService(_context);
            var bookmark = await service.Get(_factory.Bookmark.BookmarkId);
            Assert.NotNull(bookmark);
            Assert.AreEqual(_factory.Bookmark.Title, bookmark.Title);
            Assert.AreEqual(_factory.Bookmark.ImageUrl, bookmark.ImageUrl);
            Assert.AreEqual(_factory.Bookmark.Url, bookmark.Url);
        }

        [Test]
        public async Task ReadAllBookmarksTest()
        {
            var createdBookmarks = new[] 
            {
                _factory.CreateBookmark().Save().Bookmark,
                _factory.CreateBookmark().Save().Bookmark
            };

            var service = new BookmarkService(_context);
            var bookmarks = await service.GetAll();
            foreach (var bk in createdBookmarks)
            {
                Assert.True(bookmarks.Any(x => x.Title == bk.Title));
            }
        }

        [Test]
        public async Task UpdateBookmarkTest()
        {
            var bookmark = _factory.CreateBookmark().Save().Bookmark;

            var newTitle = TestContext.CurrentContext.Random.GetString(20);
            var newUrl = TestContext.CurrentContext.Random.GetString(100);
            var newImageUrl = TestContext.CurrentContext.Random.GetString(100);

            bookmark.Title = newTitle;
            bookmark.Url = newUrl;
            bookmark.ImageUrl = newImageUrl;

            var service = new BookmarkService(_context);
            await service.Update(bookmark);

            var updatedBookmark = await service.Get(bookmark.BookmarkId);
            Assert.NotNull(updatedBookmark);
            Assert.AreEqual(newTitle, updatedBookmark.Title);
            Assert.AreEqual(newUrl, updatedBookmark.Url);
            Assert.AreEqual(newImageUrl, bookmark.ImageUrl);
        }
    }
}