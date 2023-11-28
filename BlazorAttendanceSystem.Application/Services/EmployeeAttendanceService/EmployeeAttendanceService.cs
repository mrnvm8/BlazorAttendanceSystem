using BlazorAttendanceSystem.Application.Mapping;
using BlazorAttendanceSystem.Application.Repositories.EmployeeAttendancesRespository;
using BlazorAttendanceSystem.Contract.Requests.EmployeeAttendanceContract;
using BlazorAttendanceSystem.Contract.Responses;
using Microsoft.Extensions.Logging;
using Serilog;

namespace BlazorAttendanceSystem.Application.Services.EmployeeAttendanceService
{
    public class EmployeeAttendanceService : IEmployeeAttendanceService
    {
        private readonly IEmployeeAttendanceRepository _employeeAttendanceRepository;
        private readonly ILogger<EmployeeAttendanceService> _logger;

        public EmployeeAttendanceService(IEmployeeAttendanceRepository employeeAttendanceRepository, 
            ILogger<EmployeeAttendanceService> logger)
        {
            _employeeAttendanceRepository = employeeAttendanceRepository ?? throw new ArgumentNullException(nameof(employeeAttendanceRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<EmployeeAttendanceResponse>> GetAllEmployeeAttendancesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var employeeAttendances = await _employeeAttendanceRepository.GetAllEmployeeAttendancesAsync(cancellationToken);
                var employeeAttendanceResponses = employeeAttendances.Select(employeeAttendance => employeeAttendance.MapToResponse());

                _logger.LogInformation("Retrieved {Count} employee attendances from the database", employeeAttendanceResponses.Count());
                return employeeAttendanceResponses;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving employee attendances from the database");
                throw;
            }
        }

        public async Task<EmployeeAttendanceResponse?> GetEmployeeAttendanceByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var employeeAttendance = await _employeeAttendanceRepository.GetEmployeeAttendanceByIdAsync(id, cancellationToken);

                if (employeeAttendance == null)
                {
                    _logger.LogInformation("Employee attendance with ID {Id} not found", id);
                    return null;
                }

                var employeeAttendanceResponse = employeeAttendance.MapToResponse();
                _logger.LogInformation("Retrieved employee attendance with ID {Id} from the database", id);

                return employeeAttendanceResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving employee attendance from the database");
                throw;
            }
        }

        public async Task<Guid> AddEmployeeAttendanceAsync(CreateEmployeeAttendanceRequest employeeAttendanceRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var employeeAttendance = employeeAttendanceRequest.MapToEmployeeAttendance();
                var employeeAttendanceId = await _employeeAttendanceRepository.AddEmployeeAttendanceAsync(employeeAttendance, cancellationToken);

                _logger.LogInformation("Added a new employee attendance with ID {Id} to the database", employeeAttendanceId);
                return employeeAttendanceId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding a new employee attendance to the database");
                throw;
            }
        }

        public async Task<bool> UpdateEmployeeAttendanceAsync(Guid id, UpdateEmployeeAttendanceRequest employeeAttendanceRequest, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingEmployeeAttendance = await _employeeAttendanceRepository.GetEmployeeAttendanceByIdAsync(id, cancellationToken);

                if (existingEmployeeAttendance == null)
                {
                    _logger.LogInformation("Employee attendance with ID {Id} not found", id);
                    return false;
                }

                var updatedEmployeeAttendance = employeeAttendanceRequest.MapToEmployeeAttendance(id);
                var result = await _employeeAttendanceRepository.UpdateEmployeeAttendanceAsync(updatedEmployeeAttendance, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Updated employee attendance with ID {Id} in the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to update employee attendance with ID {Id} in the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating employee attendance in the database");
                throw;
            }
        }

        public async Task<bool> DeleteEmployeeAttendanceAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var existingEmployeeAttendance = await _employeeAttendanceRepository.GetEmployeeAttendanceByIdAsync(id, cancellationToken);

                if (existingEmployeeAttendance == null)
                {
                    _logger.LogInformation("Employee attendance with ID {Id} not found", id);
                    return false;
                }

                var result = await _employeeAttendanceRepository.DeleteEmployeeAttendanceAsync(id, cancellationToken);

                if (result)
                {
                    _logger.LogInformation("Deleted employee attendance with ID {Id} from the database", id);
                }
                else
                {
                    _logger.LogWarning("Failed to delete employee attendance with ID {Id} from the database", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting employee attendance from the database");
                throw;
            }
        }
    }
}
