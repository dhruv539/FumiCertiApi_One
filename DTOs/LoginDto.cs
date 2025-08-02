using System.ComponentModel.DataAnnotations;

namespace FumicertiApi.DTOs
{
    public class LoginDto
    {
        [Required] public string UserEmail { get; set; }
        [Required] public string UserPassword { get; set; }
        public bool RememberMe { get; set; }

    }
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int? BranchId { get; set; }
        public string? UserId { get; set; }
        public string Email { get; set; } = string.Empty;
        public long tokenExp { get; set; }  
    }
}
