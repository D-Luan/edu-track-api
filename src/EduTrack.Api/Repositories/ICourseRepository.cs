using EduTrack.Api.Models;

namespace EduTrack.Api.Repositories;

public interface ICourseRepository
{
    Task<int> CreateAsync(Course course);
    Task<IEnumerable<Course>> GetAllAsync();
}
