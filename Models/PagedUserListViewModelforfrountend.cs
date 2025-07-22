using FumicertiApi.DTOs.User;

namespace FumicertiApi.Models
{
    public class PagedUserListViewModel
    {
        public List<UserReadDTo> Users { get; set; } = new();
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }

}
