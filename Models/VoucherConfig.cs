using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FumicertiApi.Models

{
    [Table("voucherconfig")]
    public class VoucherConfig
    {
        [Key]
        [Column("voucherconfig_id")]
        public int VoucherConfig_Id { get; set; }
        [Column("voucherconfig_finyear_id")]
        public int VoucherConfig_FinYear_Id { get; set; }
        [Column("voucherconfig_branch_id")]
        public int VoucherConfig_Branch_Id { get; set; }
        [Column("voucherconfig_prodtype")]
        public string VoucherConfig_ProdType { get; set; } = string.Empty;
        [Column("voucherconfig_prefix")]
        public string VoucherConfig_Prefix { get; set; } = string.Empty;
        [Column("voucherconfig_suffix")]
        public string VoucherConfig_Suffix { get; set; } = string.Empty;
        [Column("voucherconfig_voucherdigit")]
        public int VoucherConfig_VoucherDigit { get; set; }
        [Column("voucherconfig_lastvoucherno")]
        public int VoucherConfig_LastVoucherNo { get; set; }
        [Column("voucherconfig_remarks")]
        public string? VoucherConfig_Remarks { get; set; }
        [Column("voucherconfig_compnayid")]
        public int VoucherConfig_CompnayId { get; set; }
        [Column("voucherconfig_islock")]
        public bool VoucherConfig_IsLock { get; set; }

        [Column("voucherconfig_phyto")]
        public string? VoucherConfig_Phyto { get; set; }

    }
}
