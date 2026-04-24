using EduTrack.Api.Models;

namespace EduTrack.Tests;

public class StudentTests
{
    /// <summary>
    /// Verifies that enrolling a student in the same course twice throws an InvalidOperationException.
    /// </summary>
    /// <remarks>This test ensures that the EnrollIn method enforces the rule that a student cannot be
    /// enrolled in the same course more than once. Attempting to enroll in a duplicate course should result in an
    /// exception, maintaining data integrity.</remarks>
    [Fact]
    public void EnrollIn_SameCourseTwice_ShouldThrowInvalidOperationException()
    {
        Student robert = new("Robert Smith", "robert234@email.com");
        Course computerScience = new("Computer Science");
        robert.EnrollIn(computerScience);

        Assert.Throws<InvalidOperationException>(() => robert.EnrollIn(computerScience));
    }
}
