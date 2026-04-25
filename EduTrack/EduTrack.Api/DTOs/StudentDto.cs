namespace EduTrack.Api.DTOs;

// Outbound DTOs (Read)
public record StudentDto(int Id, string Name, string Email);
public record StudentWithCoursesDto(int Id, string Name, string Email, List<CourseDto> Courses);

// Incoming DTOs (Write)
public record CreateStudentRequest(string Name, string Email);