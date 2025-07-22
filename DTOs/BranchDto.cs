using System.ComponentModel.DataAnnotations;

namespace FumicertiApi.DTOs
{
    public class BranchDto
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string? Gstin { get; set; }
        public string? Pan { get; set; }
        public string? Address1 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Email { get; set; }
        public string? ContactNo { get; set; }
        public bool Status { get; set; }
    }

    public class BranchAddDto
    {
        [Required]
        public string BranchName { get; set; } = string.Empty;
        public string? Gstin { get; set; }
        public string? Pan { get; set; }
        public string? Address1 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Email { get; set; }
        public string? ContactNo { get; set; }
        public bool Status { get; set; }
    }

    public class PagedBranchListViewModel
    {
        public List<BranchDto> Branches { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
    public class PaginationDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }
    public class PagedBranchResponse
    {
        public PaginationDto Pagination { get; set; } = new();
        public List<BranchDto> Data { get; set; } = new();
    }
}
