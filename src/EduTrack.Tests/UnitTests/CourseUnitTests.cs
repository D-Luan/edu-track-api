using EduTrack.Api.Models;

namespace EduTrack.Tests.UnitTests;

public class CourseUnitTests
{
    [Fact]
    public void Constructor_ValidData_ShouldCreateCourse()
    {
        // Arrange
        var course = new Course("C# Backend", "Learn .NET");

        // Act & Assert
        Assert.Equal("C# Backend", course.Name);
        Assert.Equal("Learn .NET", course.Description);
    }

    [Fact]
    public void Constructor_EmptyName_ShouldThrowArgumentException()
    {
        // Arrange, Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Course("", "Using Azure App Service for API deployment"));
        Assert.Equal("Course name cannot be empty", exception.Message);
    }
}
