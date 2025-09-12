using System.ComponentModel.DataAnnotations;

namespace FumicertiApi.DTOs.imgdata
{
    public class ImgDataReadDto
    {
        public int ImgDataId { get; set; }
        [Required]
        public string ImgData_Img_Url { get; set; }              // Base64 or path
        public string? ImgDataLocation { get; set; }         // Location/GPS
        public DateTime? ImgDataTimedate { get; set; }       // Capture time
        [Required]
        public string? ImgDataExtractedText { get; set; }    // OCR result
        public string? ImgDataUserUploaded { get; set; }     // Uploader name
        public DateTime? ImgDataCreated { get; set; }        // Created timestamp
        public DateTime? ImgDataUpdated { get; set; }        // Updated timestamp
        public string? ImgDataCreateUid { get; set; }        // Creator user ID
        public string? ImgDataEditedUid { get; set; }        // Editor user ID
        public int ImgDataCompanyId { get; set; }
    }

}
