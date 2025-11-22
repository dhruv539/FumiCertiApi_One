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
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public long TokenExp { get; set; }  // Unix timestamp
        public DateTime RefreshTokenExp { get; set; }  // DateTime UTC
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public int? BranchId { get; set; }
        public string Rolename { get; set; }
    }

    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; }
    }

}
