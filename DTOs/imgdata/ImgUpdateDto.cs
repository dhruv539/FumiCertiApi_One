namespace FumicertiApi.DTOs
{
    // DTOs/ImgUpdateDto.cs
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    namespace YourApp.DTOs
    {
        public class ImgUpdateDto
        {
            [Required(ErrorMessage = "Image ID is required.")]
            public int ImgDataId { get; set; }

            public IFormFile? ImageFile { get; set; }

            [Required(ErrorMessage = "Location is required.")]
            [MaxLength(45)]
            public string Location { get; set; }

            [MaxLength(45)]
            public string? ExtractedText { get; set; }

        }
    }

}
