using System.Data;
using Microsoft.Data.SqlClient;

namespace Sample_API.DBContext;

public class ApplicationDbContext
{
    private readonly string _connectionString;
    public ApplicationDbContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SqlConnection");
    }
    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}