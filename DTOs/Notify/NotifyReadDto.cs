namespace FumicertiApi.DTOs.Notify
{
    public class NotifyReadDto
    {
        public int NotifyId { get; set; }
        public string NotifyCompanyId { get; set; } = string.Empty;
        public int NotifyStateId { get; set; }
        public string NotifyCreateUid { get; set; } = string.Empty;
        public string? NotifyEditedUid { get; set; }
        public string NotifyName { get; set; } = string.Empty;
        public string NotifyType { get; set; } = string.Empty;
        public string? NotifyAddress { get; set; }
        public string? NotifyEmail { get; set; }
        public string? NotifyContactNo { get; set; }
        public string? NotifyGstNo { get; set; }
        public byte NotifyStatus { get; set; }
        public string? NotifyState { get; set; }
        public string? NotifyPincode { get; set; }
        public string? NotifyStateCode { get; set; }
        public DateTime NotifyCreated { get; set; }
        public DateTime NotifyUpdated { get; set; }
        public string NotifyCountry { get; set; } = string.Empty;
    }

}
