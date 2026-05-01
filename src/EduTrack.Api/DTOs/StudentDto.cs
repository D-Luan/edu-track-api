namespace EduTrack.Api.DTOs;

// Outbound DTOs (Read)
public record StudentDto(int Id, string Name, string Email);
public record StudentWithCoursesDto(int Id, string Name, string Email)
{
    public List<CourseDto> Courses { get; set; } = new();
}

// Incoming DTOs (Write)
public record StudentRequestDto(string Name, string Email);