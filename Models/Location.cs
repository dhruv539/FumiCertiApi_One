using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FumicertiApi.Models
{
    [Table("locations")]
    public class Location
    {
        [Key]
        [Column("locations_id")]
        public int LocationId { get; set; }

        [Column("locations_type")]
        [MaxLength(45)]
        public string? LocationType { get; set; }

        [Column("locations_name")]
        [MaxLength(45)]
        public string? LocationName { get; set; }

        [Column("locations_created")]
        public DateTime? CreatedAt { get; set; }

        [Column("locations_updated")]
        public DateTime? UpdatedAt { get; set; }

        [Column("locations_create_uid")]
        [MaxLength(60)]
        public string? CreatedBy { get; set; }

        [Column("locations_edited_uid")]
        [MaxLength(60)]
        public string? EditedBy { get; set; }
    }
}
