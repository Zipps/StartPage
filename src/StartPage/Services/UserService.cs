using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using StartPage.Framework;
using StartPage.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace StartPage.Services
{
    public interface IUserService
    {
        bool Authenticate(User user, string enteredPassword);
        Task Create(User user);
        Task<User> Get(string username);
        Task<IEnumerable<User>> GetAll();
        Task Update(User user);
        Task Delete(string username);
    }

    public class UserService : IUserService
    {
        private readonly StartPageContext _context;
        
        public UserService(StartPageContext context)
        {
            _context = context;
        }

        public bool Authenticate(User user, string enteredPassword)
        {
            return PasswordsMatch(user, enteredPassword);
        }

        public async Task Create(User user)
        {
            user.Password = HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            user.Password = HashPassword(user.Password);

            var updatedUser = _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> Get(string username)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(user => user.Username == username);
            if (existingUser == null) return null;

            return existingUser;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var allUsers = new List<User>();
            await _context.Users.ForEachAsync(x => allUsers.Add(x));
            return allUsers;
        }

        public async Task Delete(string username)
        {
            var existingUser = await Get(username);
            if (existingUser == null) return;

            _context.Users.Remove(existingUser);
            
            await _context.SaveChangesAsync();
        }

        private const string SplitChar = "$";
        private static string HashPassword(string password, byte[] salt = null)
        {
            if (string.IsNullOrWhiteSpace(password)) return password;

            if (salt == null)
            {
                salt = new byte[128/8];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(salt);
                }
            }

            var hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256/8));

            return hashedPassword + SplitChar + Convert.ToBase64String(salt);
        }

        private static bool PasswordsMatch(User user, string enteredPassword)
        {
            var split = user.Password.Split(SplitChar, 2);
            var hashedUserPw = split[0];
            var hashedUserSalt = split[1];
            var hashedEnteredPw = HashPassword(enteredPassword, Convert.FromBase64String(hashedUserSalt));
            return user.Password == hashedEnteredPw;
        }
    }
}
