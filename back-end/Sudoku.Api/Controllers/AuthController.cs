using Microsoft.AspNetCore.Mvc;
using Sudoku.Api.DTO;
using Sudoku.Api.Services;
using System.Threading.Tasks;

namespace Sudoku.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _users;
        private readonly ITokenService _tokens;
        private readonly IRefreshTokenService _refresh;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService users, ITokenService tokens, IRefreshTokenService refresh, ILogger<AuthController> logger)
        {
            _users = users;
            _tokens = tokens;
            _refresh = refresh;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(req?.Username) || string.IsNullOrWhiteSpace(req?.Password))
                    return BadRequest("Username and password are required.");

                var username = req.Username.Trim();
                // check if username already exists to provide clearer error
                var existing = await _users.GetByUsernameAsync(username);
                if (existing != null)
                {
                    return BadRequest("Username already taken.");
                }

                var user = await _users.CreateUserAsync(username, req.Password);
                if (user == null)
                {
                    _logger.LogWarning("CreateUserAsync returned null for username {Username}", username);
                    return BadRequest("Could not create user (check server logs).");
                }

                var token = _tokens.CreateToken(user);
                var rt = await _refresh.CreateRefreshTokenAsync(user);
                return Ok(new AuthResponse { Token = token, RefreshToken = rt.Token, Username = user.Username });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in Register");
                return StatusCode(500, "Server error during registration.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _users.ValidateUserAsync(req.Username?.Trim(), req.Password);
            if (user == null) return Unauthorized("Invalid username or password.");
            var token = _tokens.CreateToken(user);
            var rt = await _refresh.CreateRefreshTokenAsync(user);
            return Ok(new AuthResponse { Token = token, RefreshToken = rt.Token, Username = user.Username });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequest req)
        {
            var existing = await _refresh.ValidateRefreshTokenAsync(req.RefreshToken);
            if (existing == null) return Unauthorized("Invalid or expired refresh token.");
            // rotate: revoke old and issue a new one
            await _refresh.RevokeRefreshTokenAsync(existing);
            var user = existing.User;
            var access = _tokens.CreateToken(user);
            var newRt = await _refresh.CreateRefreshTokenAsync(user);
            return Ok(new AuthResponse { Token = access, RefreshToken = newRt.Token, Username = user.Username });
        }
    }
}
