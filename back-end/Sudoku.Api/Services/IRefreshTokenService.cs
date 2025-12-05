using Sudoku.Api.Model;
using System.Threading.Tasks;

namespace Sudoku.Api.Services
{
    public interface IRefreshTokenService
    {
        Task<RefreshToken> CreateRefreshTokenAsync(User user);
        Task<RefreshToken?> ValidateRefreshTokenAsync(string token);
        Task RevokeRefreshTokenAsync(RefreshToken token);
    }
}
