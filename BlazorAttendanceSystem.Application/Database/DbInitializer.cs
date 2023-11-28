using Dapper;
using System.Data;

namespace BlazorAttendanceSystem.Application.Database
{
    public class DbInitializer
    {
        private readonly INpgsqlDbConnectionFactory _dbConnectionFactory;

        public DbInitializer(INpgsqlDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory ?? 
                throw new ArgumentNullException(nameof(dbConnectionFactory));
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            // Create a database connection asynchronously
            using IDbConnection dbConnection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);

            // Create tables for all models
            await CreatePersonTableAsync(dbConnection, cancellationToken);
            await CreateOfficeTableAsync(dbConnection, cancellationToken);
            await CreateLeaveTableAsync(dbConnection, cancellationToken);
            await CreateDepartmentTableAsync(dbConnection, cancellationToken);
            await CreateEmployeeTableAsync(dbConnection, cancellationToken);
            await CreateEmployeeLeaveTableAsync(dbConnection, cancellationToken);
            await CreateEmployeeAttendanceTableAsync(dbConnection, cancellationToken);
            await CreateAttendanceTableAsync(dbConnection, cancellationToken);

        }

        private async Task CreatePersonTableAsync(IDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            if (!TableExists("People", dbConnection))
            {
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS ""People""
                    (
                        ""Id"" UUID NOT NULL,
                        ""FirstName"" VARCHAR(255) NOT NULL,
                        ""LastName"" VARCHAR(255) NOT NULL,
                        ""DateOfBirth"" DATE NOT NULL,
                        ""Email"" VARCHAR(255) NOT NULL,
                        CONSTRAINT ""PK_People"" PRIMARY KEY (""Id"")
                    );";

                await dbConnection.ExecuteAsync(createTableQuery, cancellationToken);
            }
        }

        private async Task CreateOfficeTableAsync(IDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            if (!TableExists("Offices", dbConnection))
            {
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS ""Offices""
                    (
                        ""Id"" UUID NOT NULL,
                        ""Name"" VARCHAR(255) NOT NULL,
                        ""Location"" VARCHAR(255) NOT NULL,
                        ""PhoneNumber"" VARCHAR(20) NOT NULL,
                        ""Email"" VARCHAR(255) NOT NULL,
                        CONSTRAINT ""PK_Offices"" PRIMARY KEY (""Id"")
                    );";

                await dbConnection.ExecuteAsync(createTableQuery, cancellationToken);
            }
        }

        private async Task CreateLeaveTableAsync(IDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            if (!TableExists("Leaves", dbConnection))
            {
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS ""Leaves""
                    (
                        ""Id"" UUID NOT NULL,
                        ""LeaveType"" VARCHAR(255) NOT NULL,
                        ""MaxDaysAllowed"" INT NOT NULL,
                        ""RemainingLeaveDays"" INT NOT NULL,
                        ""Description"" TEXT NOT NULL,
                        CONSTRAINT ""PK_Leaves"" PRIMARY KEY (""Id"")
                    );";

                await dbConnection.ExecuteAsync(createTableQuery, cancellationToken);
            }
        }

        private async Task CreateDepartmentTableAsync(IDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            if (!TableExists("Departments", dbConnection))
            {
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS ""Departments""
                    (
                        ""Id"" UUID NOT NULL,
                        ""OfficeId"" UUID NOT NULL,
                        ""Name"" VARCHAR(255) NOT NULL,
                        ""Manager"" VARCHAR(255),
                        ""Description"" TEXT NOT NULL,
                        CONSTRAINT ""PK_Departments"" PRIMARY KEY (""Id""),
                        CONSTRAINT ""FK_Departments_Offices"" FOREIGN KEY (""OfficeId"") REFERENCES ""Offices""(""Id"") ON DELETE CASCADE
                    );";

                await dbConnection.ExecuteAsync(createTableQuery, cancellationToken);
            }
        }

        private async Task CreateEmployeeTableAsync(IDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            if (!TableExists("Employees", dbConnection))
            {
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS ""Employees""
                    (
                        ""Id"" UUID NOT NULL,
                        ""PersonId"" UUID NOT NULL,
                        ""DepartmentId"" UUID NOT NULL,
                        ""WorkEmail"" VARCHAR(255) NOT NULL,
                        CONSTRAINT ""PK_Employees"" PRIMARY KEY (""Id""),
                        CONSTRAINT ""FK_Employees_People"" FOREIGN KEY (""PersonId"") REFERENCES ""People""(""Id"") ON DELETE CASCADE,
                        CONSTRAINT ""FK_Employees_Departments"" FOREIGN KEY (""DepartmentId"") REFERENCES ""Departments""(""Id"") ON DELETE CASCADE
                    );";

                await dbConnection.ExecuteAsync(createTableQuery, cancellationToken);
            }
        }

        private async Task CreateEmployeeLeaveTableAsync(IDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            if (!TableExists("EmployeeLeaves", dbConnection))
            {
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS ""EmployeeLeaves""
                    (
                        ""Id"" UUID NOT NULL,
                        ""LeaveId"" UUID NOT NULL,
                        ""EmployeeId"" UUID NOT NULL,
                        ""StartDate"" DATE NOT NULL,
                        ""EndDate"" DATE NOT NULL,
                        ""IsActive"" BOOLEAN NOT NULL,
                        ""IsApproved"" BOOLEAN NOT NULL,
                        CONSTRAINT ""PK_EmployeeLeaves"" PRIMARY KEY (""Id""),
                        CONSTRAINT ""FK_EmployeeLeaves_Leaves"" FOREIGN KEY (""LeaveId"") REFERENCES ""Leaves""(""Id"") ON DELETE CASCADE,
                        CONSTRAINT ""FK_EmployeeLeaves_Employees"" FOREIGN KEY (""EmployeeId"") REFERENCES ""Employees""(""Id"") ON DELETE CASCADE
                    );";

                await dbConnection.ExecuteAsync(createTableQuery, cancellationToken);
            }
        }

        private async Task CreateEmployeeAttendanceTableAsync(IDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            if (!TableExists("EmployeeAttendances", dbConnection))
            {
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS ""EmployeeAttendances""
                    (
                        ""Id"" UUID NOT NULL,
                        ""EmployeeId"" UUID NOT NULL,
                        ""EmployeeLeaveId"" UUID,
                        ""TimeIn"" VARCHAR(5),
                        ""TimeOut"" VARCHAR(5),
                        ""Present"" BOOLEAN NOT NULL,
                        ""Reason"" TEXT,
                        ""TotalWorkedHours"" DOUBLE PRECISION NOT NULL,
                        CONSTRAINT ""PK_EmployeeAttendances"" PRIMARY KEY (""Id""),
                        CONSTRAINT ""FK_EmployeeAttendances_Employees"" FOREIGN KEY (""EmployeeId"") REFERENCES ""Employees""(""Id"") ON DELETE CASCADE,
                        CONSTRAINT ""FK_EmployeeAttendances_EmployeeLeaves"" FOREIGN KEY (""EmployeeLeaveId"") REFERENCES ""EmployeeLeaves""(""Id"") ON DELETE CASCADE
                    );";

                await dbConnection.ExecuteAsync(createTableQuery, cancellationToken);
            }
        }

        private async Task CreateAttendanceTableAsync(IDbConnection dbConnection, CancellationToken cancellationToken = default)
        {
            if (!TableExists("Attendances", dbConnection))
            {
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS ""Attendances""
                    (
                        ""Id"" UUID NOT NULL,
                        ""DepartmentId"" UUID NOT NULL,
                        ""Date"" DATE NOT NULL,
                        CONSTRAINT ""PK_Attendances"" PRIMARY KEY (""Id""),
                        CONSTRAINT ""FK_Attendances_Departments"" FOREIGN KEY (""DepartmentId"") REFERENCES ""Departments""(""Id"") ON DELETE CASCADE
                    );";

                await dbConnection.ExecuteAsync(createTableQuery, cancellationToken);
            }
        }

        // Implement methods for creating other tables.
        private bool TableExists(string tableName, IDbConnection dbConnection)
        {
            string query = $"SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = '{tableName}')";
            return dbConnection.ExecuteScalar<bool>(query);
        }
    }
}
