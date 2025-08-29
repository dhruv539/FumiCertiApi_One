using System.ComponentModel.DataAnnotations;

namespace FumicertiApi.DTOs
{
    public class AfoMemberDto
    {
        public int AfoId { get; set; }
        public string? AfoName { get; set; }
        public string? AfoMbrNo { get; set; }
        public string? AfoAlpNo { get; set; }
        public int AfoCompanyId { get; set; }
    }
    public class AfoMemberAddDto
    {

        [Required]
        public string? AfoName { get; set; }
        public string? AfoMbrNo { get; set; }
        public string? AfoAlpNo { get; set; }
        public int AfoCompanyId { get; set; }
    }
}
