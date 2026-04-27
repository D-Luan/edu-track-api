using EduTrack.Api.DTOs;
using EduTrack.Api.Models;

namespace EduTrack.Api.Repositories;

public interface IStudentRepository
{
    /// <summary>
    /// Retrieves a student and their enrolled courses by student identifier.
    /// </summary>
    /// <remarks>Returns null if the student does not exist or is not enrolled in any courses.</remarks>
    Task<StudentWithCoursesDto?> GetWithCoursesAsync(int id);
    Task<StudentDto> CreateAsync(Student student);
    Task<Student?> GetByIdAsync(int id);
    Task UpdateAsync(Student student);
}
