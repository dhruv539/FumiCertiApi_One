namespace FumicertiApi.DTOs.Certi
{
    public class Certiprintbillbto
    {
        public int? CertiNo { get; set; }
        public DateTime? CertiDate { get; set; }
        public string ContainerNo { get; set; }
        public bool? Commodity { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDischarge { get; set; }
        public int? ContainerSize { get; set; }
        public double? DoseRate { get; set; }
        public string? InvoiceNo { get; set; }

    }
}
