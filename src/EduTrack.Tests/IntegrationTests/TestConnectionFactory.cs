using EduTrack.Api.Data;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EduTrack.Tests.IntegrationTests;

public class TestConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection() => new SqlConnection(connectionString);
}
