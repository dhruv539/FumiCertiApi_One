using System.ComponentModel.DataAnnotations;

namespace FumicertiApi.DTOs.Notify
{
    public class NotifyAddDto
    {
        //[Required(ErrorMessage = "Company ID is required.")]
        //public string NotifyCompanyId { get; set; } = string.Empty;

        public int NotifyStateId { get; set; } = 0;

        //[Required(ErrorMessage = "Creator ID is required.")]
        //public string NotifyCreateUid { get; set; } = string.Empty;

        [Required(ErrorMessage = "Notify name is required.")]
        [StringLength(200, ErrorMessage = "Notify name can't exceed 200 characters.")]
        public string NotifyName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required.")]
        [StringLength(40)]
        public string NotifyType { get; set; } = string.Empty;

        [StringLength(700)]
        public string? NotifyAddress { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? NotifyEmail { get; set; }

        public string? NotifyContactNo { get; set; }

        public string? NotifyGstNo { get; set; }

        public byte NotifyStatus { get; set; } = 1;

        public string? NotifyState { get; set; }

        public string? NotifyPincode { get; set; }

        public string? NotifyStateCode { get; set; }

        public string NotifyCountry { get; set; } = string.Empty;
    }
}
