using Dapper;
using EduTrack.Api.Data;
using EduTrack.Api.DTOs;
using EduTrack.Api.Models;
using System.Data;

namespace EduTrack.Api.Repositories;

public class StudentRepository(ISqlConnectionFactory connectionFactory) : IStudentRepository
{
    public async Task<StudentWithCoursesDto?> GetWithCoursesAsync(int id)
    {
        using var connection = connectionFactory.CreateConnection();

        var sql = @"
            SELECT
                s.Id, s.Name, s.Email,
                c.Id, c.Name
            FROM Student s
            LEFT JOIN Enrollment e ON s.Id = e.StudentId
            LEFT JOIN Course c ON e.CourseId = c.Id
            WHERE s.Id = @Id;
        ";
       
        var studentDictionary = new Dictionary<int, StudentWithCoursesDto>();

        var result = await connection.QueryAsync<StudentWithCoursesDto,
            CourseDto, StudentWithCoursesDto>(
            sql,
            map: (student, course) =>
            {
                // Grouping logic:
                // 1. If the student is not in the dictionary, I add them and initialize the list of courses.
                // 2. If the course is not null, I add it to the list of the student who is in the dictionary.
                // 3. I return the student instance from the dictionary.
                if (!studentDictionary.TryGetValue(student.Id, out var existingStudent))
                {
                    existingStudent = student;
                    studentDictionary.Add(student.Id, existingStudent);
                }

                if (course is not null)
                {
                    existingStudent.Courses.Add(course);
                }
                
                return existingStudent;
            },
            param: new { Id = id },
            splitOn: "Id"
        );
        
        return studentDictionary.Values.FirstOrDefault();
    }

    public async Task<StudentDto> CreateAsync(Student student)
    {
        using var connection = connectionFactory.CreateConnection();

        var sql = @"
            INSERT INTO Student (Name, Email)
            OUTPUT INSERTED.Id
            VALUES (@Name, @Email);
        ";

        // Dapper executes the query to create a new student and captures only the first column of the first row (the ID)
        var generatedId = await connection.ExecuteScalarAsync<int>(
            sql,
            param: new
            {
                Name = student.Name,
                Email = student.Email
            }
        );

        return new StudentDto(generatedId, student.Name, student.Email);
    }

    public async Task<Student?> GetByIdAsync(int id)
    {
        using var connection = connectionFactory.CreateConnection();
        var sql = "SELECT Id, Name, Email FROM Student WHERE Id = @Id;";
        return await connection.QuerySingleOrDefaultAsync<Student>(sql, new { Id = id });
    }

    public async Task UpdateAsync(Student student)
    {
        using var connection = connectionFactory.CreateConnection();
        var sql = "UPDATE Student SET Name = @Name, Email = @Email WHERE Id = @Id;";
        await connection.ExecuteAsync(sql, new { student.Name, student.Email, student.Id });
    }
    public async Task DeleteAsync(int id)
    {
        using var connection = connectionFactory.CreateConnection();
        var sql = "DELETE FROM Student WHERE Id = @Id";
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}
