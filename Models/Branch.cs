using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FumicertiApi.Models
{
    [Table("branches")]
    public class Branch
    {
        [Key]
        [Column("branch_id")]
        public int BranchId { get; set; }

        [Required]
        [Column("branch_name")]
        public string BranchName { get; set; } = string.Empty;

        [Column("branch_gstin")]
        public string? Gstin { get; set; }

        [Column("branch_pan")]
        public string? Pan { get; set; }

        [Column("branch_printname")]
        public string? PrintName { get; set; }

        [Column("branch_address1")]
        public string? Address1 { get; set; }

        [Column("branch_address2")]
        public string? Address2 { get; set; }

        [Column("branch_address3")]
        public string? Address3 { get; set; }

        [Column("branch_pincode")]
        public string? Pincode { get; set; }

        [Column("branch_state_id")]
        public int StateId { get; set; }

        [Column("branch_state_code")]
        public string StateCode { get; set; } = string.Empty;

        [Column("branch_contact_no")]
        public string? ContactNo { get; set; }

        [Column("branch_email")]
        public string? Email { get; set; }

        [Column("branch_city")]
        public string? City { get; set; }

        [Column("branch_create_uid")]
        public string AddBy { get; set; } = string.Empty;

        [Column("branch_edited_uid")]
        public string? UpdateBy { get; set; }

        [Column("branch_updated")]
        public DateTime? Updated { get; set; }

        [Column("branch_created")]
        public DateTime? Created { get; set; }

        [Column("branch_company_id")]
        public string? CompanyId { get; set; }

        [Column("branch_state")]
        public string? State { get; set; }

        [Column("branch_country")]
        public string? Country { get; set; }

        [Column("branch_msmeno")]
        public string? MsmeNo { get; set; }

        [Column("branch_status")]
        public bool Status { get; set; } = true;
    }
}
