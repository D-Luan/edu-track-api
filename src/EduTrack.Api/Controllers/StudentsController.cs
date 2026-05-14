using EduTrack.Api.DTOs;
using EduTrack.Api.Models;
using EduTrack.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace EduTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(
    IStudentRepository studentRepository,
    IValidator<StudentRequestDto> studentValidator,
    ILogger<StudentsController> logger) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateStudent([FromBody] StudentRequestDto request)
    {
        logger.LogInformation("Starting student creation. Email: {Email}", request.Email);

        var validationResult = await studentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        // Transforms the DTO into a rich domain object.
        // If the name or email address is invalid, the student class will throw an ArgumentException
        var student = new Student(request.Name, request.Email);
        var generatedId = await studentRepository.CreateAsync(student);

        logger.LogInformation("Student created successfully. ID generated: {StudentId}", generatedId);

        var response = new StudentDto(generatedId, student.Name, student.Email);

        return CreatedAtAction(
            nameof(GetStudentById),
            new { id = generatedId },
            response
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudentWithCoursesDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetStudentById(int id)
    {
        var student = await studentRepository.GetWithCoursesAsync(id);
        if (student is null)
        {
            return NotFound(new { Message = $"Student with ID {id} was not found." });
        }

        return Ok(student);
    }

    /// <summary>
    /// Retrieves a paginated list of students.
    /// </summary>
    /// <param name="pageNumber">The current page number (default: 1).</param>
    /// <param name="pageSize">The number of items per page (default: 10, max: 50).</param>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<StudentDto>))]
    public async Task<IActionResult> GetAllStudents(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 50) pageSize = 50;

        var pagedStudents = await studentRepository.GetAllAsync(pageNumber, pageSize);
        var students = pagedStudents.Items.Select(s => new StudentDto(s.Id, s.Name, s.Email));

        var response = new PagedResult<StudentDto>
        {
            Items = students,
            TotalCount = pagedStudents.TotalCount,
            PageNumber = pagedStudents.PageNumber,
            PageSize = pagedStudents.PageSize
        };

        return Ok(response);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStudent(int id, [FromBody] StudentRequestDto request)
    {
        var validationResult = await studentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var student = await studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return NotFound(new { Message = $"Student with ID {id} was not found." });
        }

        student.UpdateInfo(request.Name, request.Email);
        await studentRepository.UpdateAsync(student);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        logger.LogInformation("Initiating student ID deletion: {StudentId}", id);

        var student = await studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return NotFound(new { Message = $"Student with ID {id} was not found." });
        }

        await studentRepository.DeleteAsync(id);
        logger.LogInformation("Student ID {StudentId} successfully deleted.", id);

        return NoContent();
    }
}
