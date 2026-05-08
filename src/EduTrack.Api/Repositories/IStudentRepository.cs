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
    Task<int> CreateAsync(Student student);
    Task<Student?> GetByIdAsync(int id);
    Task<IEnumerable<Student>> GetAllAsync();
    Task UpdateAsync(Student student);
    Task DeleteAsync(int id);
}
