using StartPage.Framework;
using StartPage.Models;

namespace StartPage.Tests.Factories
{
    public class UserFactory : FactoryBase
    {
        public User User { get; set; }

        public UserFactory(StartPageContext context) : base(context) 
        {
        }

        public UserFactory CreateUser()
        {
            User = new User
            {
                Username = RandomString(10),
                EmailAddress = $"{RandomString(10)}@{RandomString(20)}.com",
                Password = RandomString(30)
            };

            return this;
        }

        public UserFactory Save()
        {
            _context.Users.Add(User);
            _context.SaveChanges();

            return this;
        }
    }
}