namespace EduTrack.Api.Repositories;

public interface IEnrollmentRepository
{
    Task<bool> EnrollAsync(int studentId, int courseId);
    Task<bool> IsEnrolledAsync(int studentId, int courseId);
}
