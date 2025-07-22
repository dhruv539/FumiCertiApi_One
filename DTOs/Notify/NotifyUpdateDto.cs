using System.ComponentModel.DataAnnotations;

namespace FumicertiApi.DTOs.Notify
{
    public class NotifyUpdateDto
    {

        public int NotifyId { get; set; }

        public int NotifyStateId { get; set; } = 0;

        [Required(ErrorMessage = "Notify name is required.")]
        public string NotifyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required.")]
        public string NotifyType { get; set; } = string.Empty;

        public string? NotifyAddress { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? NotifyEmail { get; set; }

        public string? NotifyContactNo { get; set; }

        [StringLength(20)]
        public string? NotifyGstNo { get; set; }

        public byte NotifyStatus { get; set; }

        public string? NotifyState { get; set; }
        public string? NotifyPincode { get; set; }

        public string? NotifyStateCode { get; set; }

        public string NotifyCountry { get; set; } = string.Empty;
    }
}
