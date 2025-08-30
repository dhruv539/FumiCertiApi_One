using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FumicertiApi.Models
{

    [Table("products")]
    public class Product
    {
        [Key]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        [Column("product_name")]
        [StringLength(75)]
        public string? ProductName { get; set; }

        [Required]
        [Column("product_unit")]
        [StringLength(45)]
        public string? ProductUnit { get; set; }

        [Column("product_opening")]
        public decimal ProductOpening { get; set; }

        [Column("product_opening_unit")]
        [StringLength(45)]
        public string? ProductOpeningUnit { get; set; }

        [Column("product_weight_per_unit")]
        public decimal ProductWeightPerUnit { get; set; }

        [Column("product_total_wt")]
        public decimal ProductTotalWt { get; set; }

        [Column("product_consume_qty")]
        public decimal ProductConsumeQty { get; set; }

        [Column("product_create_uid")]
        public string? ProductCreatedBy { get; set; }

        [Column("product_edited_uid")]
        public string? ProductEditBy { get; set; }

        [Column("product_created")]
        public DateTime? ProductCreated { get; set; } = DateTime.UtcNow;

        [Column("product_updated")]
        public DateTime? ProductUpdated { get; set; } = DateTime.UtcNow;

        [Column("product_type")]
        public string? ProductType { get; set; }

        [Column("product_company_id")]
        public int ProductCompanyId { get; set; }

        [Column("product_alpwtper_tablets")]
        public int ProductAlpwtperTablets { get; set; }

    }
}
