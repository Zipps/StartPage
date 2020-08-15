using System;
using System.Threading.Tasks;
using StartPage.Framework;
using StartPage.Tests.Factories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Nunit.Framework;

namespace StartPage.Tests.Services
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
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
        }
    }
}