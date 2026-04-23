using System.Data;
using Microsoft.Data.SqlClient;

namespace EduTrack.Api.Data;

/// <summary>
/// Provides a factory for creating SQL Server database connections using a specified connection string.
/// </summary>
/// <param name="connectionString">The connection string used to establish connections to the SQL Server database. Cannot be null or empty.</param>
public class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection()
    {
        return new SqlConnection(connectionString);
    }
}
