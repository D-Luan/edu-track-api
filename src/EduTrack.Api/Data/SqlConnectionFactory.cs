using System.Data;
using Microsoft.Data.SqlClient;

namespace EduTrack.Api.Data;

public class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection() => new SqlConnection(connectionString);
}
