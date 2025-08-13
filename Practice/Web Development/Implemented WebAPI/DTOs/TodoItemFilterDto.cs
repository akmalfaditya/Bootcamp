namespace Implemented_WebAPI.DTOs
{
    public class TodoItemFilterDto
    {
        public string? Status { get; set; } // "all", "completed", "pending", "overdue"
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public string? SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "CreatedAt";
        public string SortDirection { get; set; } = "desc";
    }
}
