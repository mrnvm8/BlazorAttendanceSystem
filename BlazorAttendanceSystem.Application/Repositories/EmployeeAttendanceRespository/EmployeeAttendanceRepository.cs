using BlazorAttendanceSystem.Application.Database;
using BlazorAttendanceSystem.Shared;
using Dapper;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Collections.Generic;
using System.Data;
using System.Threading;

namespace BlazorAttendanceSystem.Application.Repositories.EmployeeAttendancesRespository
{
    public class EmployeeAttendanceRepository : IEmployeeAttendanceRepository
    {
        private readonly INpgsqlDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<EmployeeAttendanceRepository> _logger;

        public EmployeeAttendanceRepository(INpgsqlDbConnectionFactory dbConnectionFactory, 
            ILogger<EmployeeAttendanceRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory ?? 
                throw new ArgumentNullException(nameof(dbConnectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<EmployeeAttendance>> GetAllEmployeeAttendancesAsync(
            CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"EmployeeAttendances\"";

            try
            {
                var attendances = await dbConnection.QueryAsync<EmployeeAttendance>(
                    new CommandDefinition(sql, cancellationToken: cancellationToken));

                transaction.Commit();
                _logger.LogInformation("Retrieved {Count} employee attendances from the database", attendances.Count());
                return attendances;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving employee attendances from the database");
                throw;
            }
        }

        public async Task<EmployeeAttendance?> GetEmployeeAttendanceByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"EmployeeAttendances\" WHERE \"Id\" = @Id";

            try
            {
                var attendance = await dbConnection.QueryFirstOrDefaultAsync<EmployeeAttendance>(
                    new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));

                transaction.Commit();

                _logger.LogInformation("Retrieved employee attendance with ID {Id} from the database", id);

                if (attendance is null)
                {
                    return null;
                }

                return attendance;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving employee attendance from the database");
                throw;
            }
        }

        public async Task<Guid> AddEmployeeAttendanceAsync(EmployeeAttendance attendance, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            INSERT INTO ""EmployeeAttendances"" (""Id"", ""EmployeeId"", ""EmployeeLeaveId"", ""TimeIn"", 
            ""TimeOut"", ""Present"", ""Reason"", ""TotalWorkedHours"") 
            VALUES(@Id, @EmployeeId, @EmployeeLeaveId, @TimeIn, @TimeOut, @Present, @Reason, @TotalWorkedHours)";

            try
            {
                await dbConnection.ExecuteAsync(new CommandDefinition(sql, attendance, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Added a new employee attendance with ID {Id} to the database", attendance.Id);

                return attendance.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while adding a new employee attendance to the database");
                throw;
            }
        }

        public async Task<bool> UpdateEmployeeAttendanceAsync(EmployeeAttendance attendance, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            UPDATE ""EmployeeAttendances""
            SET ""EmployeeId"" = @EmployeeId, ""EmployeeLeaveId"" = @EmployeeLeaveId, ""TimeIn"" = @TimeIn,
                ""TimeOut"" = @TimeOut, ""Present"" = @Present, ""Reason"" = @Reason, 
                ""TotalWorkedHours"" = @TotalWorkedHours
            WHERE ""Id"" = @Id";

            try
            {
                int rowsAffected = await dbConnection.ExecuteAsync(new CommandDefinition(sql, attendance, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Updated employee attendance with ID {Id} in the database", attendance.Id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while updating employee attendance in the database");
                throw;
            }
        }

        public async Task<bool> DeleteEmployeeAttendanceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "DELETE FROM \"EmployeeAttendances\" WHERE \"Id\" = @Id";

            try
            {
                int rowsAffected = await dbConnection.ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Deleted employee attendance with ID {Id} from the database", id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while deleting employee attendance from the database");
                throw;
            }
        }
    }
}
