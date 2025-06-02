namespace StructuredLogging.Demo.Models
{
    /// <summary>
    /// User model representing a customer in our e-commerce system
    /// This model is used throughout the application to demonstrate
    /// structured logging with user context information
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime LastLoginAt { get; set; }
        
        // This property helps us demonstrate structured logging with user roles
        public string Role { get; set; } = "Customer";
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Full name property to demonstrate structured logging with computed values
        /// </summary>
        public string FullName => $"{FirstName} {LastName}";
    }
}
