using EduTrack.Api.DTOs;
using EduTrack.Api.Models;
using EduTrack.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;

namespace EduTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController(
    IStudentRepository studentRepository,
    IValidator<StudentRequestDto> studentValidator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(StudentDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateStudent([FromBody] StudentRequestDto request)
    {
        var validationResult = await studentValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        // Transforms the DTO into a rich domain object.
        // If the name or email address is invalid, the student class will throw an ArgumentException
        var student = new Student(request.Name, request.Email);
        var generatedId = await studentRepository.CreateAsync(student);

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
        var student = await studentRepository.GetByIdAsync(id);
        if (student is null)
        {
            return NotFound(new { Message = $"Student with ID {id} was not found." });
        }

        await studentRepository.DeleteAsync(id);
        return NoContent();
    }
}
