using System.ComponentModel.DataAnnotations;

namespace FumicertiApi.DTOs
{
    public class RegisterDto
    {
        [Required] public string UserEmail { get; set; }
        [Required] public string UserPassword { get; set; }
        [Required] public string UserName { get; set; }
        [Required] public int UserCompanyId { get; set; }
    }

}
