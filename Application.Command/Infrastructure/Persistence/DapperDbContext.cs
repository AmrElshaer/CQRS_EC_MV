using System.Data;
using Microsoft.Data.SqlClient;

namespace Application.Command.Infrastructure.Persistence;

public class DapperDbContext
{
    private readonly string _connectionString;

    public DapperDbContext(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}
