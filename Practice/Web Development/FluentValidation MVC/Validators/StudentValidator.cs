using FluentValidation;
using FluentValidationMVC.Models;

namespace FluentValidationMVC.Validators
{
    /// <summary>
    /// StudentValidator demonstrates advanced FluentValidation techniques for complex models
    /// This replaces all the Data Annotations we removed from the Student model
    /// Shows how FluentValidation can handle complex business rules more elegantly than attributes
    /// </summary>
    public class StudentValidator : AbstractValidator<Student>
    {
        /// <summary>
        /// Constructor defining validation rules for Student model
        /// Notice how much more readable and maintainable this is compared to Data Annotations
        /// We can easily modify validation rules without touching the model class
        /// </summary>
        public StudentValidator()
        {
            // Name validation - required field with length constraints
            // NotEmpty() ensures the field is not null, empty, or whitespace
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Student name is required")
                .Length(2, 100).WithMessage("Name must be between 2 and 100 characters")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Name can only contain letters and spaces");

            // Gender validation - must be one of the allowed values
            // This replaces the RegularExpression attribute we had before
            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .Must(BeValidGender).WithMessage("Gender must be Male, Female, or Other");

            // Branch validation - academic department
            RuleFor(x => x.Branch)
                .NotEmpty().WithMessage("Branch is required")
                .Length(2, 50).WithMessage("Branch must be between 2 and 50 characters");

            // Section validation - class section
            RuleFor(x => x.Section)
                .NotEmpty().WithMessage("Section is required")
                .Length(1, 10).WithMessage("Section must be between 1 and 10 characters");

            // Email validation - using built-in EmailAddress rule
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please enter a valid email address");

            // Phone number validation - optional field with format checking
            // When() creates conditional validation - only validate if phone number is provided
            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?[\d\s\-\(\)]+$").WithMessage("Please enter a valid phone number")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            // Enrollment date validation - must be a reasonable date
            RuleFor(x => x.EnrollmentDate)
                .NotEmpty().WithMessage("Enrollment date is required")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Enrollment date cannot be in the future")
                .GreaterThan(DateTime.Now.AddYears(-10)).WithMessage("Enrollment date cannot be more than 10 years ago");
        }

        /// <summary>
        /// Custom validation method for gender
        /// This demonstrates how to create custom validation logic
        /// Much cleaner than complex regular expressions in attributes
        /// </summary>
        private bool BeValidGender(string gender)
        {
            var validGenders = new[] { "Male", "Female", "Other" };
            return validGenders.Contains(gender);
        }
    }
}
