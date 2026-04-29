using EduTrack.Api.Models;

namespace EduTrack.Tests.UnitTests;

public class StudentUnitTests
{
    [Fact]
    public void EnrollIn_SameCourseTwice_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Student robert = new("Robert Smith", "robert234@email.com");
        Course computerScience = new("Computer Science");
        robert.EnrollIn(computerScience);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => robert.EnrollIn(computerScience));
    }
}
