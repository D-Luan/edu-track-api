using EduTrack.Api.Data;
using EduTrack.Api.Models;
using Dapper;

namespace EduTrack.Api.Repositories;

public class CourseRepository(ISqlConnectionFactory connectionFactory) : ICourseRepository
{
    public async Task<int> CreateAsync(Course course)
    {
        using var connection = connectionFactory.CreateConnection();

        var sql = @"
            INSERT INTO Course (Name, Description)
            OUTPUT INSERTED.Id
            VALUES (@Name, @Description);
        ";

        var generatedId = await connection.ExecuteScalarAsync<int>(
            sql,
            param: new
            {
                Name = course.Name,
                Description = course.Description
            }
        );

        return generatedId;
    }

    public async Task<IEnumerable<Course>> GetAllAsync()
    {
        using var connection = connectionFactory.CreateConnection();
        var sql = "SELECT Id, Name, Description FROM Course;";
        return await connection.QueryAsync<Course>(sql);
    }

    public async Task<Course?> GetByIdAsync(int id)
    {
        using var connection = connectionFactory.CreateConnection();
        var sql = "SELECT Id, Name, Description FROM Course WHERE Id = @Id;";
        return await connection.QuerySingleOrDefaultAsync<Course>(sql, new { Id = id });
    }
}
