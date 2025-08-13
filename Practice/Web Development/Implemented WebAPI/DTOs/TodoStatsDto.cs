namespace Implemented_WebAPI.DTOs
{
    public class TodoStatsDto
    {
        public int Total { get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }
        public int Overdue { get; set; }
    }
}
