using FluentValidation;
using FluentValidationMVC.Models;

namespace FluentValidationMVC.Validators
{
    /// <summary>
    /// GradeValidator demonstrates FluentValidation for models with relationships
    /// This shows how to validate decimal values, dates, and complex business rules
    /// Much more flexible than the Range and StringLength attributes we removed
    /// </summary>
    public class GradeValidator : AbstractValidator<Grade>
    {
        /// <summary>
        /// Constructor defining validation rules for Grade model
        /// Demonstrates numeric validation, date validation, and conditional rules
        /// </summary>
        public GradeValidator()
        {
            // StudentID validation - must reference an existing student
            RuleFor(x => x.StudentID)
                .NotEmpty().WithMessage("Student ID is required")
                .GreaterThan(0).WithMessage("Student ID must be a positive number");

            // Subject validation - academic subject name
            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Subject is required")
                .Length(2, 100).WithMessage("Subject name must be between 2 and 100 characters")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Subject name can only contain letters and spaces");            // Grade value validation - must be between 0 and 100
            // This replaces the Range attribute with more detailed error messages
            RuleFor(x => x.GradeValue)
                .InclusiveBetween(0, 100).WithMessage("Grade must be between 0 and 100")
                .PrecisionScale(5, 2, false).WithMessage("Grade can have maximum 2 decimal places");

            // Letter grade validation - must be valid letter grades
            // Only validate if letter grade is provided (it's optional)
            RuleFor(x => x.LetterGrade)
                .Must(BeValidLetterGrade).WithMessage("Letter grade must be A, B, C, D, or F")
                .When(x => !string.IsNullOrEmpty(x.LetterGrade));

            // Grade date validation - should be reasonable
            RuleFor(x => x.GradeDate)
                .NotEmpty().WithMessage("Grade date is required")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Grade date cannot be in the future")
                .GreaterThan(DateTime.Now.AddYears(-5)).WithMessage("Grade date cannot be more than 5 years ago");

            // Comments validation - optional but limited length
            RuleFor(x => x.Comments)
                .MaximumLength(500).WithMessage("Comments cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Comments));

            // Cross-field validation example: ensure letter grade matches numeric grade
            // This demonstrates advanced FluentValidation capabilities
            RuleFor(x => x)
                .Must(HaveMatchingLetterGrade)
                .WithMessage("Letter grade does not match the numeric grade value")
                .When(x => !string.IsNullOrEmpty(x.LetterGrade));
        }        /// <summary>
        /// Custom validation method for letter grades
        /// Demonstrates how to encapsulate business rules in validation methods
        /// </summary>
        private bool BeValidLetterGrade(string? letterGrade)
        {
            if (string.IsNullOrEmpty(letterGrade))
                return true; // Letter grade is optional
                
            var validGrades = new[] { "A", "B", "C", "D", "F", "A+", "A-", "B+", "B-", "C+", "C-", "D+" };
            return validGrades.Contains(letterGrade);
        }

        /// <summary>
        /// Advanced validation: ensure letter grade matches numeric grade
        /// This shows how FluentValidation can handle complex business rules
        /// </summary>
        private bool HaveMatchingLetterGrade(Grade grade)
        {
            if (string.IsNullOrEmpty(grade.LetterGrade))
                return true; // Letter grade is optional

            var expectedLetterGrade = grade.GradeValue switch
            {
                >= 97 => "A+",
                >= 93 => "A",
                >= 90 => "A-",
                >= 87 => "B+",
                >= 83 => "B",
                >= 80 => "B-",
                >= 77 => "C+",
                >= 73 => "C",
                >= 70 => "C-",
                >= 67 => "D+",
                >= 60 => "D",
                _ => "F"
            };

            return grade.LetterGrade == expectedLetterGrade;
        }
    }
}
