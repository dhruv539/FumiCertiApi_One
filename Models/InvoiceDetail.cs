namespace FumicertiApi.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

        [Table("invoice_detail")]
        public class InvoiceDetail
        {
            [Key]
            [Column("invoicedetail_id")]
            public int InvoiceDetailId { get; set; }

            [Column("invoicedetail_product_id")]
            public int? InvoiceDetailProductId { get; set; }

            [Column("invoicedetail_no_of_box")]
            public int? InvoiceDetailNoOfBox { get; set; }

            [Column("invoicedetail_qty_kg")]
            public double? InvoiceDetailQtyKg { get; set; }

            [Column("invoicedetail_qty_no")]
            public int? InvoiceDetailQtyNo { get; set; }

            [Column("invoicedetail_batch_no")]
            public string? InvoiceDetailBatchNo { get; set; }

            [Column("invoicedetail_create_uid")]
            public string? InvoiceDetailCreateUid { get; set; }

            [Column("invoicedetail_edited_uid")]
            public string? InvoiceDetailEditedUid { get; set; }

            [Column("invoicedetail_created")]
            public DateTime? InvoiceDetailCreated { get; set; }

            [Column("invoicedetail_updated")]
            public DateTime? InvoiceDetailUpdated { get; set; }
        }
}


