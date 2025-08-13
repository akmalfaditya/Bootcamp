using FluentValidation;
using Implemented_MVC.DTOs;

namespace Implemented_MVC.Validators
{
    public class TodoItemFilterValidator : AbstractValidator<TodoItemFilterDto>
    {
        public TodoItemFilterValidator()
        {
            RuleFor(x => x.Status)
                .Must(status => string.IsNullOrEmpty(status) || 
                               new[] { "all", "completed", "pending" }.Contains(status.ToLower()))
                .WithMessage("Status must be 'all', 'completed', or 'pending'.");

            RuleFor(x => x.SortBy)
                .Must(sortBy => string.IsNullOrEmpty(sortBy) || 
                               new[] { "createdAt", "duedate", "title" }.Contains(sortBy.ToLower()))
                .WithMessage("SortBy must be 'CreatedAt', 'DueDate', or 'Title'.");

            RuleFor(x => x.SortOrder)
                .Must(sortOrder => string.IsNullOrEmpty(sortOrder) || 
                                  new[] { "asc", "desc" }.Contains(sortOrder.ToLower()))
                .WithMessage("SortOrder must be 'asc' or 'desc'.");

            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100.");
        }
    }
}
