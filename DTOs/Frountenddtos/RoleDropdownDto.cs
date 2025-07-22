using System.Text.Json.Serialization;

namespace FumicertiApi.DTOs.Frountenddtos
{
    public class RoleDropdownDto
    {
        [JsonPropertyName("roleUuid")]
        public string RoleUuid { get; set; } = string.Empty;
        [JsonPropertyName("roleName")]
        public string RoleName { get; set; } = string.Empty;
    }
    public class RoleListResponseDto
    {
        [JsonPropertyName("pagination")]
        public PaginationDto Pagination { get; set; }

        [JsonPropertyName("pagedRoles")]
        public List<RoleDropdownDto> PagedRoles { get; set; } = new();
    }

    public class PaginationDto
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("totalRecords")]
        public int TotalRecords { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }
    }

}
