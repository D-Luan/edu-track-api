using Dapper;
using EduTrack.Api.Data;
using EduTrack.Api.DTOs;
using EduTrack.Api.Models;
using System.Data;

namespace EduTrack.Api.Repositories;

public class StudentRepository(ISqlConnectionFactory connectionFactory) : IStudentRepository
{
    /// <summary>
    /// Retrieves a student and their enrolled courses by student identifier asynchronously.
    /// </summary>
    /// <remarks>The returned object includes the student's basic information and all courses in which the
    /// student is currently enrolled. If the student does not exist or is not enrolled in any courses, the result is
    /// null.</remarks>
    /// <param name="studentId">The unique identifier of the student whose information and courses are to be retrieved.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a StudentWithCoursesDto with the
    /// student's details and a list of enrolled courses, or null if the student is not found.</returns>
    public async Task<StudentWithCoursesDto?> GetStudentWithCoursesAsync(int studentId)
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
                    existingStudent = student; //with { Courses = new List<CourseDto>() };
                    studentDictionary.Add(student.Id, existingStudent);
                }

                if (course is not null)
                {
                    existingStudent.Courses.Add(course);
                }
                
                return existingStudent;
            },
            param: new { Id = studentId },
            splitOn: "Id"
        );
        
        return studentDictionary.Values.FirstOrDefault();
    }

    /// <summary>
    /// Asynchronously creates a new student record in the database and returns a data transfer object representing the
    /// created student.
    /// </summary>
    /// <param name="student">The student entity containing the name and email to be added. The Name and Email properties must not be null or
    /// empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a StudentDto with the generated
    /// student ID, name, and email.</returns>
    public async Task<StudentDto> CreateAsync(Student student)
    {
        using var connection = connectionFactory.CreateConnection();

        var sql = @"
            INSERT INTO Student (Name, Email)
            OUTPUT INSERTED.Id
            VALUES (@Name, @Email);
        ";

        // Dapper executes the query and captures only the first column of the first row (the ID)
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

    public async Task<Student?> GetEntityByIdAsync(int id)
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
}
