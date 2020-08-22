using System.Collections.Generic;
using System.Linq;
using StartPage.Models;

namespace StartPage.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<User> WithoutSensitive(this IEnumerable<User> users)
        {
            return users.Select(x => x.WithoutSensitive());
        }

        public static User WithoutSensitive(this User user)
        {
            user.Password = null;
            return user;
        }
    }
}