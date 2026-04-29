using System.Data;
using Microsoft.Data.SqlClient;
using EduTrack.Api.Models;
using EduTrack.Api.Repositories;
using EduTrack.Api.Data;
using EduTrack.Tests.Fixtures;

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
}
