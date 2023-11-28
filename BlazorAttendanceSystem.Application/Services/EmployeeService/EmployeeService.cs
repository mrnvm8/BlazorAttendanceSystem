using BlazorAttendanceSystem.Application.Mapping;
using BlazorAttendanceSystem.Application.Repositories.EmployeeRepository;
using BlazorAttendanceSystem.Contract.Requests.EmployeeContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BlazorAttendanceSystem.Application.Services.EmployeeService
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;
        
        public EmployeeService(IEmployeeRepository employeeRepository, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<EmployeeResponse>> GetAllEmployeesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var employees = await _employeeRepository.GetAllEmployeesAsync(cancellationToken);
                var employeeResponses = employees.Select(employee => employee.MapToResponse());

                _logger.LogInformation("Retrieved {Count} employees from the database", employeeResponses.Count());
                return employeeResponses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving employees from the database");
                throw;
            }
        }

        public async Task<EmployeeResponse?> GetEmployeeByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var employee = await _employeeRepository.GetEmployeeByIdAsync(id, cancellationToken);

                if (employee == null)
                {
                    _logger.LogInformation("Employee with ID {Id} not found", id);
                    return null;
                }

                var employeeResponse = employee.MapToResponse();
                _logger.LogInformation("Retrieved employee with ID {Id} from the database", id);

                return employeeResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving employee from the database");
                throw;
            }
        }

        public async Task<Guid> AddEmployeeAsync(CreateEmployeeRequest employeeRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var employee = employeeRequest.MapToEmployee();
                var employeeId = await _employeeRepository.AddEmployeeAsync(employee, cancellationToken);

                _logger.LogInformation("Added a new employee with ID {Id} to the database", employeeId);
                return employeeId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding a new employee to the database");
                throw;
            }
        }

        public async Task<bool> UpdateEmployeeAsync(Guid id, UpdateEmployeeRequest employeeRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id, cancellationToken);

                if (existingEmployee == null)
                {
                    _logger.LogInformation("Employee with ID {Id} not found", id);
                    return false;
                }

                var updatedEmployee = employeeRequest.MapToEmployee(id);
                var result = await _employeeRepository.UpdateEmployeeAsync(updatedEmployee, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Updated employee with ID {Id} in the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to update employee with ID {Id} in the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating employee in the database");
                throw;
            }
        }

        public async Task<bool> DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id, cancellationToken);

                if (existingEmployee == null)
                {
                    _logger.LogInformation("Employee with ID {Id} not found", id);
                    return false;
                }

                var result = await _employeeRepository.DeleteEmployeeAsync(id, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Deleted employee with ID {Id} from the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to delete employee with ID {Id} from the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting employee from the database");
                throw;
            }
        }
    }
}
