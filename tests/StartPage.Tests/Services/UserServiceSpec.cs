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
    public class UserServiceSpec 
    {
        private SqliteConnection _connection;
        public StartPageContext _context;
        public UserFactory _factory;

        [OneTimeSetUp]
        public void SetUp()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<StartPageContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new StartPageContext(options);
            _factory = new UserFactory(_context);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _connection.Close();
            _context.Dispose();
        }

        [Test]
        public async Task CreateUserCreatesIdTest()
        {
            var user = _factory.CreateUser().User;
            Assert.AreEqual(Guid.Empty, user.UserId);

            var service = new UserService(_context);
            await service.Create(user);
            
            var readUser = service.Get(user.Username);
            Assert.NotNull(readUser);
            Assert.AreNotEqual(Guid.Empty, readUser.Id);
        }

        [Test]
        public async Task ReadUserTest()
        {
            _factory.CreateUser().Save();

            var service = new UserService(_context);
            var user = await service.Get(_factory.User.Username);
            Assert.NotNull(user);
            Assert.AreEqual(_factory.User.Username, user.Username);
            Assert.AreEqual(_factory.User.EmailAddress, user.EmailAddress);
            Assert.AreEqual(_factory.User.Password, user.Password);
        }

        [Test]
        public async Task ReadAllUsersTest()
        {
            _factory.CreateUser().Save();
            _factory.CreateUser().Save();

            var service = new UserService(_context);
            var users = await service.GetAll();
            Assert.GreaterOrEqual(users.Count(), 2);
        }

        [Test]
        public async Task UpdateUserTest()
        {
            var user = _factory.CreateUser().Save().User;

            var newUsername = TestContext.CurrentContext.Random.GetString(10);
            var newEmail = $"{TestContext.CurrentContext.Random.GetString(10)}@test.com";

            user.Username = newUsername;
            user.EmailAddress = newEmail;

            var service = new UserService(_context);
            await service.Update(user);

            var readUser = await service.Get(newUsername);
            Assert.NotNull(readUser);
            Assert.AreEqual(newEmail, readUser.EmailAddress);
        }

        [Test]
        public async Task UserPasswordIsHashedOnCreateTest()
        {
            var newUser = _factory.CreateUser().User;
            var password = TestContext.CurrentContext.Random.GetString(20);
            newUser.Password = password;
            var service = new UserService(_context);
            await service.Create(newUser);

            var user = await service.Get(newUser.Username);
            Assert.NotNull(user);
            Assert.AreNotEqual(password, user.Password);
        }

        [Test]
        public async Task UserPasswordIsHashedOnUpdateTest()
        {
            var user = _factory.CreateUser().Save().User;
            var password = TestContext.CurrentContext.Random.GetString(25);
            user.Password = password;
            
            var service = new UserService(_context);
            await service.Update(user);

            var readUser = await service.Get(user.Username);
            Assert.NotNull(readUser);
            Assert.AreNotEqual(password, readUser.Password);
        }

        [Test]
        public async Task UserDeleteTest()
        {
            var user = _factory.CreateUser().Save().User;

            var service = new UserService(_context);
            await service.Delete(user.UserId);

            var readUser = await service.Get(user.Username);
            Assert.IsNull(readUser);
        }

        [Test]
        public async Task UserAuthenticateTest()
        {
            var user = _factory.CreateUser().User;
            var password = TestContext.CurrentContext.Random.GetString(25);

            user.Password = password;
            
            var service = new UserService(_context);
            await service.Create(user);

            var readUser = await service.Get(user.Username);
            Assert.NotNull(readUser);

            var isAuthenticated = service.Authenticate(user, password);
            Assert.IsTrue(isAuthenticated);
        }

        [Test]
        public async Task UserWithWrongPasswordFailsAuthentication()
        {
            var user = _factory.CreateUser().User;
            var password = TestContext.CurrentContext.Random.GetString(20);

            user.Password = password;

            var service = new UserService(_context);
            await service.Create(user);

            var isAuthenticated = service.Authenticate(user, "wrong password");
            Assert.IsFalse(isAuthenticated);
        }
    }
}