using FumicertiApi.DTOs.Certi;

namespace FumicertiApi.DTOs
{
    public class PrintBillDto
    {
        public int BillId { get; set; }
        public string? BillNoStr { get; set; }
        public string? BillPrefix { get; set; }
        public string? BillSufix { get; set; }
        public DateTime BillDate { get; set; }
 
        public int? PartyId { get; set; }
        public string? ShipParty { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Address3 { get; set; }
        public string? State { get; set; }
        public string? Gstin { get; set; }
        public string? Pin { get; set; }
        public string? ContactNo { get; set; }

        public double GrossAmount { get; set; }
        public double TaxableAmount { get; set; }
        public double NetAmount { get; set; }
        public int BillPosId { get; set; }

        public float GstRate { get; set; }
        public float Sgst { get; set; }
        public float Cgst { get; set; }
        public float Igst { get; set; }

        public float RatePerContainer { get; set; }
        public float Rate40Container { get; set; }

        public string? SupplyType { get; set; }
        public string? IrnNo { get; set; }
        public string? AckNo { get; set; }
        public DateTime? AckDate { get; set; }

        public string? Remarks { get; set; }

        // Extra helper fields for printing
        public string? BillPeriod =>
            BillDateFrom.HasValue && BillDateTo.HasValue
                ? $"{BillDateFrom:dd/MM/yyyy} - {BillDateTo:dd/MM/yyyy}"
                : null;

        public DateTime? BillDateFrom { get; set; }
        public DateTime? BillDateTo { get; set; }

        public int BillCompanyId { get; set; }

        // ==== Company Info (header/footer in bill) ====
        public string? CompanyName { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyAddress { get; set; }
        public string? CompanyGstin { get; set; }
        public string? CompanyState { get; set; }
        public string? CompanyContactNo { get; set; }
        public string? CompanyEmail { get; set; }
        public string? Panno { get; set; }

        public string PosStateName { get; set; } = string.Empty;



        public List<Certiprintbillbto> Certis { get; set; } = new();
        public NotifyDto Notify { get; set; }



    }
}
