using Npgsql;
using System.Data;

namespace Movies.Application.Database
{
    public interface IDbConnnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
    }

    public class NpgSqlConnectionFactory : IDbConnnectionFactory
    {
        private readonly string _connectionString;
        public NpgSqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(token);
            return connection;
        }
    }
}
