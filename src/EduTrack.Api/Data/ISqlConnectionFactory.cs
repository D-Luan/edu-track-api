using System.Data;

namespace EduTrack.Api.Data;

/// <summary>
/// Factory for creating SQL database connections.
/// </summary>
/// <remarks>
/// The caller is responsible for opening and disposing the returned connection.
/// </remarks>
public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
