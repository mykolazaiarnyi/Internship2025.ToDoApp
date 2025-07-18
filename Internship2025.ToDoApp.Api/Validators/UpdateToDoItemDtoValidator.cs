using FluentValidation;
using Internship2025.ToDoApp.Domain.DTOs;

namespace Internship2025.ToDoApp.Api.Validators;

public class UpdateToDoItemDtoValidator : AbstractValidator<UpdateToDoItemDto>
{
    public UpdateToDoItemDtoValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.DueDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Due date must be in the future or today.");
    }
}
