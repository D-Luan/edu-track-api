using System.Data;

namespace EduTrack.Api.Data;

/// <summary>
/// Defines a factory for creating new instances of database connections to a SQL data source.
/// </summary>
/// <remarks>Implementations of this interface are responsible for providing properly configured and ready-to-use
/// instances of <see cref="IDbConnection"/>. The caller is responsible for managing the lifetime of the returned
/// connection, including opening and disposing it as needed.</remarks>
public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}
