using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FumicertiApi.Data;
using FumicertiApi.Models;

using FumicertiApi.Services;
using FumicertiApi.DTOs.User;
using FumicertiApi.DTOs;
using FumicertiApi.DTOs.Auth;

namespace FumicertiApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly TokenService _tokenService;

        public AuthController(AppDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // ✅ Login Endpoint
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserEmail == dto.UserEmail);

                if (user == null || !BCrypt.Net.BCrypt.Verify(dto.UserPassword, user.UserPassword))
                    return Unauthorized("Invalid credentials");

                var token = _tokenService.CreateToken(user);
                var tokenExpiry = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds();

                return Ok(new AuthResponseDto
                {
                    Token = token,
                    UserName = user.UserName,
                    Email = user.UserEmail,
                    //tokenExp = 60 * 60 * 24// Default to 1 day expiration
                    tokenExp = tokenExpiry,
                    BranchId=user.UserBranchId
                });
            }
            catch (Exception ex)
            {
                // Optional: log the full exception stack trace to a logging service or file
                Console.WriteLine(ex);

                // Return a proper error response
                return StatusCode(500, $"An unexpected error occurred while logging in -{ex}.");
            }
        }


        // ✅ Register (optional, if needed)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserAddDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.UserEmail == dto.UserEmail))
                return BadRequest("Email already exists.");

            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                UserRoleId = dto.UserRoleId,
                UserFirstName = dto.UserFirstName,
                UserLastName = dto.UserLastName,
                UserEmail = dto.UserEmail,
                UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.UserPassword),
                UserMobile = dto.UserMobile,
                UserName = dto.UserName,
                UserCreated = DateTime.UtcNow,
                UserStatus = 1
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully.");
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            // 🔍 1. Find user by email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == dto.Email);

            if (user == null)
                return NotFound("User not found");

            // 🔐 2. Generate unique reset token
            string token = Guid.NewGuid().ToString();

            // 🕒 3. Set token + expiry
            user.PasswordResetToken = token;
            user.ResetTokenExpires = DateTime.UtcNow.AddMinutes(15); // Token valid for 15 min

            await _context.SaveChangesAsync();

            // 📎 4. Generate reset link (to send by email or return to frontend)
            string resetLink = $"http://localhost:4200/reset-password?email={dto.Email}&token={token}";

            // ✅ 5. Return reset link for testing (you can send it via email in production)
            return Ok(new
            {
                message = "Password reset link generated successfully.",
                resetToken = token,
                resetLink = resetLink
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            // 🔍 1. Find user by email and token
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.UserEmail == dto.Email &&
                u.PasswordResetToken == dto.Token);

            if (user == null)
                return NotFound("Invalid email or token.");

            // 🕒 2. Check if token is expired
            if (user.ResetTokenExpires == null || user.ResetTokenExpires < DateTime.UtcNow)
                return BadRequest("Reset token has expired.");

            // 🔐 3. Update password (⚠️ hash in production)
            user.UserPassword = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            // 🧹 4. Clear token fields
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok("Password reset successfully.");
        }

    }
}
