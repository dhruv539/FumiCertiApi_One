// FumicertiApi.Models.Invoice.cs
namespace FumicertiApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("invoice")]
    public class Invoice
    {
        [Key]
        [Column("inv_id")]
        public int InvId { get; set; }

        [Column("inv_type")]
        public string? InvType { get; set; }

        [Column("inv_date")]
        public DateTime? InvDate { get; set; }

        [Column("inv_no")]
        public int? InvNo { get; set; }

        [Column("inv_payment_mode")]
        public string? InvPaymentMode { get; set; }

        [Column("inv_total_qty")]
        public int? InvTotalQty { get; set; }

        [Column("inv_total_wt_gram")]
        public double? InvTotalWtGram { get; set; }

        [Column("inv_total_wt_kg")]
        public double? InvTotalWtKg { get; set; }

        [Column("inv_remarks")]
        public string? InvRemarks { get; set; }

        [Column("inv_supplier_id")]
        public int? InvSupplierId { get; set; }

        [Column("inv_total_amount")]
        public int? InvTotalAmount { get; set; }

        [Column("inv_address")]
        public string? InvAddress { get; set; }

        [Column("inv_mobileno")]
        public string? InvMobileno { get; set; }

        [Column("inv_gross_total")]
        public int? InvGrossTotal { get; set; }

        [Column("inv_sgst_amt")]
        public int? InvSgstAmt { get; set; }

        [Column("inv_cgst_amt")]
        public int? InvCgstAmt { get; set; }

        [Column("inv_igst_amt")]
        public int? InvIgstAmt { get; set; }

        [Column("inv_created")]
        public DateTime? InvCreated { get; set; }

        [Column("inv_updated")]
        public DateTime? InvUpdated { get; set; }

        [Column("inv_create_uid")]
        public string? InvCreateUid { get; set; }

        [Column("inv_edited_uid")]
        public string? InvEditedUid { get; set; }
        [Column("inv_prod_type")]
        public string? Invprodtype { get; set; }

        [Column("inv_company_id")]
        public int InvCompanyId { get; set; }


        // Don't serialize this, to avoid infinite loop
        [InverseProperty("Invoice")]
        public virtual List<InvoiceDetail>? InvoiceDetails { get; set; }
    }
}
