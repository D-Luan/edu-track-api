using System.Text.RegularExpressions;

namespace EduTrack.Api.Models;

public class Student
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    // Private collection to protect internal state. No one from the outside can directy use ".Add()".
    private readonly List<Course> _courses = new();
    public IReadOnlyCollection<Course> Courses => _courses.AsReadOnly();

    protected Student() { }

    public Student(string name, string email)
    {
        UpdateInfo(name, email);
    }

    /// <summary>
    /// Updates the user's name and email address with the specified values.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown if the name is null, empty, or consists only of white-space characters, or if the email is not in a valid
    /// email format.</exception>
    public void UpdateInfo(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty");
        }

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            throw new ArgumentException("Invalid email format");
        }

        Name = name;
        Email = email;
    }
    
    /// <summary>
    /// Enrolls the student in the specified course.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if the student is already enrolled in the specified course.</exception>
    public void EnrollIn(Course course)
    {
        if (_courses.Any(c => c.Id == course.Id))
        {
            throw new InvalidOperationException("Student is already enrolled in this course.");
        }

        _courses.Add(course);
    }
}
