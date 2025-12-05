using Sudoku.Api.Model;

namespace Sudoku.Api.Services
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
