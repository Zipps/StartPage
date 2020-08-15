using System;
using StartPage.Framework;

namespace StartPage.Tests.Factories
{
    public abstract class FactoryBase
    {
        protected StartPageContext _context;

        public FactoryBase(StartPageContext context)
        {
            _context = context;
        }

        internal string RandomString(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new String(stringChars);
        }
    }
}