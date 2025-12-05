using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sudoku.Api.Data;
using Sudoku.Api.Model;
using System.Threading.Tasks;

namespace Sudoku.Api.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<User?> CreateUserAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return null;
            if (await _db.Users.AnyAsync(u => u.Username == username)) return null;
            var user = new User { Username = username };
            user.PasswordHash = _hasher.HashPassword(user, password);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;
            var res = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (res == PasswordVerificationResult.Success) return user;
            return null;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
