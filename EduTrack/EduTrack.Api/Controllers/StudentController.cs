using EduTrack.Api.DTOs;
using EduTrack.Api.Models;
using EduTrack.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace EduTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController(
    IStudentRepository repository,
    IValidator<CreateStudentRequest> validator) : ControllerBase
{
    /// <summary>
    /// Creates a new student record using the provided student details.
    /// </summary>
    /// <remarks>Returns a 500 Internal Server Error response if an unexpected error occurs during
    /// creation.</remarks>
    /// <param name="request">The student information to create. Must include valid name and email values. Cannot be null.</param>
    /// <returns>A 201 Created response containing the created student if successful; otherwise, a 400 Bad Request response if
    /// validation fails.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest request)
    {
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        // Mapping: Transforms the DTO into a rich domain object.
        // If the name or email address is invalid, the student class will throw an ArgumentException
        var studentDomain = new Student(request.Name, request.Email);
        var createdStudentDto = await repository.CreateAsync(studentDomain);

        return CreatedAtAction(
            nameof(GetStudentById),
            new { id = createdStudentDto.Id },
            createdStudentDto
        );
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        return Ok();
    }
}
