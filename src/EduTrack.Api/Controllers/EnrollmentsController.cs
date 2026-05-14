using EduTrack.Api.DTOs;
using EduTrack.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.Data.SqlClient;

namespace EduTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController(
    IEnrollmentRepository enrollmentRepository,
    IValidator<EnrollmentRequestDto> enrollmentValidator,
    ILogger<EnrollmentsController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(EnrollmentRequestDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Enroll([FromBody] EnrollmentRequestDto request)
    {
        logger.LogInformation("Initiating enrollment request. Student: {StudentId}, Course: {CourseId}", request.StudentId, request.CourseId);

        var validationResult = await enrollmentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var isEnrolled = await enrollmentRepository.IsEnrolledAsync(request.StudentId, request.CourseId);
        if (isEnrolled)
        {
            return Conflict(new { Message = "The student is already enrolled in this course." });
        }

        await enrollmentRepository.EnrollAsync(request.StudentId, request.CourseId);
        logger.LogInformation("Enrollment completed successfully. Student: {StudentId}, Course: {CourseId}", request.StudentId, request.CourseId);

        return Ok(new { Message = "Student successfully enrolled!" });
    }
}
