namespace EduTrack.Api.DTOs;

// Outbound DTOs (Read)
public record CourseDto(int Id, string Name, string? Description = null);

// Incoming DTOs (Write)
public record CourseRequestDto(string Name, string? Description = null);