using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Sudoku.Api.Data;
using Sudoku.Api.Model;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Sudoku.Api.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;

        public RefreshTokenService(AppDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<RefreshToken> CreateRefreshTokenAsync(User user)
        {
            var expireMinutes = _config.GetSection("Jwt").GetValue<int>("RefreshExpireMinutes", 60*24*7); // default 7 days
            var token = new RefreshToken
            {
                UserId = user.Id,
                Token = GenerateTokenString(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(expireMinutes)
            };
            _db.RefreshTokens.Add(token);
            await _db.SaveChangesAsync();
            return token;
        }

        public async Task<RefreshToken?> ValidateRefreshTokenAsync(string token)
        {
            var t = await _db.RefreshTokens.Include(r=>r.User).FirstOrDefaultAsync(r => r.Token == token);
            if (t == null) return null;
            if (t.RevokedAt != null) return null;
            if (t.ExpiresAt < DateTime.UtcNow) return null;
            return t;
        }

        public async Task RevokeRefreshTokenAsync(RefreshToken token)
        {
            token.RevokedAt = DateTime.UtcNow;
            _db.RefreshTokens.Update(token);
            await _db.SaveChangesAsync();
        }

        private static string GenerateTokenString()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
