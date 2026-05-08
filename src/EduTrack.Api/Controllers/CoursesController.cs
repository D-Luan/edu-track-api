using EduTrack.Api.Data;
using EduTrack.Api.DTOs;
using EduTrack.Api.Models;
using EduTrack.Api.Repositories;
using EduTrack.Api.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(
    ICourseRepository courseRepository,
    IValidator<CourseRequestDto> courseValidator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCourse([FromBody] CourseRequestDto request)
    {
        var validationResult = await courseValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        // Transforms the DTO into a rich domain object.
        // If the name is invalid, the course class will throw an ArgumentException
        var course = new Course(request.Name, request.Description);
        var generatedId = await courseRepository.CreateAsync(course);

        var response = new CourseDto(generatedId, course.Name, course.Description);

        return CreatedAtAction(
            nameof(GetCourseById),
            new { id = generatedId },
            response
        );
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CourseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCourseById(int id)
    {
        var course = await courseRepository.GetByIdAsync(id);
        if (course is null)
        {
            return NotFound(new { Message = $"Student with ID {id} was not found." });
        }

        var response = new CourseDto(course.Id, course.Name, course.Description);

        return Ok(response);
    }
}
