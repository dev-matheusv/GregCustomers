using Microsoft.Data.SqlClient;
using System.Data;

namespace GregCustomers.Infrastructure.Persistence;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
        => _connectionString = connectionString;

    public IDbConnection Create()
        => new SqlConnection(_connectionString);
}