using Microsoft.AspNetCore.Http;
using System;
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

        public static User CurrentUser(this HttpContext context)
        {
            if (context.Items.TryGetValue("User", out var userObject))
            {
                return userObject as User;
            }

            return null;
        }

        public static bool IsCurrentUser(this HttpContext context, User user)
        {
            return context.CurrentUser()?.UserId == user?.UserId;
        }

        public static bool IsCurrentUser(this HttpContext context, Guid userId)
        {
            return context.CurrentUser()?.UserId == userId;
        }
    }
}