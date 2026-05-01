namespace EduTrack.Api.Models;

public class Course
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    protected Course() { }

    public Course(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Course name cannot be empty");
        Name = name;
    }
}
