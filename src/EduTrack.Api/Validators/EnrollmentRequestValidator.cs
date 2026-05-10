using FluentValidation;
using EduTrack.Api.DTOs;

namespace EduTrack.Api.Validators;

public class EnrollmentRequestValidator : AbstractValidator<EnrollmentRequestDto>
{
    public EnrollmentRequestValidator()
    {
        RuleFor(e => e.StudentId)
            .GreaterThan(0).WithMessage("StudentId must be greater than 0.");

        RuleFor(e => e.CourseId)
            .GreaterThan(0).WithMessage("CourseId must be greater than 0.");
    }
}
