using BlazorAttendanceSystem.Application.Database;
using BlazorAttendanceSystem.Shared;
using Dapper;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Data;

namespace BlazorAttendanceSystem.Application.Repositories.EmployeeLeaveRepository
{
    public class EmployeeLeaveRepository : IEmployeeLeaveRepository
    {
        private readonly INpgsqlDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<EmployeeLeaveRepository> _logger;

        public EmployeeLeaveRepository(INpgsqlDbConnectionFactory dbConnectionFactory, 
            ILogger<EmployeeLeaveRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory ?? throw new ArgumentNullException(nameof(dbConnectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<EmployeeLeave>> GetAllEmployeeLeavesAsync(CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"EmployeeLeaves\"";

            try
            {
                var employeeLeaves = await dbConnection.QueryAsync<EmployeeLeave>(
                    new CommandDefinition(sql, cancellationToken: cancellationToken));

                transaction.Commit();
                _logger.LogInformation("Retrieved {Count} employee leaves from the database", employeeLeaves.Count());
                return employeeLeaves;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving employee leaves from the database");
                throw;
            }
        }

        public async Task<EmployeeLeave?> GetEmployeeLeaveByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"EmployeeLeaves\" WHERE \"Id\" = @Id";

            try
            {
                var employeeLeave = await dbConnection.QueryFirstOrDefaultAsync<EmployeeLeave>(
                    new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));

                transaction.Commit();

                _logger.LogInformation("Retrieved employee leave with ID {Id} from the database", id);

                if (employeeLeave is null)
                {
                    return null;
                }

                return employeeLeave;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving employee leave from the database");
                throw;
            }
        }

        public async Task<Guid> AddEmployeeLeaveAsync(EmployeeLeave employeeLeave, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            INSERT INTO ""EmployeeLeaves"" (""Id"", ""LeaveId"", ""EmployeeId"", ""StartDate"", ""EndDate"", ""IsActive"", ""IsApproved"")
            VALUES (@Id, @LeaveId, @EmployeeId, @StartDate, @EndDate, @IsActive, @IsApproved)";

            try
            {
                await dbConnection.ExecuteAsync(new CommandDefinition(sql, employeeLeave, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Added a new employee leave with ID {Id} to the database", employeeLeave.Id);

                return employeeLeave.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while adding a new employee leave to the database");
                throw;
            }
        }

        public async Task<bool> UpdateEmployeeLeaveAsync(EmployeeLeave employeeLeave, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            UPDATE ""EmployeeLeaves""
            SET ""LeaveId"" = @LeaveId, ""EmployeeId"" = @EmployeeId, ""StartDate"" = @StartDate, ""EndDate"" = @EndDate,
                ""IsActive"" = @IsActive, ""IsApproved"" = @IsApproved
            WHERE ""Id"" = @Id";

            try
            {
                int rowsAffected = await dbConnection.ExecuteAsync(new CommandDefinition(sql, employeeLeave, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Updated employee leave with ID {Id} in the database", employeeLeave.Id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while updating employee leave in the database");
                throw;
            }
        }

        public async Task<bool> DeleteEmployeeLeaveAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"DELETE FROM ""EmployeeLeaves"" WHERE ""Id"" = @Id";

            try
            {
                int rowsAffected = await dbConnection.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Deleted employee leave with ID {Id} from the database", id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while deleting employee leave from the database");
                throw;
            }
        }
    }
}
