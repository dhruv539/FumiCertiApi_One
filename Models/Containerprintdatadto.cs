using FumicertiApi.DTOs;
using FumicertiApi.DTOs.Container;

namespace FumicertiApi.Models
{
    public class CertiContainerReportDto
    {
        // Certi fields
        public int? CertiNo { get; set; }
        public DateTime? CertiDate { get; set; }
        public string? CertiType { get; set; }
        public string? CertiJobType { get; set; }
        public string? CertiProductType { get; set; }
        public DateTime? CertiFumidate { get; set; }
        public string? CertiFumiplace { get; set; }
        public double? CertiTemperature { get; set; }
        public string? CertiHumidity { get; set; }
        public double? CertiDoseRate { get; set; }
        public string? CertiRemarks { get; set; }
        public string? CertiFumiduration { get; set; }

        // Container
        public ContainerReadDto Container { get; set; } = new();

        // Company
        public CompanyDto Company { get; set; } = new();

        // Afo
        public AfoDto Afo { get; set; } = new();
    }

    public class AfoDto
    {
        public string? AfoMbrNo { get; set; }
        public string? AfoName { get; set; }
        public string? AfoAlpNo { get; set; }
    }

}
