using Sudoku.Api.Model;
using System.Threading.Tasks;

namespace Sudoku.Api.Services
{
    public interface IUserService
    {
        Task<User?> CreateUserAsync(string username, string password);
        Task<User?> ValidateUserAsync(string username, string password);
        Task<User?> GetByUsernameAsync(string username);
    }
}
