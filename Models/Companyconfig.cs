using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FumicertiApi.Models
{
        [Table("company_config")]
        public class CompanyConfig
        {
            [Key]
            [Column("company_config_id")]
            public int CompanyConfigId { get; set; }

            [Column("company_config_companyid")]
            public int? CompanyId { get; set; }

            [Column("company_config_userroleid")]
            [StringLength(60)]
            public string? UserRoleId { get; set; }

            [Column("company_config_userid")]
            [StringLength(60)]
            public string? UserId { get; set; }

            [Column("company_config_afovisible")]
            public bool? AfoVisible { get; set; }

            [Column("company_config_branchvisible")]
            public bool? BranchVisible { get; set; }

            [Column("company_config_certimbrvisible")]
            public bool? CertImbrVisible { get; set; }

            [Column("company_config_certialpvisible")]
            public bool? CertIalpVisible { get; set; }

            [Column("company_config_certiafasvisible")]
            public bool? CertIafasVisible { get; set; }

            [Column("company_config_companyvisible")]
            public bool? CompanyVisible { get; set; }

            [Column("company_config_containerlistvisible")]
            public bool? ContainerListVisible { get; set; }

            [Column("company_config_indexvisible")]
            public bool? IndexVisible { get; set; }

            [Column("company_config_purchaseinvoicevisible")]
            public bool? PurchaseInvoiceVisible { get; set; }

            [Column("company_config_sellinvoicevisible")]
            public bool? SellInvoiceVisible { get; set; }

            [Column("company_config_locationvisible")]
            public bool? LocationVisible { get; set; }

            [Column("company_config_notifyvisible")]
            public bool? NotifyVisible { get; set; }

            [Column("company_config_productvisible")]
            public bool? ProductVisible { get; set; }

            [Column("company_config_uservisible")]
            public bool? UserVisible { get; set; }

            [Column("company_config_voucherconfigvisible")]
            public bool? VoucherConfigVisible { get; set; }

            [Column("company_config_yearvisible")]
            public bool? YearVisible { get; set; }

            [Column("company_config_allcertivisible")]
            public bool? AllCertiVisible { get; set; }

            [Column("company_config_create_uid")]
            [StringLength(60)]
            public string? CreateUid { get; set; }

            [Column("company_config_edited_uid")]
            [StringLength(60)]
            public string? EditedUid { get; set; }

            [Column("company_config_created")]
            public DateTime Created { get; set; } = DateTime.Now;

            [Column("company_config_updated")]
            public DateTime Updated { get; set; } = DateTime.Now;

            [Column("company_config_grouphomevisible")]
            public bool? GroupHomeVisible { get; set; }

            [Column("company_config_groupadminvisible")]
            public bool? GroupAdminVisible { get; set; }

            [Column("company_config_mastervisible")]
            public bool? MasterVisible { get; set; }

            [Column("company_config_voucherentryvisible")]
            public bool? VoucherEntryVisible { get; set; }

            [Column("company_config_certificateentryvisible")]
            public bool? CertificateEntryVisible { get; set; }

            [Column("company_config_reportvisible")]
            public bool? ReportVisible { get; set; }

            [Column("company_config_aboutvisible")]
            public bool? AboutVisible { get; set; }

        [Column("company_config_visible")]
        public bool? CompanyConfigVisible { get; set; }

    }

    public class CompanyConfigDto
    {
        public int CompanyConfigId { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public string? UserId { get; set; }
        public string? UserName { get; set; }

        public string? UserRoleId { get; set; }
        public string? UserRoleName { get; set; }


        public bool? AfoVisible { get; set; }
        public bool? BranchVisible { get; set; }
        public bool? CertImbrVisible { get; set; }
        public bool? CertIalpVisible { get; set; }
        public bool? CertIafasVisible { get; set; }
        public bool? CompanyVisible { get; set; }
        public bool? ContainerListVisible { get; set; }
        public bool? IndexVisible { get; set; }
        public bool? PurchaseInvoiceVisible { get; set; }
        public bool? SellInvoiceVisible { get; set; }
        public bool? LocationVisible { get; set; }
        public bool? NotifyVisible { get; set; }
        public bool? ProductVisible { get; set; }
        public bool? UserVisible { get; set; }
        public bool? VoucherConfigVisible { get; set; }
        public bool? YearVisible { get; set; }
        public bool? AllCertiVisible { get; set; }

        public bool? GroupHomeVisible { get; set; }
        public bool? GroupAdminVisible { get; set; }
        public bool? MasterVisible { get; set; }
        public bool? VoucherEntryVisible { get; set; }
        public bool? CertificateEntryVisible { get; set; }
        public bool? ReportVisible { get; set; }
        public bool? AboutVisible { get; set; }
        public bool? CompanyConfigVisible { get; set; }

    }

    public class CompanyConfigAddDto
    {
        public int? CompanyId { get; set; }
        public string? UserId { get; set; }
        public string? UserRoleId { get; set; }

        public bool? AfoVisible { get; set; }
        public bool? BranchVisible { get; set; }
        public bool? CertImbrVisible { get; set; }
        public bool? CertIalpVisible { get; set; }
        public bool? CertIafasVisible { get; set; }
        public bool? CompanyVisible { get; set; }
        public bool? ContainerListVisible { get; set; }
        public bool? IndexVisible { get; set; }
        public bool? PurchaseInvoiceVisible { get; set; }
        public bool? SellInvoiceVisible { get; set; }
        public bool? LocationVisible { get; set; }
        public bool? NotifyVisible { get; set; }
        public bool? ProductVisible { get; set; }
        public bool? UserVisible { get; set; }
        public bool? VoucherConfigVisible { get; set; }
        public bool? YearVisible { get; set; }
        public bool? AllCertiVisible { get; set; }

        public bool? GroupHomeVisible { get; set; }
        public bool? GroupAdminVisible { get; set; }
        public bool? MasterVisible { get; set; }
        public bool? VoucherEntryVisible { get; set; }
        public bool? CertificateEntryVisible { get; set; }
        public bool? ReportVisible { get; set; }
        public bool? AboutVisible { get; set; }
        public bool? CompanyConfigVisible { get; set; }

    }
}
