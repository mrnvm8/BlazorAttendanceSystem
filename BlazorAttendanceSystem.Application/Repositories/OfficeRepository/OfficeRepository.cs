using BlazorAttendanceSystem.Application.Database;
using BlazorAttendanceSystem.Shared;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BlazorAttendanceSystem.Application.Repositories.OfficeRepository
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly INpgsqlDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<OfficeRepository> _logger;

        public OfficeRepository(INpgsqlDbConnectionFactory dbConnectionFactory, ILogger<OfficeRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory ??
                throw new ArgumentNullException(nameof(dbConnectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        // Retrieves all offices from the database.
        public async Task<IEnumerable<Office>> GetAllOfficesAsync(CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"Offices\"";

            try
            {
                // Create a CommandDefinition with the SQL query and cancellation token
                // Then execute the query
                var offices = await dbConnection
                    .QueryAsync<Office>(new CommandDefinition(sql, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Retrieved {Count} offices from the database", offices.Count());
                return offices;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving offices from the database");
                throw;
            }
        }


        // Retrieves an office by its ID from the database.
        public async Task<Office?> GetOfficeByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"Offices\" WHERE \"Id\" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                var office = await dbConnection
                    .QueryFirstOrDefaultAsync<Office>(
                    new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Retrieved office with ID {Id} from the database", id);

                if (office is null)
                {
                    return null;
                }
                return office;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving office from the database");
                throw;
            }
        }

        // Adds a new office to the database.
        public async Task<Guid> AddOfficeAsync(Office office, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            INSERT INTO ""Offices"" (""Id"", ""Name"", ""Location"", ""PhoneNumber"", ""Email"")
            VALUES (@Id, @Name, @Location, @PhoneNumber, @Email)";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, office, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Added a new office with ID {Id} to the database", office.Id);

                return office.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while adding a new office to the database");
                throw;
            }
        }

        // Updates an existing office in the database.
        public async Task<bool> UpdateOfficeAsync(Office office, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            UPDATE ""Offices""
            SET ""Name"" = @Name, ""Location"" = @Location, ""PhoneNumber"" = @PhoneNumber, ""Email"" = @Email
            WHERE ""Id"" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                int rowsAffected = await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, office, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Updated office with ID {Id} in the database", office.Id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while updating office in the database");
                throw;
            }
        }


        // Deletes an office by its ID from the database.
        public async Task<bool> DeleteOfficeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"DELETE FROM ""Offices"" WHERE ""Id"" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                int rowsAffected = await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Deleted office with ID {Id} from the database", id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while deleting office from the database");
                throw;
            }
        }
    }
}
