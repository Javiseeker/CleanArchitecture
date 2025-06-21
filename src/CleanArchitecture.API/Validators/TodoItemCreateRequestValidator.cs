using FluentValidation;
using CleanArchitecture.API.DTOs;

namespace CleanArchitecture.API.Validators
{
    public class TodoItemCreateRequestValidator : AbstractValidator<TodoItemCreateRequest>
    {
        public TodoItemCreateRequestValidator()
        {
            // Title validation
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required")
                .Length(1, 200)
                .WithMessage("Title must be between 1 and 200 characters")
                .Matches(@"^[a-zA-Z0-9\s\-_.,!?()]+$")
                .WithMessage("Title contains invalid characters");

            // Description validation  
            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage("Description cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));

            // Priority validation
            RuleFor(x => x.Priority)
                .IsInEnum()
                .WithMessage("Invalid priority value. Must be Low, Medium, High, or Critical");

            // Due date validation (basic format check only - business rules in domain)
            RuleFor(x => x.DueDate)
                .Must(BeValidDate)
                .WithMessage("Invalid date format")
                .When(x => x.DueDate.HasValue);
        }

        private static bool BeValidDate(DateTime? date)
        {
            return !date.HasValue || date.Value > DateTime.MinValue;
        }
    }
}
