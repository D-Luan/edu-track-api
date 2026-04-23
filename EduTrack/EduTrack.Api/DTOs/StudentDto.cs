namespace EduTrack.Api.DTOs;

public record StudentDto(int Id, string Name, string Email);
public record StudentWithCoursesDto(int Id, string Name, string Email, List<CourseDto> Courses);
