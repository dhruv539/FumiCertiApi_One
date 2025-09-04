namespace FumicertiApi.DTOs
{
    public class VoucherConfigDto
    {
        public int VoucherConfig_Id { get; set; }
        public int VoucherConfig_FinYear_Id { get; set; }
        public int VoucherConfig_Branch_Id { get; set; }
        public string? BranchName { get; set; }   // <-- added
        public string VoucherConfig_ProdType { get; set; } = string.Empty;
        public string VoucherConfig_Prefix { get; set; } = string.Empty;
        public string VoucherConfig_Suffix { get; set; } = string.Empty;
        public int VoucherConfig_VoucherDigit { get; set; }
        public long VoucherConfig_LastVoucherNo { get; set; }
        public string? VoucherConfig_Remarks { get; set; }
        public int VoucherConfig_CompanyId { get; set; }
        public bool VoucherConfig_IsLock { get; set; }
        public string? VoucherConfig_Phyto { get; set; }
    }
}
