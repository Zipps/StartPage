using System.Collections.Immutable;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using StartPage.Framework;
using StartPage.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace StartPage.Services
{
    public interface IUserService
    {
        Task<bool> Authenticate(string username, string password);
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

        public async Task<bool> Authenticate(string username, string password)
        {
            var existingUser = await Get(username);
            if (existingUser == null) return false;

            if (!PasswordsMatch(existingUser, password)) return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, existingUser.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(GetTokenKey()), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            existingUser.Token = tokenHandler.WriteToken(token);
            await Update(existingUser);

            return true;
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

        public static byte[] GetTokenKey()
        {
            const string tokenSecretVariable = "STARTPAGE_TOKEN_SECRET";
            var secret = Environment.GetEnvironmentVariable(tokenSecretVariable);
            if (string.IsNullOrWhiteSpace(secret))
            {
                secret = "abcdefghijk123456789";
                Console.WriteLine($"Please set the {tokenSecretVariable} variable");
            }
            var key = Encoding.ASCII.GetBytes(secret);
            return key;
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
