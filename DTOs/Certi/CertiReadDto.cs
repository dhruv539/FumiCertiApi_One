namespace FumicertiApi.DTOs.Certi
{
    public class CertiReadDto : CertiAddDto
    {
        public string CertiId { get; set; } = string.Empty;
        public string? CertiEditedUid { get; set; }
        public DateTime? CertiCreated { get; set; }
        public DateTime? CertiUpdated { get; set; }
    }
}
