using FluentValidation;
using EduTrack.Api.DTOs;

namespace EduTrack.Api.Validators;

public class CourseRequestValidator : AbstractValidator<CourseRequestDto>
{
    public CourseRequestValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");
    }
}
