using System.Text.RegularExpressions;

namespace EduTrack.Api.Models;

public class Student
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    // DDD: private collection to protect internal state. No one from the outside can directy use ".Add()".
    private readonly List<Course> _courses = new();
    public IReadOnlyCollection<Course> Courses => _courses.AsReadOnly();

    protected Student() { }

    public Student(string name, string email)
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
    /// <param name="course">The course to enroll the student in. Cannot be null. The student must not already be enrolled in this course.</param>
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
