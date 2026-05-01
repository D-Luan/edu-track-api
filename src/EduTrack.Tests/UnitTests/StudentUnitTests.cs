using EduTrack.Api.Models;

namespace EduTrack.Tests.UnitTests;

public class StudentUnitTests
{
    [Fact]
    public void EnrollIn_SameCourseTwice_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Student student = new("Robert Smith", "robert234@email.com");
        Course course = new("Computer Science");
        student.EnrollIn(course);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => student.EnrollIn(course));
    }

    [Fact]
    public void UpdateInfo_ValidData_ShouldUpdateProperties()
    {
        // Arrange
        var student = new Student("Alice Johnson", "alice@email.com");

        // Act
        student.UpdateInfo("Alice Smith", "alice.smith@email.com");

        // Assert
        Assert.Equal("Alice Smith", student.Name);
        Assert.Equal("alice.smith@email.com", student.Email);
    }

    [Fact]
    public void UpdateInfo_EmptyName_ShouldThrowArgumentException()
    {
        // Arrange
        var student = new Student("Anna Taylor", "anna.taylor@email.com");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => student.UpdateInfo("", "anna.taylor@email.com"));
        Assert.Equal("Name cannot be empty", exception.Message);
    }

    [Fact]
    public void UpdateInfo_InvalidEmail_ShouldThrowArgumentException()
    {
        // Arrange
        var student = new Student("John Jackson", "john@email.com");

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => student.UpdateInfo("John Jackson", "john.jackson-email.com"));
        Assert.Equal("Invalid email format", exception.Message);
    }
}
