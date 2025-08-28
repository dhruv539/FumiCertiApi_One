using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FumicertiApi.Models
{
    [Table("notifies")]
    public class Notify
    {
        [Column("notify_id")]
        public int NotifyId { get; set; }

        [Column("notify_company_id")]
        public int NotifyCompanyId { get; set; } 


        [Column("notify_state_id")]
        public int NotifyStateId { get; set; }

        [Column("notify_create_uid")]
        public string NotifyCreateUid { get; set; } = string.Empty;

        [Column("notify_edited_uid")]
        public string? NotifyEditedUid { get; set; }

        [Required]
        [Column("notify_name")]
        public string NotifyName { get; set; } = string.Empty;

        [Required]
        [Column("notify_type")]
        public string NotifyType { get; set; } = string.Empty;

        [Column("notify_address")]
        public string? NotifyAddress { get; set; }

        [Column("notify_email")]
        public string? NotifyEmail { get; set; }

        [Column("notify_contactno")]
        public string? NotifyContactNo { get; set; }

        [Column("notify_gstno")]
        public string? NotifyGstNo { get; set; }

        [Column("notify_status")]
        public bool NotifyStatus { get; set; } 

        [Column("notify_state")]
        public string? NotifyState { get; set; }

        [Column("notify_pincode")]
        public string? NotifyPincode { get; set; }

        [Column("notify_statecode")]
        public string? NotifyStateCode { get; set; }

        [Column("notify_created")]
        public DateTime NotifyCreated { get; set; }

        [Column("notify_updated")]
        public DateTime NotifyUpdated { get; set; }

        [Column("notify_country")]
        public string NotifyCountry { get; set; } = string.Empty;
    }
    public class PaginatedResponse<T>
    {
        public PaginationMeta Pagination { get; set; } = new();
        public IEnumerable<T> Data { get; set; } = new List<T>();
    }

    public class PaginationMeta
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }

}
