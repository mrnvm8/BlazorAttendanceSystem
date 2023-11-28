using BlazorAttendanceSystem.Application.Database;
using BlazorAttendanceSystem.Shared;
using Dapper;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Data;

namespace BlazorAttendanceSystem.Application.Repositories.DepartmentRepository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly INpgsqlDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<DepartmentRepository> _logger;

        public DepartmentRepository(INpgsqlDbConnectionFactory dbConnectionFactory, ILogger<DepartmentRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory ??
                throw new ArgumentNullException(nameof(dbConnectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Retrieves all departments from the database.
        public async Task<IEnumerable<Department>> GetAllDepartmentsAsync(CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"Departments\"";

            try
            {
                // Create a CommandDefinition with the SQL query and cancellation token
                // Then execute the query
                var departments = await dbConnection
                    .QueryAsync<Department>(new CommandDefinition(sql, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Retrieved {Count} departments from the database", departments.Count());
                return departments;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving departments from the database");
                throw;
            }
        }

        // Retrieves a department by its ID from the database.
        public async Task<Department?> GetDepartmentByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"Departments\" WHERE \"Id\" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                var department = await dbConnection
                    .QueryFirstOrDefaultAsync<Department>(
                    new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Retrieved department with ID {Id} from the database", id);

                if (department is null)
                {
                    return null;
                }
                return department;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving department from the database");
                throw;
            }
        }

        // Adds a new department to the database.
        public async Task<Guid> AddDepartmentAsync(Department department, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            INSERT INTO ""Departments"" (""Id"", ""OfficeId"", ""Name"", ""Manager"", ""Description"")
            VALUES (@Id, @OfficeId, @Name, @Manager, @Description)";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, department, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Added a new department with ID {Id} to the database", department.Id);

                return department.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while adding a new department to the database");
                throw;
            }
        }

        // Updates an existing department in the database.
        public async Task<bool> UpdateDepartmentAsync(Department department, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            UPDATE ""Departments""
            SET ""OfficeId"" = @OfficeId, ""Name"" = @Name, ""Manager"" = @Manager, ""Description"" = @Description
            WHERE ""Id"" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                int rowsAffected = await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, department, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Updated department with ID {Id} in the database", department.Id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while updating department in the database");
                throw;
            }
        }

        // Deletes a department by its ID from the database.
        public async Task<bool> DeleteDepartmentAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"DELETE FROM ""Departments"" WHERE ""Id"" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                int rowsAffected = await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Deleted department with ID {Id} from the database", id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while deleting department from the database");
                throw;
            }
        }
    }
}
