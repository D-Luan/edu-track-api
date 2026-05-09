using EduTrack.Api.Data;
using Dapper;
using EduTrack.Api.Models;

namespace EduTrack.Api.Repositories;

public class EnrollmentRepository(ISqlConnectionFactory connectionFactory) : IEnrollmentRepository
{
    public async Task<bool> IsEnrolledAsync(int studentId, int courseId)
    {
        using var connection = connectionFactory.CreateConnection();
        var sql = "SELECT COUNT(1) FROM Enrollment WHERE StudentId = @StudentId AND CourseId = @CourseId;";

        var count = await connection.ExecuteScalarAsync<int>(sql, new { StudentId = studentId, CourseId = courseId });
        return count > 0;
    }

    public async Task<bool> EnrollAsync(int studentId, int courseId)
    {
        using var connection = connectionFactory.CreateConnection();
        var sql = @"
            INSERT INTO Enrollment (StudentId, CourseId)
            VALUES (@StudentId, @CourseId);
        ";

        var rowsAffected = await connection.ExecuteAsync(sql, new { StudentId = studentId, CourseId = courseId });
        return rowsAffected > 0;
    }
}
