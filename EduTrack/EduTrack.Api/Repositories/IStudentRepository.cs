using EduTrack.Api.DTOs;
using EduTrack.Api.Models;

namespace EduTrack.Api.Repositories;

public interface IStudentRepository
{
    Task<StudentWithCoursesDto?> GetStudentWithCoursesAsync(int studentId);
    Task<StudentDto> CreateAsync(Student student);
    Task<Student?> GetEntityByIdAsync(int id);
    Task UpdateAsync(Student student);
}
