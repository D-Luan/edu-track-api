using EduTrack.Api.Models;

namespace EduTrack.Tests.UnitTests;

public class EnrollmentUnitTests
{
    [Fact]
    public void Constructor_ValidIds_ShouldCreateEnrollment()
    {
        // Arrange
        var enrollment = new Enrollment(1, 2);

        // Act & Assert
        Assert.Equal(1, enrollment.StudentId);
        Assert.Equal(2, enrollment.CourseId);
    }

    [Fact]
    public void Constructor_InvalidStudentId_ShouldThrowArgumentException()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Enrollment(0, 2));
        Assert.Equal("Student ID is invalid", exception.Message);
    }

    [Fact]
    public void Constructor_InvalidCourseId_ShouldThrowArgumentException()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Enrollment(1, -1));
        Assert.Equal("Course ID is invalid", exception.Message);
    }
}
