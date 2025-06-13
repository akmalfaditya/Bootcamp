using FluentValidation;
using FluentValidationMVC.Models;

namespace FluentValidationMVC.Validators
{
    /// <summary>
    /// RegistrationValidator demonstrates FluentValidation implementation exactly as described in the training material
    /// This class inherits from AbstractValidator and defines validation rules for RegistrationModel
    /// Notice how we separate validation logic from the model itself - this is the power of FluentValidation!
    /// </summary>
    public class RegistrationValidator : AbstractValidator<RegistrationModel>
    {
        /// <summary>
        /// Constructor where we define all validation rules using the fluent interface
        /// Each RuleFor() method creates a validation rule for a specific property
        /// The fluent syntax makes validation rules read like natural language
        /// </summary>
        public RegistrationValidator()
        {
            // Username validation rules
            // RuleFor creates a rule for the Username property
            // We chain validation methods for multiple constraints
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is Required.")
                .Length(5, 30).WithMessage("Username must be between 5 and 30 characters.");

            // Email validation rules
            // Notice how we can chain multiple validation rules
            // EmailAddress() is a built-in FluentValidation rule for email format
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is Required.")
                .EmailAddress().WithMessage("Valid Email Address is Required.");

            // Password validation rules
            // Length validation ensures secure password requirements
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is Required.")
                .Length(6, 100).WithMessage("Password must be between 6 and 100 characters.");
        }
    }
}
