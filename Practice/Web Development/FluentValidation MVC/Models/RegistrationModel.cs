namespace FluentValidationMVC.Models
{
    /// <summary>
    /// Registration model representing user registration form data
    /// This model demonstrates FluentValidation implementation exactly as described in the training material
    /// Notice: NO data annotation attributes here - all validation is handled by FluentValidation
    /// This is the key difference from traditional Data Annotations approach
    /// </summary>
    public class RegistrationModel
    {
        /// <summary>
        /// Username for the new user account
        /// Will be validated using FluentValidation rules for:
        /// - Required field
        /// - Length between 5-30 characters
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Email address for the user
        /// Will be validated for:
        /// - Required field
        /// - Proper email format using FluentValidation
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Password for the user account
        /// Will be validated for:
        /// - Required field
        /// - Length between 6-100 characters using FluentValidation
        /// </summary>
        public string? Password { get; set; }
    }
}
