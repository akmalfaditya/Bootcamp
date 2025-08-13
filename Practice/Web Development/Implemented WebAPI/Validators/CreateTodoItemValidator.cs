using FluentValidation;
using Implemented_WebAPI.DTOs;

namespace Implemented_WebAPI.Validators
{
    public class CreateTodoItemValidator : AbstractValidator<CreateTodoItemDto>
    {
        public CreateTodoItemValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");
                
            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
                
            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.Now.AddDays(-1))
                .WithMessage("Due date cannot be in the past")
                .When(x => x.DueDate.HasValue);
        }
    }
}
