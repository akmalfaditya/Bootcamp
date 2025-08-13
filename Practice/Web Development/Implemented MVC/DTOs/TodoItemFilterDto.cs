namespace Implemented_MVC.DTOs
{
    public class TodoItemFilterDto
    {
        public string? Status { get; set; } // "all", "completed", "pending"
        public string? SortBy { get; set; } = "CreatedAt"; // "CreatedAt", "DueDate", "Title"
        public string? SortOrder { get; set; } = "desc"; // "asc", "desc"
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
