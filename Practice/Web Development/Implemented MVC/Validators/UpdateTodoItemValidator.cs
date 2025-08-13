using FluentValidation;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Validators
{
    public class UpdateTodoItemValidator : AbstractValidator<UpdateTodoItemDto>
    {
        public UpdateTodoItemValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(DateTime.Today)
                .When(x => x.DueDate.HasValue)
                .WithMessage("Due date cannot be in the past.");
        }
    }
}
