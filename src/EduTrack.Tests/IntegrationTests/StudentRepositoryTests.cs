using System.Data;
using Microsoft.Data.SqlClient;
using EduTrack.Api.Models;
using EduTrack.Api.Repositories;
using EduTrack.Api.Data;
using EduTrack.Tests.Fixtures;
using Dapper;

namespace EduTrack.Tests.IntegrationTests;

public class TestConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection() => new SqlConnection(connectionString);
}

public class StudentRepositoryTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly StudentRepository _repository = new(new TestConnectionFactory(fixture.ConnectionString));

    [Fact]
    public async Task CreateAsync_ValidStudent_ShouldInsertAndReturnId()
    {
        // Arrange
        var student = new Student("Alice Test", "alice23@gmail.com");

        // Act
        var generatedId = await _repository.CreateAsync(student);

        // Assert
        Assert.True(generatedId > 0);
    }

    [Fact]
    public async Task GetStudentWithCourses_StudentWithTwoCourses_ShouldReturnGroupedDto()
    {
        /// Arrange
        using var connection = new SqlConnection(fixture.ConnectionString);

        var studentId = await connection.ExecuteScalarAsync<int>(
            "INSERT INTO Student (Name, Email) OUTPUT INSERTED.Id VALUES ('Robert', 'robert23@test.com');");

        var course1Id = await connection.ExecuteScalarAsync<int>(
            "INSERT INTO Course (Name) OUTPUT INSERTED.Id VALUES ('C# Backend');");
        var course2Id = await connection.ExecuteScalarAsync<int>(
            "INSERT INTO Course (Name) OUTPUT INSERTED.Id VALUES ('Azure Cloud');");

        await connection.ExecuteAsync(
            "INSERT INTO Enrollment (StudentId, CourseId) VALUES (@sId, @c1Id), (@sId, @c2Id);",
            new { sId = studentId, c1Id = course1Id, c2Id = course2Id });

        // Act
        var result = await _repository.GetWithCoursesAsync(studentId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Robert", result.Name);
        Assert.Equal(2, result.Courses.Count);
        Assert.Contains(result.Courses, c => c.Name == "C# Backend");
        Assert.Contains(result.Courses, c => c.Name == "Azure Cloud");
    }

    [Fact]
    public async Task DeleteAsync_StudentWithActiveEnrollment_ShouldThrowSqlException()
    {
        // Arrange
        using var connection = new SqlConnection(fixture.ConnectionString);
        var studentId = await connection.ExecuteScalarAsync<int>(
            "INSERT INTO Student (Name, Email) OUTPUT INSERTED.Id VALUES ('John', 'john@test.com');");
        var courseId = await connection.ExecuteScalarAsync<int>(
            "INSERT INTO Course (Name) OUTPUT INSERTED.Id VALUES ('SQL 101');");
        await connection.ExecuteAsync(
            "INSERT INTO Enrollment (StudentId, CourseId) VALUES (@sId, @cId);", new { sId = studentId, cId = courseId });

        // Act & Assert
        var exception = await Assert.ThrowsAsync<SqlException>(() => _repository.DeleteAsync(studentId));
        Assert.Equal(547, exception.Number); // 547 = Foreign Key Violation
    }
}
