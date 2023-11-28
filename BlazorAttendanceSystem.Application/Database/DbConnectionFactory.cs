using Npgsql;
using System.Data;

namespace BlazorAttendanceSystem.Application.Database
{
    public interface INpgsqlDbConnectionFactory
    {
        // Creates a new database connection.
        //A task representing the asynchronous operation. The result is the database connection.
        //A cancellation token that can be used to cancel the operation
        Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default);
    }

    public class NpgsqlDbConnectionFactory : INpgsqlDbConnectionFactory
    {
        //A string Connection of the database
        private readonly string _connectionString;

        public NpgsqlDbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> CreateConnectionAsync(CancellationToken cancellationToken = default)
        {
            // Create a new NpgsqlConnection
            var connection = new NpgsqlConnection(_connectionString);
            // Open the connection asynchronously with cancellation support
            await connection.OpenAsync(cancellationToken);
            //return the connection
            return connection;
        }
    }
}
