using BlazorAttendanceSystem.Application.Database;
using BlazorAttendanceSystem.Shared;
using Dapper;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Data;

namespace BlazorAttendanceSystem.Application.Repositories.EmployeeRepository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly INpgsqlDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<EmployeeRepository> _logger;

        public EmployeeRepository(INpgsqlDbConnectionFactory dbConnectionFactory, 
            ILogger<EmployeeRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory ??
                throw new ArgumentNullException(nameof(dbConnectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        // Retrieves all employees from the database.
        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"Employees\"";

            try
            {
                // Create a CommandDefinition with the SQL query and cancellation token
                // Then execute the query
                var employees = await dbConnection
                    .QueryAsync<Employee>(new CommandDefinition(sql, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Retrieved {Count} employees from the database", employees.Count());
                return employees;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving employees from the database");
                throw;
            }
        }

        // Retrieves an employee by its ID from the database.
        public async Task<Employee?> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = "SELECT * FROM \"Employees\" WHERE \"Id\" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                var employee = await dbConnection
                    .QueryFirstOrDefaultAsync<Employee>(
                    new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Retrieved employee with ID {Id} from the database", id);

                if (employee is null)
                {
                    return null;
                }
                return employee;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving employee from the database");
                throw;
            }
        }


        // Adds a new employee to the database.
        public async Task<Guid> AddEmployeeAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            INSERT INTO ""Employees"" (""Id"", ""PersonId"", ""DepartmentId"", ""WorkEmail"")
            VALUES (@Id, @PersonId, @DepartmentId, @WorkEmail)";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, employee, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Added a new employee with ID {Id} to the database", employee.Id);

                return employee.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while adding a new employee to the database");
                throw;
            }
        }

        // Updates an existing employee in the database.
        public async Task<bool> UpdateEmployeeAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"
            UPDATE ""Employees""
            SET ""PersonId"" = @PersonId, ""DepartmentId"" = @DepartmentId, ""WorkEmail"" = @WorkEmail
            WHERE ""Id"" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                int rowsAffected = await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, employee, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Updated employee with ID {Id} in the database", employee.Id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while updating employee in the database");
                throw;
            }
        }

        /// <summary>
        /// Deletes an employee by its ID from the database.
        /// </summary>
        public async Task<bool> DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            var sql = @"DELETE FROM ""Employees"" WHERE ""Id"" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                int rowsAffected = await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Deleted employee with ID {Id} from the database", id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while deleting employee from the database");
                throw;
            }
        }
    }
}
