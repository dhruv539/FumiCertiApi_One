using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FumicertiApi.Models
{
    [Table("years")]
    public class Year
    {
        [Key]
        [Column("year_id")]
        public int YearId { get; set; }

        [Column("year_company_id")]
        [MaxLength(30)]
        public string YearCompanyId { get; set; }

        [Column("year_addby_user_id")]
        [MaxLength(30)]
        public string YearAddByUserId { get; set; }

        [Column("year_updatedby_user_id")]
        [MaxLength(30)]
        public string YearUpdatedByUserId { get; set; }

        [Column("year_name")]
        [MaxLength(60)]
        public string YearName { get; set; }

        [Column("year_datefrom")]
        public DateTime? YearDateFrom { get; set; }

        [Column("year_dateto")]
        public DateTime? YearDateTo { get; set; }

        [Column("year_status")]
        public bool YearStatus { get; set; }

        [Column("year_created")]
        public DateTime YearCreated { get; set; }

        [Column("year_updated")]
        public DateTime YearUpdated { get; set; }

        [Column("year_isdefault")]
        public bool YearIsDefault { get; set; }
    }
}