using FluentValidation;
using ProjectManager.API.DTOs;

namespace ProjectManager.API.Validators;

public class TeamDtoValidator : AbstractValidator<TeamDto>
{
    public TeamDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Team name is required.")
            .MinimumLength(3).WithMessage("Team name must be at least 3 characters long.");
    }
}