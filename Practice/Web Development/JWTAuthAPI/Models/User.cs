using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace JWTAuthAPI.Models
{
    /// <summary>
    /// User entity representing registered users in our system
    /// Now inheriting from IdentityUser to leverage Microsoft Identity's built-in features
    /// This gives us password hashing, email confirmation, lockout, and more out of the box
    /// Think of this as extending your basic user profile with enterprise-grade security
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Full name property for convenience
        /// This combines first and last name without storing redundant data
        /// </summary>
            public string FullName => $"{FirstName} {LastName}".Trim();
    }
}
