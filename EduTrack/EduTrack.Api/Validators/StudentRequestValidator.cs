using EduTrack.Api.DTOs;
using FluentValidation;

namespace EduTrack.Api.Validators;

public class StudentRequestValidator : AbstractValidator<StudentRequestDto>
{
    public StudentRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email format is required.");
    }
}
