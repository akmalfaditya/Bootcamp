using Microsoft.AspNetCore.Identity;

namespace Implemented_MVC.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    }
}
