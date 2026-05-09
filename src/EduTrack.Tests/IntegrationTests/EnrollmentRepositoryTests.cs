using EduTrack.Api.Repositories;
using EduTrack.Tests.Fixtures;
using Microsoft.Data.SqlClient;
using Dapper;

namespace EduTrack.Tests.IntegrationTests;

public class EnrollmentRepositoryTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly EnrollmentRepository _repository = new(new TestConnectionFactory(fixture.ConnectionString));

    private async Task<(int studentId, int courseId)> SeedStudentAndCourseAsync()
    {
        using var connection = new SqlConnection(fixture.ConnectionString);

        var uniqueEmail = $"john.marston.{Guid.NewGuid()}@email.com";

        var studentId = await connection.ExecuteScalarAsync<int>(
            "INSERT INTO Student(Name, Email) OUTPUT INSERTED.Id VALUES ('John Marston', @Email);",
            new { Email = uniqueEmail });

        var courseId = await connection.ExecuteScalarAsync<int>(
            "INSERT INTO Course(Name, Description) OUTPUT INSERTED.Id VALUES ('TypeScript Course', NULL);");

        return (studentId, courseId);
    }

    [Fact]
    public async Task EnrollAsync_ValidData_ShouldReturnTrue()
    {
        // Arrange
        var (studentId, courseId) = await SeedStudentAndCourseAsync();

        // Act
        var result = await _repository.EnrollAsync(studentId, courseId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsEnrolledAsync_WhenEnrolled_ShouldReturnTrue()
    {
        // Arrange
        var (studentId, courseId) = await SeedStudentAndCourseAsync();
        await _repository.EnrollAsync(studentId, courseId);

        // Act
        var isEnrolled = await _repository.IsEnrolledAsync(studentId, courseId);

        // Assert
        Assert.True(isEnrolled);
    }

    [Fact]
    public async Task IsEnrolledAsync_WhenNotEnrolled_ShouldReturnFalse()
    {
        // Arrange
        var (studentId, courseId) = await SeedStudentAndCourseAsync();

        // Act
        var isEnrolled = await _repository.IsEnrolledAsync(studentId, courseId);

        // Assert
        Assert.False(isEnrolled);
    }
}
