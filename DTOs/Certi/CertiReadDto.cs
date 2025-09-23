using System.ComponentModel.DataAnnotations.Schema;

namespace FumicertiApi.DTOs.Certi
{
    public class CertiReadDto : CertiAddDto
    {
        public string? CertiId { get; set; } = string.Empty;

        public int CertiCompanyId { get; set; }

        public string? CertiEditedUid { get; set; }
        public DateTime? CertiCreated { get; set; }
        public DateTime? CertiUpdated { get; set; }
        public int CertiYearId { get; set; }
        public string? Certiprefix { get; set; }
         public string? Certisuffix { get; set; }


    }
}
