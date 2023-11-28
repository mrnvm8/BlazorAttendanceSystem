using BlazorAttendanceSystem.Application.Database;
using BlazorAttendanceSystem.Shared;
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;

namespace BlazorAttendanceSystem.Application.Repositories.PersonRepository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly INpgsqlDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<PersonRepository> _logger;
        public PersonRepository(INpgsqlDbConnectionFactory dbConnectionFactory, ILogger<PersonRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory ??
                throw new ArgumentNullException(nameof(dbConnectionFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Person>> GetAllPeopleAsync(CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            // Define the SQL query
            var sql = "SELECT * FROM \"People\"";

            try
            {
                // Create a CommandDefinition with the SQL query and cancellation token
                // Then execute the query
                var people = await dbConnection
                    .QueryAsync<Person>(new CommandDefinition(sql, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Retrieved {Count} people from the database", people.Count());
                return people;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving people from the database");
                throw;
            }
        }

        public async Task<Person?> GetPersonByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            // Define the SQL query
            var sql = "SELECT * FROM \"People\" WHERE \"Id\" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                var person = await dbConnection
                    .QueryFirstOrDefaultAsync<Person>(
                    new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Retrieved person with ID {Id} from the database", id);

                if (person is null)
                {
                    return null;
                }
                return person;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while retrieving person from the database");
                throw;
            }
        }

        public async Task<Guid> AddPersonAsync(Person person, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            // Define the SQL query
            var sql = @"
            INSERT INTO ""People"" (""Id"", ""FirstName"", ""LastName"", ""DateOfBirth"", ""Email"")
            VALUES (@Id, @FirstName, @LastName, @DateOfBirth, @Email)";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, person, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Added a new person with ID {Id} to the database", person.Id);

                return person.Id;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while adding a new person to the database");
                throw;
            }
        }

        public async Task<bool> UpdatePersonAsync(Person person, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();

            // Define the SQL query
            var sql = @"
            UPDATE ""People""
            SET ""FirstName"" = @FirstName, ""LastName"" = @LastName, ""DateOfBirth"" = @DateOfBirth, ""Email"" = @Email
            WHERE ""Id"" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                int rowsAffected = await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, person, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Updated person with ID {Id} in the database", person.Id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while updating person in the database");
                throw;
            }
        }

        public async Task<bool> DeletePersonAsync(Guid id, CancellationToken cancellationToken = default)
        {
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
            using var transaction = dbConnection.BeginTransaction();
            // Define the SQL query
            var sql = @"DELETE FROM ""People"" WHERE ""Id"" = @Id";

            try
            {
                // Create a CommandDefinition with the SQL query, parameters, and cancellation token
                // Execute the query
                int rowsAffected = await dbConnection
                    .ExecuteAsync(new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
                transaction.Commit();

                _logger.LogInformation("Deleted person with ID {Id} from the database", id);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                _logger.LogError(ex, "Error while deleting person from the database");
                throw;
            }
        }
    }
}
