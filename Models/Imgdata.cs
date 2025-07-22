using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FumicertiApi.Models
{
    [Table("imgdata")]
    public class ImgData
    {
        [Key]
        [Column("imgdata_id")]
        public int ImgDataId { get; set; }

        [Required]
        [Column("imgdata_img")]
        public Byte[]? ImgDataImg { get; set; } // Base64 string or path


        [Column("imgdata_Location")]
        [MaxLength(45)]
        public string? ImgDataLocation { get; set; } // Location/GPS/Address

        [Column("imgdata_timedate")]
        public DateTime? ImgDataTimedate { get; set; } // Capture/Upload datetime

        [Required]
        [Column("imgdata_extractedtext")]
        [MaxLength(45)]
        public string? ImgDataExtractedText { get; set; } // OCR extracted text

        [Column("imgdata_useruploded")]
        [MaxLength(45)]
        public string? ImgDataUserUploaded { get; set; } // Who uploaded

        [Column("imgdata_created")]
        public DateTime? ImgDataCreated { get; set; } // When record created

        [Column("imgdata_updated")]
        public DateTime? ImgDataUpdated { get; set; } // When record updated

        [Column("imgdata_create_uid")]
        [MaxLength(60)]
        public string? ImgDataCreateUid { get; set; } // Creator user ID

        [Column("imgdata_edited_uid")]
        [MaxLength(60)]
        public string? ImgDataEditedUid { get; set; } // Editor user ID
    }
}
