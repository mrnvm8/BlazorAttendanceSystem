using BlazorAttendanceSystem.Application.Database;
using BlazorAttendanceSystem.Shared;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BlazorAttendanceSystem.Application.Repositories.AttendanceRepository
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly INpgsqlDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<AttendanceRepository> _logger;

        public AttendanceRepository(INpgsqlDbConnectionFactory dbConnectionFactory, ILogger<AttendanceRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory ??
                throw new ArgumentNullException(nameof(dbConnectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Attendance>> GetAllAttendancesAsync(CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"Attendances\"";

            try
            {
                var attendances = await dbConnection.QueryAsync<Attendance>(
                    new CommandDefinition(sql, cancellationToken: cancellationToken));

                transaction.Commit();
                _logger.LogInformation("Retrieved {Count} attendances from the database", attendances.Count());
                return attendances;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving attendances from the database");
                throw;
            }
        }

        public async Task<Attendance?> GetAttendanceByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"Attendances\" WHERE \"Id\" = @Id";

            try
            {
                var attendance = await dbConnection.QueryFirstOrDefaultAsync<Attendance>(
                    new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));

                transaction.Commit();

                _logger.LogInformation("Retrieved attendance with ID {Id} from the database", id);

                if (attendance is null)
                {
                    return null;
                }

                return attendance;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving attendance from the database");
                throw;
            }
        }

        public async Task<Guid> AddAttendanceAsync(Attendance attendance, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            INSERT INTO ""Attendances"" (""Id"", ""EmployeeId"", ""EmployeeLeaveId"", ""TimeIn"", ""TimeOut"", ""Present"", ""Reason"", ""TotalWorkedHours"")
            VALUES (@Id, @EmployeeId, @EmployeeLeaveId, @TimeIn, @TimeOut, @Present, @Reason, @TotalWorkedHours)";

            try
            {
                await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, attendance, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Added a new attendance with ID {Id} to the database", attendance.Id);

                return attendance.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while adding a new attendance to the database");
                throw;
            }
        }

        public async Task<bool> UpdateAttendanceAsync(Attendance attendance, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            UPDATE ""Attendances""
            SET ""EmployeeId"" = @EmployeeId, ""EmployeeLeaveId"" = @EmployeeLeaveId, ""TimeIn"" = @TimeIn, ""TimeOut"" = @TimeOut,
                ""Present"" = @Present, ""Reason"" = @Reason, ""TotalWorkedHours"" = @TotalWorkedHours
            WHERE ""Id"" = @Id";

            try
            {
                int rowsAffected = await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, attendance, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Updated attendance with ID {Id} in the database", attendance.Id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while updating attendance in the database");
                throw;
            }
        }

        public async Task<bool> DeleteAttendanceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"DELETE FROM ""Attendances"" WHERE ""Id"" = @Id";

            try
            {
                int rowsAffected = await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Deleted attendance with ID {Id} from the database", id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while deleting attendance from the database");
                throw;
            }
        }
    }
}
