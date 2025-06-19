using Npgsql;
using System.Data;

namespace Movies.Application.Database
{
    public interface IDbConnnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync();
    }

    public class NpgSqlConnectionFactory : IDbConnnectionFactory
    {
        private readonly string _connectionString;
        public NpgSqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<IDbConnection> CreateConnectionAsync()
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }
    }
}
