namespace EduTrack.Api.Models;

public class Course
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    protected Course() { }

    public Course(string name, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Course name cannot be empty");
        }

        Name = name;
        Description = description;
    }
}
