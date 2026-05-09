namespace EduTrack.Api.Models;

public class Enrollment
{
    public int StudentId { get; private set; }
    public int CourseId { get; private set; }

    protected Enrollment() { }

    public Enrollment(int studentId, int courseId)
    {
        if (studentId <= 0) throw new ArgumentException("Student ID is invalid");
        if (courseId <= 0) throw new ArgumentException("Course ID is invalid");

        StudentId = studentId;
        CourseId = courseId;
    }
}
