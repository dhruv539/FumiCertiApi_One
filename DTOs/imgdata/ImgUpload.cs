using System.ComponentModel.DataAnnotations;

namespace FumicertiApi.DTOs.imgdata
{
    public class ImgUploadDto
    {
        [Required]
        public IFormFile ImageFile { get; set; }


        public string? Location { get; set; }

        [Required]
        public string? ExtractedText { get; set; }

      
    }
}
