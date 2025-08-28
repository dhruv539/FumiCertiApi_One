using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FumicertiApi.Models
{
    [Table("afo_member")]
    public class AfoMember
    {
        [Key]
        [Column("afo_id")]
        public int AfoId { get; set; }

        [Required]
        [Column("afo_name")]
        public string? AfoName { get; set; }

        [Column("afo_mbr_no")]
        public string? AfoMbrNo { get; set; }

        [Column("afo_alp_no")]
        public string? AfoAlpNo { get; set; }

        [Column("afo_create_uid")]
        public string? AfoAddBy { get; set; }

        [Column("afo_edited_uid")]
        public string? AfoEditBy { get; set; }

        [Column("afo_created")]
        public DateTime? AfoCreated { get; set; }

        [Column("afo_updated")]
        public DateTime? AfoUpdated { get; set; }

        [Column("afo_company_id")]
        public int AfoCompanyId { get; set; }
    }
    public class PagedAfoMemberListViewModel
    {
        public List<AfoMember> Members { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
    public class PagedAfoMemberApiResponse
    {
        public PaginationMeta Pagination { get; set; } = new();
        public List<AfoMember> Data { get; set; } = new();
    }

    //public class PaginationMeta
    //{
    //    public int Page { get; set; }
    //    public int PageSize { get; set; }
    //    public int TotalRecords { get; set; }
    //    public int TotalPages { get; set; }
    //}

}
