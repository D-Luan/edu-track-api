using EduTrack.Api.Models;
using EduTrack.Api.Repositories;
using EduTrack.Tests.Fixtures;

namespace EduTrack.Tests.IntegrationTests;

public class CourseRepositoryTests(DatabaseFixture fixture) : IClassFixture<DatabaseFixture>
{
    private readonly CourseRepository _repository = new(new TestConnectionFactory(fixture.ConnectionString));

    [Fact]
    public async Task CreateAsync_ValidCourse_ShouldInsertAndReturnId()
    {
        // Arrange
        var course = new Course("Docker 101", "Container Files");

        // Act
        var generatedId = await _repository.CreateAsync(course);

        // Assert
        Assert.True(generatedId > 0);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCourses()
    {
        // Arrange
        await _repository.CreateAsync(new Course("Django Framework"));
        await _repository.CreateAsync(new Course("AWS Cloud"));

        // Act
        var courses = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(courses);
        Assert.Contains(courses, c => c.Name == "Django Framework");
        Assert.Contains(courses, c => c.Name == "AWS Cloud");
    }
}
