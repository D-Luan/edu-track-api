using EduTrack.Api.DTOs;
using FluentValidation;

namespace EduTrack.Api.Validators;

public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
{
    /// <summary>
    /// Initializes a new instance of the CreateStudentRequestValidator class with validation rules for creating a
    /// student request.
    /// </summary>
    /// <remarks>This validator enforces that the Name property is required and cannot exceed 200 characters,
    /// and that the Email property is required and must be in a valid email format. Use this validator to ensure that
    /// incoming create student requests meet the required data integrity constraints before processing.</remarks>
    public CreateStudentRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email format is required.");
    }
}
