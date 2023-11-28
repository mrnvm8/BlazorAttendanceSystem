using BlazorAttendanceSystem.Application.Database;
using BlazorAttendanceSystem.Shared;
using Dapper;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Data;

namespace BlazorAttendanceSystem.Application.Repositories.LeaveRepository
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly INpgsqlDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<LeaveRepository> _logger;

        public LeaveRepository(INpgsqlDbConnectionFactory dbConnectionFactory, 
            ILogger<LeaveRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory ??
                 throw new ArgumentNullException(nameof(dbConnectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Retrieves all leave records from the database.
        public async Task<IEnumerable<Leave>> GetAllLeavesAsync(CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"Leaves\"";

            try
            {
                // Create a CommandDefinition with the SQL query and cancellation token
                // Then execute the query
                var leaves = await dbConnection
                    .QueryAsync<Leave>(new CommandDefinition(sql, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Retrieved {Count} leaves from the database", leaves.Count());
                return leaves;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving leaves from the database");
                throw;
            }
        }

        // Retrieves a leave record by its ID from the database.
        public async Task<Leave?> GetLeaveByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"Leaves\" WHERE \"Id\" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                var leave = await dbConnection
                    .QueryFirstOrDefaultAsync<Leave>(
                    new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Retrieved leave with ID {Id} from the database", id);

                if (leave is null)
                {
                    return null;
                }
                return leave;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving leave from the database");
                throw;
            }
        }

        // Adds a new leave record to the database
        public async Task<Guid> AddLeaveAsync(Leave leave, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            INSERT INTO ""Leaves"" (""Id"", ""LeaveType"", ""MaxDaysAllowed"", ""RemainingLeaveDays"", ""Description"")
            VALUES (@Id, @LeaveType, @MaxDaysAllowed, @RemainingLeaveDays, @Description)";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, leave, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Added a new leave with ID {Id} to the database", leave.Id);

                return leave.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while adding a new leave to the database");
                throw;
            }
        }

        // Updates an existing leave record in the database.
        public async Task<bool> UpdateLeaveAsync(Leave leave, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            UPDATE ""Leaves""
            SET ""LeaveType"" = @LeaveType, ""MaxDaysAllowed"" = @MaxDaysAllowed, ""RemainingLeaveDays"" = @RemainingLeaveDays, ""Description"" = @Description
            WHERE ""Id"" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                int rowsAffected = await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, leave, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Updated leave with ID {Id} in the database", leave.Id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while updating leave in the database");
                throw;
            }
        }


        // Deletes a leave record by its ID from the database.
        public async Task<bool> DeleteLeaveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"DELETE FROM ""Leaves"" WHERE ""Id"" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                int rowsAffected = await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Deleted leave with ID {Id} from the database", id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while deleting leave from the database");
                throw;
            }
        }
    }
}
